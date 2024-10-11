using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prog7312POEST10071737.Models
{
    /// <summary>
    /// enum for the status of the report
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
        /// declare the event for the property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// declare the fields for the issue report
        /// </summary>
        private Guid _id;
        private string _description;
        private string _location;
        private List<byte[]> _mediaPaths;
        private ReportStatus _status;
        private string _category;
        private List<Guid> _subscribedUsers;
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
        public List<byte[]> MediaPaths
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// default constructor for the issue report
        /// </summary>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="mediaPaths"></param>
        /// <param name="categoryID"></param>
        public IssueReport(string description, string location, List<byte[]> mediaPaths, string categoryID)
        {
            Id = Guid.NewGuid();
            Description = description;
            Location = location;
            MediaPaths = mediaPaths;
            Status = ReportStatus.Pending;
            Category = categoryID;
            SubscribedUsers = new List<Guid>();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// constructor for the issue report with a user
        /// </summary>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="mediaPaths"></param>
        /// <param name="categoryID"></param>
        /// <param name="user"></param>
        public IssueReport(string description, string location, List<byte[]> mediaPaths, string categoryID, Guid user)
            : this(description, location, mediaPaths, categoryID)
        {
            SubscribedUsers.Add(user);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// subscribe a user to the issue report
        /// </summary>
        /// <param name="userId"></param>
        public void Subscribe(Guid userId)
        {
            SubscribedUsers.Add(userId);
            OnPropertyChanged(nameof(SubscribedUsers));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// update the status of the issue report
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(ReportStatus status)
        {
            Status = status;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// event handler for the property changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// set the field of the issue report
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
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