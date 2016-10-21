using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMode = PR2PS.GameServer.GameConstants.GameMode;

namespace PR2PS.GameServer
{
    /// <summary>
    /// Class representing new race (game).
    /// </summary>
    public class Race
    {
        /// <summary>
        /// Enum representing possible statuses of this race, that is, initialized, started, etc.
        /// </summary>
        private enum GameStatus
        { 
            /// <summary>
            /// Game has just been created.
            /// </summary>
            NOT_INITIALIZED,
            /// <summary>
            /// Important variables have already been initialized, but game has not started yet.
            /// </summary>
            INITIALIZED,
            /// <summary>
            /// Game has started and is waiting until racers will download and draw map.
            /// </summary>
            STARTED,
            /// <summary>
            /// Race has officially begun.
            /// </summary>
            RUNNING
        }

        /// <summary>
        /// Date and time when the game has begun (racers can move).
        /// </summary>
        private DateTime gameStartTime;
        /// <summary>
        /// Level identifier (without _version).
        /// </summary>
        private String levelId;
        /// <summary>
        /// Indicates state of this game.
        /// </summary>
        private GameStatus gameStatus = GameStatus.NOT_INITIALIZED;
        /// <summary>
        /// Represents game mode of this game (race, dm, etc).
        /// TODO - Get true game mode from DB instead of relying on what clients say...
        /// </summary>
        private GameMode gameMode = GameMode.UNKNOWN;
        /// <summary>
        /// List of clients.
        /// </summary>
        private List<ConnectedClient> clients;
        /// <summary>
        /// List of racers. We will be using this instead of ConnectedClient for wrapping purposes.
        /// </summary>
        private List<Racer> racers;
        /// <summary>
        /// List of racers who finished/forfeited.
        /// </summary>
        private List<Racer> finRacers;
        /// <summary>
        /// Dictionary mapping hat ids to dropped hats.
        /// </summary>
        private Dictionary<String, Hat> looseHats;

        private Object clientsSyncLock;
        private Object racersSyncLock;
        private Object finRacersSyncLock;
        private Object hatsSyncLock;

        public Race(String levelId, List<ConnectedClient> clients)
        {
            this.gameStartTime = DateTime.UtcNow;
            this.levelId = levelId;
            this.clients = clients;

            this.racers = new List<Racer>();
            this.finRacers = new List<Racer>();
            this.looseHats = new Dictionary<String, Hat>();
            this.clientsSyncLock = new Object();
            this.racersSyncLock = new Object();
            this.finRacersSyncLock = new Object();
            this.hatsSyncLock = new Object();
        }

        /// <summary>
        /// Stores level id, initializes list of racers and sets status to INITIALIZED.
        /// </summary>
        public void Initialize()
        {
            if (this.gameStatus < GameStatus.INITIALIZED)
            {
                // Update levelId from levelId_version format to levelId.
                if (this.levelId.Contains('_'))
                {
                    this.levelId = this.levelId.Split(Separators.UNDERSCORE_SEPARATOR)[0];
                }

                // Fill list of racers and assign hats.
                lock (this.clientsSyncLock)
                {
                    lock (this.racersSyncLock)
                    {
                        lock (this.hatsSyncLock)
                        {
                            Int32 startingHatId = this.looseHats.Count;

                            for (Int32 idx = 0; idx < this.clients.Count; idx++)
                            {
                                this.clients[idx].Game = this;
                                Racer newRacer = new Racer(idx.ToString(), this.clients[idx]);
                                Hat newHat = new Hat
                                {   Id = startingHatId.ToString(),
                                    Type = newRacer.AccData.Hat.ToString(),
                                    PrimaryColor = newRacer.AccData.HatColor.ToString(),
                                    SecondaryColor = newRacer.AccData.HatColor2.ToString()
                                };
                                newRacer.AddHat(newHat);
                                this.racers.Add(newRacer);
                                startingHatId++;
                            }
                        }
                    }
                }

                this.gameStatus = GameStatus.INITIALIZED;
            }
            else
            { 
                // TODO - Log this. Initialize should only be called once.
            }
        }

        /// <summary>
        /// Sends startGame message so that client can begin downloading and rendering levels
        /// and also sends info about opponents. The race does not begin yet.
        /// Game status is set to STARTED.
        /// </summary>
        public void StartRace()
        {
            if (this.gameStatus == GameStatus.INITIALIZED)
            {
                this.BroadCast(String.Format("startGame`{0}", this.levelId));
                this.sendCreateCharacters();
                this.gameStatus = GameStatus.STARTED;
            }
            else
            { 
                // TODO - Log.
            }
        }

        /// <summary>
        /// Sends beginRace and setHats messages to all clients which starts the countdown.
        /// At this point, the race officialy begins. Game status is set to RUNNING.
        /// </summary>
        private void beginRace()
        {
            if (this.gameStatus == GameStatus.STARTED)
            {
                // TODO - Broadcast info about hats present in map from the beginning, i.e. Jiggmin hat.
                //lock (this.hatsSyncLock)
                //{
                //    foreach (KeyValuePair<String, Hat> droppedHat in this.looseHats)
                //    {
                //        this.BroadCast(
                //            String.Format(
                //                "addEffect`Hat`{0}`{1}`{2}`{3}`{4}",
                //                msgSegments[3],
                //                msgSegments[4],
                //                msgSegments[5],
                //                droppedHat.ToString(),
                //                droppedHat.Id
                //        ));
                //    }
                //}
                this.sendSetHats();
                this.BroadCast("beginRace`");
                this.gameStartTime = DateTime.UtcNow;
                this.gameStatus = GameStatus.RUNNING;
            }
            else
            { 
                // TODO - Log.
            }
        }

        /// <summary>
        /// Makes racer quit, broadcasts changes and stores time. Racer can still spectate.
        /// </summary>
        /// <param name="client">Client who forfeited.</param>
        public void Forfeit(ConnectedClient client)
        {
            if (this.gameStatus > GameStatus.INITIALIZED)
            {
                // Lets try to find physical racer who forfeited.
                Racer racer = null;
                lock (this.racersSyncLock)
                {
                    racer = this.racers.Find(r => r.Client == client);
                }

                if (racer == null)
                {
                    // None found, racer probably forfeited and quit already.

                    // TODO - Log this situation, because once client left this game he shouldnt initiate forfeit.
                }
                else
                {
                    // Racer found.

                    this.forfeit(racer);
                }
            }
            else
            { 
                // TODO - Log.
            }
        }

        /// <summary>
        /// Makes racer finish, broadcasts changes and stores time. Racer can still spectate.
        /// </summary>
        /// <param name="connectedClient">Client who finished.</param>
        /// <param name="msgSegments">FinishRace message in its raw format.</param>
        public void Finish(ConnectedClient client, String[] msgSegments)
        {
            // TODO - Check if msgSegments is valid.

            if (this.gameStatus > GameStatus.INITIALIZED)
            {
                // Lets try to find physical racer who forfeited.
                Racer racer = null;
                lock (this.racersSyncLock)
                {
                    racer = this.racers.Find(r => r.Client == client);
                }

                if (racer == null)
                {
                    // None found, racer probably forfeited and quit already.

                    // TODO - Log this situation, because once client left this game he shouldnt initiate forfeit.
                }
                else
                {
                    // Racer found.

                    // Check whether racer finished already.
                    if (racer.FinishedInfo == null)
                    {
                        // He did not, lets make him forfeit.

                        racer.FinishedDrawing = true;
                        racer.FinishedInfo = new FinishedInfo(DateTime.UtcNow - this.gameStartTime, true);
                        lock (this.finRacersSyncLock)
                        {
                            this.finRacers.Add(racer);
                        }

                        this.postFinishTasks(racer);
                    }
                    else
                    {
                        // Racer finished already, ignore forfeit attempt.

                        // TODO - Log this situation, because once client finished, then he shouldnt be capable
                        // of sending another forfeit request.
                    }
                }
            }
            else
            {
                // TODO - Log.
            }
        }

        /// <summary>
        /// Makes racer quit, broadcasts changes and stores time. Racer can still spectate.
        /// </summary>
        /// <param name="racer">Racer who forfeited.</param>
        private void forfeit(Racer racer)
        {
            if (this.gameStatus > GameStatus.INITIALIZED)
            {
                // Check whether racer finished already.
                if (racer.FinishedInfo == null)
                {
                    // He did not, lets make him forfeit.

                    racer.FinishedDrawing = true;
                    racer.FinishedInfo = new FinishedInfo(DateTime.UtcNow - this.gameStartTime, false);
                    lock (this.finRacersSyncLock)
                    {
                        this.finRacers.Add(racer);
                    }

                    this.postFinishTasks(racer);
                }
                else
                {
                    // Racer finished already, ignore forfeit attempt.

                    // TODO - Log this situation, because once client finished, then he shouldnt be capable
                    // of sending another forfeit request.
                }
            }
            else
            {
                // TODO - Log.
            }
        }

        /// <summary>
        /// Set of actions that need to be performed when someone will either forfeit or finish.
        /// Such actions include finish drawing checks, broadcasting finish times broadcasting awards, etc.
        /// </summary>
        /// <param name="racer">Racer who finished.</param>
        private void postFinishTasks(Racer racer)
        {
            if (this.gameStatus > GameStatus.INITIALIZED)
            {
                if (this.gameStatus < GameStatus.RUNNING)
                {
                    this.sendFinishDrawing(racer);
                    this.checkFinishDrawing();
                }

                // TODO - Award...

                this.sendFinishTimes();
            }
        }

        /// <summary>
        /// Sends finishDrawing message to all racers.
        /// </summary>
        /// <param name="racer">Racer, who finished drawing.</param>
        private void sendFinishDrawing(Racer racer)
        {
            this.BroadCast(String.Concat("finishDrawing`", racer.Position));
        }

        /// <summary>
        /// Checks whether everyone has already finished drawing and if so, then race can begin.
        /// </summary>
        private void checkFinishDrawing()
        {
            Int32 finishDrawingLeft = 0;

            lock (this.racersSyncLock)
            {
                finishDrawingLeft = this.racers.FindAll(racer => racer.FinishedDrawing == false).Count;
            }

            if (finishDrawingLeft == 0)
            { 
                // Everyone has finished drawing, lets begin race!

                this.beginRace();
            }
        }

        /// <summary>
        /// Sends setHats message to all racers in this game.
        /// </summary>
        private void sendSetHats()
        {
            lock (this.racersSyncLock)
            {
                foreach (Racer racer in this.racers)
                {
                    this.BroadCast(String.Concat("setHats", racer.Position, racer.GetHatsInfo()));
                }
            }
        }

        /// <summary>
        /// Sends finishTimes message to all racers in this game.
        /// </summary>
        private void sendFinishTimes()
        {
            StringBuilder strBuilder = new StringBuilder("finishTimes");

            lock (this.finRacersSyncLock)
            {
                foreach (Racer racer in this.finRacers)
                {
                    strBuilder.Append(
                        String.Format(
                        "`{0}`{1}`{2}",
                        racer.AccData.Username,
                        racer.FinishedInfo.Value.Completed ? racer.FinishedInfo.Value.FinishTime.TotalSeconds.ToString().Replace(Separators.COMMA_CHAR, Separators.PERIOD_CHAR) : StatusMessages.FORFEIT,
                        racer.Client != null ? StatusMessages.ONE : String.Empty
                        ));
                }
            }

            this.BroadCast(strBuilder.ToString());
        }

        /// <summary>
        /// Removes client from this game and also makes him forfeit if he didnt already.
        /// </summary>
        /// <param name="client">Client to remove.</param>
        public void RemoveClient(ConnectedClient client)
        {
            client.Game = null;

            lock (this.clientsSyncLock)
            {
                this.clients.Remove(client);
            }

            if (this.gameStatus > GameStatus.NOT_INITIALIZED)
            {
                Racer racerToRemove;

                lock (this.racersSyncLock)
                {
                    racerToRemove = this.racers.Find(racer => racer.Client == client);
                }

                if (racerToRemove != null)
                {
                    racerToRemove.RemoveClient();
                    if (racerToRemove.FinishedInfo == null)
                    {
                        this.forfeit(racerToRemove);
                    }
                    else
                    {
                        sendFinishTimes();
                    }
                }
            }

            // TODO time, etc...
        }

        /// <summary>
        /// Sends createLocal and createRemote characters to everyone.
        /// </summary>
        private void sendCreateCharacters()
        {
            lock (this.racersSyncLock)
            {
                foreach (Racer me in this.racers)
                {
                    foreach (Racer other in this.racers)
                    {
                        if (me == other)
                        {
                            me.SendMessage(
                                String.Format(
                                "createLocalCharacter`{0}`{1}`{2}`{3}`{4}`{5}`{6}`{7}`{8}`{9}`{10}`{11}`{12}`{13}`{14}`{15}",
                                me.Position,
                                me.AccData.Speed,
                                me.AccData.Accel,
                                me.AccData.Jump,
                                me.AccData.HatColor,
                                me.AccData.HeadColor,
                                me.AccData.BodyColor,
                                me.AccData.FeetColor,
                                me.AccData.Hat,
                                me.AccData.Head,
                                me.AccData.Body,
                                me.AccData.Feet,
                                me.AccData.HatColor2,
                                me.AccData.HeadColor2,
                                me.AccData.BodyColor2,
                                me.AccData.FeetColor2
                                ));
                        }
                        else
                        {
                            me.SendMessage(
                                String.Format(
                                "createRemoteCharacter`{0}`{1}`{2}`{3}`{4}`{5}`{6}`{7}`{8}`{9}`{10}`{11}`{12}`{13}",
                                other.Position,
                                other.AccData.Username,
                                other.AccData.HatColor,
                                other.AccData.HeadColor,
                                other.AccData.BodyColor,
                                other.AccData.FeetColor,
                                other.AccData.Hat,
                                other.AccData.Head,
                                other.AccData.Body,
                                other.AccData.Feet,
                                other.AccData.HatColor2,
                                other.AccData.HeadColor2,
                                other.AccData.BodyColor2,
                                other.AccData.FeetColor2
                                ));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends message to all racers.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void BroadCast(String message)
        {
            lock (this.racersSyncLock)
            {
                foreach (Racer racer in this.racers)
                {
                    racer.SendMessage(message);
                }
            }
        }

        /// <summary>
        /// Notify other racers that this one has finihed drawing and check whether race can begin.
        /// </summary>
        /// <param name="connectedClient">Client who finished drawing.</param>
        /// <param name="msgSegments">FinishDrawing message in its raw format.</param>
        public void HandleFinishDrawing(ConnectedClient connectedClient, String[] msgSegments)
        {
            Racer foundRacer = null;

            lock (this.racersSyncLock)
            {
                foundRacer = this.racers.Find(racer => racer.Client == connectedClient);
            }

            if (foundRacer != null)
            {
                foundRacer.FinishedDrawing = true;
                this.gameMode = GameConstants.GameModeMap[msgSegments[4]]; // TODO - From DB instead from client.

                this.sendFinishDrawing(foundRacer);
                this.checkFinishDrawing();
            }
        }

        /// <summary>
        /// Broadcasts position change of respective racer.
        /// </summary>
        /// <param name="connectedClient">Client who sent position update message.</param>
        /// <param name="msgSegments">Raw data of message p.</param>
        public void BroadCastPosition(ConnectedClient connectedClient, String[] msgSegments)
        {
            lock(this.racersSyncLock)
            {
                Racer me = this.racers.Find(racer => racer.Client == connectedClient);

                foreach (Racer racer in this.racers)
                {
                    if (racer != me)
                    {
                        racer.SendMessage(String.Format("p{0}`{1}`{2}", me.Position, msgSegments[3], msgSegments[4]));
                    }
                }
            }
        }

        /// <summary>
        /// Broadcasts exact position change of respective racer.
        /// </summary>
        /// <param name="connectedClient">Client who sent position update message.</param>
        /// <param name="msgSegments">Raw data of message p.</param>
        public void BroadCastExactPosition(ConnectedClient connectedClient, String[] msgSegments)
        {
            lock (this.racersSyncLock)
            {
                Racer me = this.racers.Find(racer => racer.Client == connectedClient);

                foreach (Racer racer in this.racers)
                {
                    if (racer != me)
                    {
                        racer.SendMessage(String.Format("exactPos{0}`{1}`{2}", me.Position, msgSegments[3], msgSegments[4]));
                    }
                }
            }
        }

        /// <summary>
        /// Broadcasts state change of respective racer.
        /// </summary>
        /// <param name="connectedClient">Client who sent state update message.</param>
        /// <param name="msgSegments">Raw data of message set_var.</param>
        public void BroadCastVar(ConnectedClient connectedClient, String[] msgSegments)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 3; i < msgSegments.Length; i++)
            {
                strBuilder.Append(Separators.ARG_CHAR);
                strBuilder.Append(msgSegments[i]);
            }
            String varData = strBuilder.ToString();

            lock (this.racersSyncLock)
            {
                Racer me = this.racers.Find(racer => racer.Client == connectedClient);

                foreach (Racer racer in this.racers)
                {
                    if (racer != me)
                    {
                        racer.SendMessage(String.Format("var{0}{1}", me.Position, varData));
                    }
                }
            }
        }

        /// <summary>
        /// Broadcasts add effect.
        /// </summary>
        /// <param name="connectedClient">Client who sent add effect message.</param>
        /// <param name="msgSegments">Raw data of message set_var.</param>
        public void BroadCastEffect(ConnectedClient connectedClient, String[] msgSegments)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 3; i < msgSegments.Length; i++)
            {
                strBuilder.Append(Separators.ARG_CHAR);
                strBuilder.Append(msgSegments[i]);
            }
            String effectData = strBuilder.ToString();

            lock (this.racersSyncLock)
            {
                foreach (Racer racer in this.racers)
                {
                    if (racer.Client != connectedClient)
                    {
                        racer.SendMessage(String.Format("addEffect{0}", effectData));
                    }
                }
            }
        }

        /// <summary>
        /// Broadcasts activate.
        /// </summary>
        /// <param name="connectedClient">Client who sent activate message.</param>
        /// <param name="msgSegments">Raw data of message activate.</param>
        public void BroadCastActivate(ConnectedClient connectedClient, String[] msgSegments)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 3; i < msgSegments.Length; i++)
            {
                strBuilder.Append(Separators.ARG_CHAR);
                strBuilder.Append(msgSegments[i]);
            }
            String activateData = strBuilder.ToString();

            lock (this.racersSyncLock)
            {
                foreach (Racer racer in this.racers)
                {
                    if (racer.Client != connectedClient)
                    {
                        racer.SendMessage(String.Format("activate{0}`", activateData));
                    }
                }
            }
        }

        /// <summary>
        /// Broadcasts zap.
        /// </summary>
        /// <param name="connectedClient">Client who sent zap.</param>
        public void BroadCastZap(ConnectedClient connectedClient)
        {
            lock (this.racersSyncLock)
            {
                Racer me = this.racers.Find(racer => racer.Client == connectedClient);

                foreach (Racer racer in this.racers)
                {
                    if (racer != me)
                    {
                        racer.SendMessage("zap`");
                    }
                }
            }
        }

        /// <summary>
        /// Makes client drop his hat and broadcast changes to everyone.
        /// Dropped hat will be added to dictionary dropped hats.
        /// </summary>
        /// <param name="connectedClient">Client who lost hat.</param>
        /// <param name="msgSegments">Message loose_hat in its raw format.</param>
        public void LooseHat(ConnectedClient connectedClient, String[] msgSegments)
        {
            lock (this.racersSyncLock)
            {
                Racer me = this.racers.Find(racer => racer.Client == connectedClient);
                if (me == null) return;

                Hat droppedHat = me.RemoveHat();

                if (droppedHat != null)
                {
                    lock (this.hatsSyncLock)
                    {
                        this.looseHats.Add(droppedHat.Id, droppedHat);
                    }

                    this.BroadCast(
                        String.Format(
                            "addEffect`Hat`{0}`{1}`{2}`{3}`{4}",
                            msgSegments[3],
                            msgSegments[4],
                            msgSegments[5],
                            droppedHat.ToString(),
                            droppedHat.Id
                    ));
                    this.BroadCast(String.Concat("setHats", me.Position, me.GetHatsInfo()));
                }
            }
        }

        /// <summary>
        /// Attempts to pick up hat. Once done update will be broadcasted.
        /// </summary>
        /// <param name="connectedClient">Client who wants to pick up hat.</param>
        /// <param name="hatId">Id of the hat.</param>
        public void GetHat(ConnectedClient connectedClient, String hatId)
        {
            lock (this.hatsSyncLock)
            {
                Hat foundHat;

                if (this.looseHats.TryGetValue(hatId, out foundHat))
                {
                    lock (this.racersSyncLock)
                    {
                        Racer me = this.racers.Find(racer => racer.Client == connectedClient);
                        if (me == null) return;

                        if (foundHat.Type == "12")
                        {
                            // Racer picked up thief hat. Need to handle separately.

                            Racer swapRacer = this.findNextRacerForHatSwap(me);
                            if (swapRacer == null)
                            {
                                // No candidate for hat swap found. Just pick up the hat.

                                this.looseHats.Remove(hatId);
                                me.AddHat(foundHat);

                                this.BroadCast(String.Format("removeHat{0}`", foundHat.Id));
                                this.BroadCast(String.Concat("setHats", me.Position, me.GetHatsInfo()));
                            }
                            else
                            { 
                                // Candidate found. Lets swap hats.

                                this.looseHats.Remove(hatId);
                                Hat swapRacerHat = swapRacer.RemoveHat();

                                if (swapRacerHat == null)
                                {
                                    me.AddHat(foundHat);
                                }
                                else
                                {
                                    me.AddHat(swapRacerHat);
                                    swapRacer.AddHat(foundHat);
                                }

                                this.BroadCast(String.Format("removeHat{0}`", foundHat.Id));
                                this.BroadCast(String.Concat("setHats", me.Position, me.GetHatsInfo()));
                                this.BroadCast(String.Concat("setHats", swapRacer.Position, swapRacer.GetHatsInfo()));
                            }
                        }
                        else
                        {
                            this.looseHats.Remove(hatId);
                            me.AddHat(foundHat);

                            this.BroadCast(String.Format("removeHat{0}`", foundHat.Id));
                            this.BroadCast(String.Concat("setHats", me.Position, me.GetHatsInfo()));
                        }
                    }
                }
            }
        }

        private Racer findNextRacerForHatSwap(Racer me)
        {
            lock (this.racersSyncLock)
            {
                Int32 myPosition;
                if (!Int32.TryParse(me.Position, out myPosition)
                    || (myPosition < 0 || myPosition >= this.racers.Count))
                {
                    return null;
                }

                for (Int32 i = myPosition + 1; i < this.racers.Count; i++)
                {
                    if (this.racers[i].HasAnyHat()) return racers[i];
                }

                for (Int32 i = 0; i < myPosition; i++)
                {
                    if (this.racers[i].HasAnyHat()) return racers[i];                    
                }

                return null;
            }
        }
    }

    /// <summary>
    /// Class representing ingame racer. Serves as wrapper.
    /// </summary>
    public class Racer
    {
        /// <summary>
        /// Reference to the actual client. If its null then racer is gone.
        /// </summary>
        public ConnectedClient Client { get; private set; }
        /// <summary>
        /// Reference to the account data of the racer. We need this because when client leaves/dcs,
        /// we still require his name, rank and other stuff.
        /// </summary>
        public AccountDataDTO AccData { get; private set; }
        /// <summary>
        /// Represents position of the racer according to fill queue.
        /// </summary>
        public String Position { get; private set; }
        /// <summary>
        /// Indicates, whether this racer already finished drawing.
        /// </summary>
        public Boolean FinishedDrawing { get; set; }
        /// <summary>
        /// Finish race details or null if racer is still playing.
        /// </summary>
        public FinishedInfo? FinishedInfo { get; set; }
        
        /// <summary>
        /// Stack of equipped hats.
        /// </summary>
        private Stack<Hat> myHats;

        private Object hatsSyncLock;

        public Racer(String position, ConnectedClient client)
        {
            this.Position = position;
            this.Client = client;
            this.AccData = client.AccData;
            this.myHats = new Stack<Hat>();
            this.hatsSyncLock = new Object();
        }

        /// <summary>
        /// Adds hat into stack of equipped hats.
        /// </summary>
        /// <param name="hat">Added hat.</param>
        public void AddHat(Hat hat)
        {
            lock (this.hatsSyncLock)
            {
                this.myHats.Push(hat);
            }
        }

        /// <summary>
        /// Pops hat from stack of equipped hats.
        /// </summary>
        /// <returns>Popped hat or null if none.</returns>
        public Hat RemoveHat()
        {
            lock (this.hatsSyncLock)
            {
                return (this.myHats.Count > 0) ? this.myHats.Pop() : null;
            }
        }

        /// <summary>
        /// Checks if this racer has any hats at all.
        /// </summary>
        /// <returns>True if he has, false otherwise.</returns>
        public Boolean HasAnyHat()
        {
            lock (this.hatsSyncLock)
            {
                return this.myHats.Any();
            }
        }

        /// <summary>
        /// Returns info about equipped hats in following format:
        /// `firstHatType`firstHatprimaryColor`firstHatsecondaryColor`secondHatType`secondHatprimaryColor`secondHatsecondaryColor`...
        /// </summary>
        /// <returns>String representation of equipped hats in ready to send format.</returns>
        public String GetHatsInfo()
        {
            StringBuilder strBuilder = new StringBuilder();

            lock (this.hatsSyncLock)
            {
                if (this.myHats.Count == 0)
                {
                    return Separators.ARG_CHAR.ToString();
                }

                for (Int32 hatId = this.myHats.Count -1; hatId >= 0; hatId--)
                {
                    strBuilder.Append(Separators.ARG_CHAR);
                    strBuilder.Append(this.myHats.ElementAt(hatId).ToString());
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Sets reference of connected client to null.
        /// We still have access to customize info and hats.
        /// </summary>
        public void RemoveClient()
        {
            this.Client = null;
        }

        /// <summary>
        /// Checks whether client still present in this game and if so, sends message to him.
        /// </summary>
        /// <param name="message">String message.</param>
        public void SendMessage(String message)
        {
            if (this.Client != null)
            {
                this.Client.SendMessage(message);
            }
        }
    }

    /// <summary>
    /// Class representing ingame hat.
    /// </summary>
    public class Hat
    {
        /// <summary>
        /// Hat identifier in this game.
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// Hat type.
        /// </summary>
        public String Type { get; set; }
        /// <summary>
        /// Primary color.
        /// </summary>
        public String PrimaryColor { get; set; }
        /// <summary>
        /// Secondary color.
        /// </summary>
        public String SecondaryColor { get; set; }

        /// <summary>
        /// Returns string representation of this hat.
        /// Format: hatType`primaryColor`secondaryColor
        /// </summary>
        /// <returns>String representation of this hat.</returns>
        public override String ToString()
        {
            return String.Format("{0}`{1}`{2}", this.Type, this.PrimaryColor, this.SecondaryColor);
        }
    }

    /// <summary>
    /// Wrapper struct containing information about time when racer finished and also flag whether he completed/quit.
    /// </summary>
    public struct FinishedInfo
    {
        /// <summary>
        /// Time when racer finished/ended game (duration).
        /// </summary>
        public TimeSpan FinishTime { get; private set; }

        /// <summary>
        /// True if racer has finished the game, false if he forfeited.
        /// </summary>
        public Boolean Completed { get; private set; }

        /// <summary>
        /// Struct constructor.
        /// </summary>
        /// <param name="finishTime">Relative time when racer ended race.</param>
        /// <param name="completed">True if racer completed the game or false if he forfeited.</param>
        public FinishedInfo(TimeSpan finishTime, Boolean completed)
            : this()
        {
            this.FinishTime = finishTime;
            this.Completed = completed;
        }
    }
}
