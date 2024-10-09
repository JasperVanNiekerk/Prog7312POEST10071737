using System;

namespace Prog7312POEST10071737.Models
{
    public class User
    {
        /// <summary>
        /// defines the user class
        /// </summary>
        private string username;
        private string password;
        private string email;
        private Guid id;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        public User(string username, string password, string email)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            id = Guid.NewGuid();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns the user email
        /// </summary>
        /// <returns></returns>
        public string returnEmail()
        {
            return email;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns user Guid
        /// </summary>
        /// <returns></returns>
        public Guid returnGUID()
        {
            return id;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// sets the username
        /// </summary>
        /// <param name="username"></param>
        public void SetUsername(string username)
        {
            this.username = username;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// sets the password
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            this.password = password;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// sets the email
        /// </summary>
        /// <param name="email"></param>
        public void SetEmail(string email)
        {
            this.email = email;
        }
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________