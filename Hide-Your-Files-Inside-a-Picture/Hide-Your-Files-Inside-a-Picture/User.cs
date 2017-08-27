using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hide_Your_Files_Inside_a_Picture
{
    public class User
    {
        public string name { get; set; }
        public string password { get; set; }
        public string tokenName { get; private set; }

        public User()
        {

        }

        public User(string name, string password)
        {
            this.name = name;
            this.password = password;
            tokenName = name + "_token.log";
        }
    }
}
