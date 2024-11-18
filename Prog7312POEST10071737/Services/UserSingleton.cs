using Prog7312POEST10071737.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prog7312POEST10071737.Services
{
    public class UserSingleton
    {
        /// <summary>
        /// Defines the current user.
        /// </summary>
        private User CurrentUser;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Defines the list of issue reports.
        /// </summary>
        public ObservableCollection<IssueReport> IssueReports { get; } = new ObservableCollection<IssueReport>();
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Defines the instance of the user singleton.
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
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private UserSingleton() { }

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
        /// Checks if a user is currently logged in.
        /// </summary>
        /// <returns>True if a user exists, false otherwise.</returns>
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
        /// Adds an issue report without subscribing a user.
        /// </summary>
        public void AddIssueReport(string description, string location, List<UploadedFile> mediaPaths, string category)
        {
            IssueReports.Add(new IssueReport(description, location, mediaPaths, category));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds an issue report with a subscribed user.
        /// </summary>
        public void AddIssueReport(string description, string location, List<UploadedFile> mediaPaths, string category, Guid user)
        {
            IssueReports.Add(new IssueReport(description, location, mediaPaths, category, user));
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
}//____________________________________EOF_________________________________________________________________________
