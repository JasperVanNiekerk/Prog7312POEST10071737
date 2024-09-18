using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prog7312POEST10071737.Models
{
    public enum ReportStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }

    public class IssueReport : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Guid _id;
        private string _description;
        private string _location;
        private List<string> _mediaPaths;
        private ReportStatus _status;
        private string _category;
        private List<Guid> _subscribedUsers;

        public Guid Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        public string Location
        {
            get => _location;
            set => SetField(ref _location, value);
        }

        public List<string> MediaPaths
        {
            get => _mediaPaths;
            set => SetField(ref _mediaPaths, value);
        }

        public ReportStatus Status
        {
            get => _status;
            set => SetField(ref _status, value);
        }

        public string Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }

        public List<Guid> SubscribedUsers
        {
            get => _subscribedUsers;
            set => SetField(ref _subscribedUsers, value);
        }

        public IssueReport(string description, string location, List<string> mediaPaths, string categoryID)
        {
            Id = Guid.NewGuid();
            Description = description;
            Location = location;
            MediaPaths = mediaPaths;
            Status = ReportStatus.Pending;
            Category = categoryID;
            SubscribedUsers = new List<Guid>();
        }

        public IssueReport(string description, string location, List<string> mediaPaths, string categoryID, Guid user)
            : this(description, location, mediaPaths, categoryID)
        {
            SubscribedUsers.Add(user);
        }

        public void Subscribe(Guid userId)
        {
            SubscribedUsers.Add(userId);
            OnPropertyChanged(nameof(SubscribedUsers));
        }

        public void UpdateStatus(ReportStatus status)
        {
            Status = status;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}