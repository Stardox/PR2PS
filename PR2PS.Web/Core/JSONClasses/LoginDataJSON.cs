using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR2PS.Web.Core.JSONClasses
{
    public class LoginDataJSON
    {
        public Boolean Remember { get; set; }
        public String User_name { get; set; }
        public String Domain { get; set; }
        public Int32 Login_id { get; set; }
        public String Version { get; set; }
        public String User_pass { get; set; }
        public ServerJSON Server { get; set; }
        public String Login_code { get; set; }
    }
}