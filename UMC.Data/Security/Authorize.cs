using System;
using System.Collections.Generic;
using System.Text;

namespace UMC.Security
{
    public class AuthorizeType
    {
        public const string UserAllow = "UserAllow";
        public const string UserDeny = "UserDeny";
        public const string RoleAllow = "RoleAllow";
        public const string RoleDeny = "RoleDeny";
       //void d(){ new  List<String>()./
    }
    public class Authorize
    {
        public String Type
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
    }
}
