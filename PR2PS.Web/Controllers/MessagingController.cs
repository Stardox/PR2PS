using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.Common.Extensions;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.JSONClasses;
using PR2PS.Web.Core.Management;
using PR2PS.DataAccess;
using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class MessagingController : ApiController
    {
        /// <summary>
        /// Gets private messages.
        /// </summary>
        /// <returns>List of private messages or error.</returns>
        [HttpPost]
        public HttpResponseMessage GetMessages([FromBody] GetMessagesFormModel getMessagesData)
        {
            try
            {
                if (getMessagesData == null
                    || !getMessagesData.Count.HasValue
                    || !getMessagesData.Start.HasValue)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = ErrorMessages.ERR_NO_QUERY_DATA
                    }));
                }

                if (String.IsNullOrEmpty(getMessagesData.Token)) getMessagesData.Token = String.Empty;

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(getMessagesData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = ErrorMessages.ERR_NOT_LOGGED_IN
                    }));
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account accModel = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    if (accModel == null)
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME
                        }));
                    }

                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new MessageListJSON()
                    {
                        Messages = accModel.Messages
                            .Where(m => !m.IsDeleted)
                            .OrderByDescending(m => m.DateSent)
                            .Skip(getMessagesData.Start.Value)
                            .Take(getMessagesData.Count.Value)
                            .Select(m => new MessageJSON
                            {
                                Message_id = m.Id,
                                Message = m.Content,
                                Time = m.DateSent,
                                User_id = m.Sender.Id,
                                Name = m.Sender.Username,
                                Group = m.Sender.Group
                            })
                            .ToList(),
                        Success = true
                    }));
                }             
            }
            catch(Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Sends private message to specified recipient.
        /// </summary>
        /// <returns>Status indicating whether action was successful.</returns>
        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody] SendMessageFormModel sendMessageData)
        {
            try
            {
                if (sendMessageData == null)
                {
                    return HttpResponseFactory.Response200Plain("error=No form data received.");
                }

                if (String.IsNullOrEmpty(sendMessageData.To_Name)) sendMessageData.To_Name = String.Empty;

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(sendMessageData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain("error=You are not logged in.");
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account sender = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    Account recipient = db.Accounts.FirstOrDefault(a => a.Username.ToUpper() == sendMessageData.To_Name.ToUpper());
                    
                    if (recipient == null)
                    {
                        return HttpResponseFactory.Response200Plain("error=Could not find a user with that name.");
                    }

                    recipient.Messages.Add(new Message
                    {
                        Sender = sender,
                        Recipient = recipient,
                        Content = sendMessageData.Message ?? String.Empty,
                        DateSent = DateTime.UtcNow.GetSecondsSinceUnixTime(),
                        IPAddress = this.Request.GetRemoteIPAddress()
                    });
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain("message=Your message was sent succesfully!");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response200Plain(ex.Message);
            }
        }

        /// <summary>
        /// Reports the message to the administration.
        /// TODO - Implement.
        /// </summary>
        /// <returns>Status indicating whether action was successful.</returns>
        [HttpPost]
        public HttpResponseMessage ReportMessage([FromBody] DeleteOrReportMessageFormModel reportMessageData)
        {
            try
            {
                if (reportMessageData == null
                    || !reportMessageData.Message_Id.HasValue)
                {
                    return HttpResponseFactory.Response200Plain("error=No form data received.");
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(reportMessageData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain("error=You are not logged in.");
                }

                return HttpResponseFactory.Response200Plain("error=This feature is not implemented yet.");
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Marks the message specified by message id as deleted.
        /// </summary>
        /// <returns>Status indicating whether action was successful.</returns>
        [HttpPost]
        public HttpResponseMessage DeleteMessage([FromBody] DeleteOrReportMessageFormModel reportMessageData)
        {
            try
            {
                if (reportMessageData == null
                    || !reportMessageData.Message_Id.HasValue)
                {
                    return HttpResponseFactory.Response200Plain("error=No form data received.");
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(reportMessageData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain("error=You are not logged in.");
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account accModel = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    Message message = accModel.Messages.FirstOrDefault(m => m.Id == reportMessageData.Message_Id.Value);
                    if (message == null)
                    {
                        return HttpResponseFactory.Response200Plain("error=Could not find such message.");
                    }

                    message.IsDeleted = true;
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain("success=true");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Deletes all messages from the inbox.
        /// </summary>
        /// <param name="token">Session token.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>Status indicating whether action was successful.</returns>
        [HttpGet]
        public HttpResponseMessage DeleteAllMessages(String token, String rand)
        {
            try
            {
                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain("error=You are not logged in.");
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account accModel = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    IEnumerable<Message> messages = accModel.Messages.Where(m => !m.IsDeleted);
                    
                    if (messages == null || !messages.Any())
                    {
                        return HttpResponseFactory.Response200Plain("error=You have no messages to delete.");
                    }

                    foreach (Message message in messages)
                    {
                        message.IsDeleted = true;
                    }
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain("success=true");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
