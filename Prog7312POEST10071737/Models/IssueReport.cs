using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prog7312POEST10071737.Models
{
    /// <summary>
    /// Enum for the status of the report.
    /// </summary>
    public enum ReportStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }

    public class IssueReport : INotifyPropertyChanged
    {
        /// <summary>
        /// Event for property changed notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        //___________________________________________________________________________________________________________

        private Guid _id;
        private string _name;
        private string _description;
        private string _location;
        private List<UploadedFile> _mediaPaths;
        private ReportStatus _status;
        private string _category;
        private List<Guid> _subscribedUsers;
        private DateTime _dateCreated;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the unique identifier of the issue report.
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the description of the issue report.
        /// </summary>
        public string name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the description of the issue report.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the location of the issue report.
        /// </summary>
        public string Location
        {
            get => _location;
            set => SetField(ref _location, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the media paths associated with the issue report.
        /// </summary>
        public List<UploadedFile> MediaPaths
        {
            get => _mediaPaths;
            set => SetField(ref _mediaPaths, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the status of the issue report.
        /// </summary>
        public ReportStatus Status
        {
            get => _status;
            set => SetField(ref _status, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the category of the issue report.
        /// </summary>
        public string Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the list of subscribed users for the issue report.
        /// </summary>
        public List<Guid> SubscribedUsers
        {
            get => _subscribedUsers;
            set => SetField(ref _subscribedUsers, value);
        }
        /// <summary>
        /// Gets or sets the date and time when the issue report was created.
        /// </summary>
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetField(ref _dateCreated, value);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Default constructor for the issue report.
        /// </summary>
        public IssueReport()
        {
            MediaPaths = new List<UploadedFile>();
            SubscribedUsers = new List<Guid>();
        }

        /// <summary>
        /// Constructor for the issue report.
        /// </summary>
        public IssueReport(string description, string location, List<UploadedFile> mediaPaths, string categoryID)
        {
            Id = Guid.NewGuid();
            Description = description;
            Location = location;
            MediaPaths = mediaPaths;
            Status = ReportStatus.Pending;
            Category = categoryID;
            SubscribedUsers = new List<Guid>();
            DateCreated = DateTime.Now;
            name = location + "_" + DateTime.Now + "_" + categoryID;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Constructor for the issue report with a subscribed user.
        /// </summary>
        public IssueReport(string description, string location, List<UploadedFile> mediaPaths, string categoryID, Guid user)
            : this(description, location, mediaPaths, categoryID)
        {
            SubscribedUsers.Add(user);
        }

        /// <summary>
        /// Subscribes a user to the issue report.
        /// </summary>
        public void Subscribe(Guid userId)
        {
            if (!SubscribedUsers.Contains(userId))
            {
                SubscribedUsers.Add(userId);
                OnPropertyChanged(nameof(SubscribedUsers));
            }
        }

        /// <summary>
        /// Updates the status of the issue report.
        /// </summary>
        public void UpdateStatus(ReportStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Sets the field and raises property changed if needed.
        /// </summary>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________