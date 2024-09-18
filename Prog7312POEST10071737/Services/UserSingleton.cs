using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prog7312POEST10071737.Models;

namespace Prog7312POEST10071737.Services
{
    public class UserSingleton
    {

        private User CurrentUser;

        public ObservableCollection<IssueReport> IssueReports { get; } = new ObservableCollection<IssueReport>();

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

        private UserSingleton()
        {
            
        }

        public Guid GetUserGUID()
        {
            return CurrentUser.returnGUID();
        }

        public ObservableCollection<IssueReport> GetIssueReports()
        {
            return IssueReports;
        }

        public bool UserExists()
        {
            if (CurrentUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SetUser()
        {
            CurrentUser = new User("", "", "");
        }

        public void UpdateCurrentUser(string UN, string P, string E)
        {
            CurrentUser.SetUsername(UN);
            CurrentUser.SetPassword(P);
            CurrentUser.SetEmail(E);
        }

        public void AddIssueReport(string description, string location, List<string> MediaPath, string category)
        {
            IssueReports.Add(new IssueReport(description, location, MediaPath, category));
        }

        public void AddIssueReport(string description, string location, List<string> MediaPath, string category, Guid user)
        {
            IssueReports.Add(new IssueReport(description, location, MediaPath, category, user));
        }

        public void SubscribeToIssue(Guid issueId)
        {
            var issue = IssueReports.FirstOrDefault(x => x.Id == issueId);
            if (issue != null)
            {
                issue.Subscribe(CurrentUser.returnGUID());
            }
        }

        public string GetEmail()
        {
            return CurrentUser.returnEmail();
        }
    }
}
