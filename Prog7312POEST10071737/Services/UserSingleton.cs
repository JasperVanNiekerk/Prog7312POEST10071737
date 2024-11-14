using Prog7312POEST10071737.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prog7312POEST10071737.Services
{
    public class UserSingleton
    {
        /// <summary>
        /// defines the current user
        /// </summary>
        private User CurrentUser;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// defines the list of issue reports
        /// </summary>
        public ObservableCollection<IssueReport> IssueReports { get; } = new ObservableCollection<IssueReport>();
        //___________________________________________________________________________________________________________

        /// <summary>
        /// defines the instance of the user singleton
        /// </summary>
        private static UserSingleton instance = null;

        public static UserSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserSingleton();
                }
                return instance;
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// constructor for the user singleton
        /// </summary>
        private UserSingleton()
        {

        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns the current user GUID
        /// </summary>
        /// <returns></returns>
        public Guid GetUserGUID()
        {
            return CurrentUser.returnGUID();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns the list of issue reports
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<IssueReport> GetIssueReports()
        {
            return IssueReports;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to check if the user exists
        /// </summary>
        /// <returns></returns>
        public bool UserExists()
        {
            return CurrentUser != null;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to create a new user
        /// </summary>
        /// <param name="UN"></param>
        /// <param name="P"></param>
        /// <param name="E"></param>
        public void UpdateCurrentUser(string UN, string P, string E)
        {
            CurrentUser.SetUsername(UN);
            CurrentUser.SetPassword(P);
            CurrentUser.SetEmail(E);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to add a issue report
        /// </summary>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="MediaPath"></param>
        /// <param name="category"></param>
        public void AddIssueReport(string description, string location, List<byte[]> MediaPath, string category)
        {
            IssueReports.Add(new IssueReport(description, location, MediaPath, category));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to add a issue report with subscribed user
        /// </summary>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="MediaPath"></param>
        /// <param name="category"></param>
        /// <param name="user"></param>
        public void AddIssueReport(string description, string location, List<byte[]> MediaPath, string category, Guid user)
        {
            IssueReports.Add(new IssueReport(description, location, MediaPath, category, user));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to subscribe the current user to an issue
        /// </summary>
        /// <param name="issueId"></param>
        public void SubscribeToIssue(Guid issueId)
        {
            var issue = IssueReports.FirstOrDefault(x => x.Id == issueId);
            if (issue != null)
            {
                issue.Subscribe(CurrentUser.returnGUID());
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to get the current user email
        /// </summary>
        /// <returns></returns>
        public string GetEmail()
        {
            return CurrentUser.returnEmail();
        }
        //___________________________________________________________________________________________________________

        public bool IsLoggedIn => CurrentUser != null;

        public event EventHandler LoginStateChanged;

        /// <summary>
        /// Creates a new user and sets it as the current user
        /// </summary>
        /// <param name="username">Username for the new user</param>
        /// <param name="password">Password for the new user</param>
        /// <param name="email">Email for the new user</param>
        /// <returns>True if user creation was successful</returns>
        public bool CreateUser(string username, string password, string email)
        {
            try
            {
                CurrentUser = new User(username, password, email);
                LoginStateChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="user">The user to log in</param>
        public void Login(User user)
        {
            CurrentUser = user;
            LoginStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            LoginStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the email of the current user
        /// </summary>
        /// <returns>The email of the current user or empty string if no user is logged in</returns>
        public string GetCurrentUserEmail()
        {
            return CurrentUser?.returnEmail() ?? string.Empty;
        }
    }
}
//____________________________________EOF_________________________________________________________________________