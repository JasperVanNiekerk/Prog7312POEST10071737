using System;
using System.Linq;
using System.Windows.Controls;
using Prog7312POEST10071737.Services;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Interaction logic for BasicTree.xaml
    /// </summary>
    public partial class BasicTree : UserControl
    {
        private UserSingleton _userSingleton;

        public BasicTree()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            LoadIssueReports();
            // subscribe to collection changed because IssueReports can be modified at runtime
            _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;
        }

        private void LoadIssueReports()
        {
            IssueReportsTreeView.Items.Clear();

            foreach (var report in _userSingleton.IssueReports)
            {
                TreeViewItem reportItem = new TreeViewItem
                {
                    Header = $"Issue Report {_userSingleton.IssueReports.IndexOf(report) + 1}",
                    IsExpanded = false
                };

                // ID
                TreeViewItem idItem = new TreeViewItem
                {
                    Header = $"ID: {report.Id}",
                    IsExpanded = false
                };
                reportItem.Items.Add(idItem);

                // Description
                TreeViewItem descriptionItem = new TreeViewItem
                {
                    Header = $"Description: {report.Description}",
                    IsExpanded = false
                };
                reportItem.Items.Add(descriptionItem);

                // Location
                TreeViewItem locationItem = new TreeViewItem
                {
                    Header = $"Location: {report.Location}",
                    IsExpanded = false
                };
                reportItem.Items.Add(locationItem);

                // Media Paths
                TreeViewItem mediaPathsItem = new TreeViewItem
                {
                    Header = "Media Paths:",
                    IsExpanded = false
                };

                if (report.MediaPaths != null && report.MediaPaths.Any())
                {
                    foreach (var media in report.MediaPaths)
                    {
                        string base64String = Convert.ToBase64String(media);
                        TreeViewItem mediaItem = new TreeViewItem
                        {
                            Header = base64String,
                            IsExpanded = false
                        };
                        mediaPathsItem.Items.Add(mediaItem);
                    }
                }
                else
                {
                    TreeViewItem noMediaItem = new TreeViewItem
                    {
                        Header = "No Media Available",
                        IsExpanded = false
                    };
                    mediaPathsItem.Items.Add(noMediaItem);
                }
                reportItem.Items.Add(mediaPathsItem);

                // Status
                TreeViewItem statusItem = new TreeViewItem
                {
                    Header = $"Status: {report.Status}",
                    IsExpanded = false
                };
                reportItem.Items.Add(statusItem);

                // Category
                TreeViewItem categoryItem = new TreeViewItem
                {
                    Header = $"Category: {report.Category}",
                    IsExpanded = false
                };
                reportItem.Items.Add(categoryItem);

                // Subscribed Users
                TreeViewItem subscribedUsersItem = new TreeViewItem
                {
                    Header = "Subscribed Users:",
                    IsExpanded = false
                };

                if (report.SubscribedUsers != null && report.SubscribedUsers.Any())
                {
                    foreach (var userId in report.SubscribedUsers)
                    {
                        TreeViewItem userItem = new TreeViewItem
                        {
                            Header = userId.ToString(),
                            IsExpanded = false
                        };
                        subscribedUsersItem.Items.Add(userItem);
                    }
                }
                else
                {
                    TreeViewItem noUsersItem = new TreeViewItem
                    {
                        Header = "No Subscribers",
                        IsExpanded = false
                    };
                    subscribedUsersItem.Items.Add(noUsersItem);
                }
                reportItem.Items.Add(subscribedUsersItem);

                IssueReportsTreeView.Items.Add(reportItem);
            }
        }

        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reload the TreeView when the IssueReports collection changes
            LoadIssueReports();
        }
    }
}
