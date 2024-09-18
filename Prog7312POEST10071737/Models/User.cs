using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog7312POEST10071737.Models
{
    public class User
    {
        private string username;
        private string password;
        private string email;
        private Guid id;

        public User(string username, string password, string email)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            id = Guid.NewGuid();
        }

        public string returnEmail()
        {
            return email;
        }

        public Guid returnGUID()
        {
            return id;
        }

        

        public void SetUsername(string username)
        {
            this.username = username;
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public void SetId(Guid id)
        {
            this.id = id;
        }
    }
}
