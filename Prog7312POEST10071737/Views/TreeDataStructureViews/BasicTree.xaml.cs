using Prog7312POEST10071737.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Interaction logic for BasicTree.xaml
    /// </summary>
    public partial class BasicTree : UserControl
    {
        private UserSingleton _userSingleton;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicTree"/> class.
        /// </summary>
        public BasicTree()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            LoadIssueReports();
            // Subscribe to collection changed because IssueReports can be modified at runtime
            _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Loads the issue reports into the TreeView.
        /// </summary>
        private void LoadIssueReports()
        {
            IssueReportsTreeView.Items.Clear();

            foreach (var report in _userSingleton.IssueReports)
            {
                // Create a Border to wrap the Issue Report Header
                Border reportHeaderBorder = new Border
                {
                    Background = Brushes.White,
                    Padding = new Thickness(5),
                    CornerRadius = new CornerRadius(5),
                    Child = new TextBlock
                    {
                        Text = $"{report.name}",
                        FontWeight = FontWeights.Bold
                    }
                };

                TreeViewItem reportItem = new TreeViewItem
                {
                    Header = reportHeaderBorder,
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

                // Media Files
                TreeViewItem mediaFilesItem = new TreeViewItem
                {
                    Header = "Uploaded Files:",
                    IsExpanded = false
                };

                if (report.MediaPaths != null && report.MediaPaths.Any())
                {
                    foreach (var file in report.MediaPaths)
                    {
                        TreeViewItem fileItem = new TreeViewItem
                        {
                            Header = file.FileName,
                            IsExpanded = false
                        };

                        fileItem.ToolTip = "Double click to open file.";

                        fileItem.MouseDoubleClick += (s, e) =>
                        {
                            try
                            {
                                file.OpenFile();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Unable to open file {file.FileName}. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        };

                        mediaFilesItem.Items.Add(fileItem);
                    }
                }
                else
                {
                    TreeViewItem noFilesItem = new TreeViewItem
                    {
                        Header = "No Files Uploaded",
                        IsExpanded = false
                    };
                    mediaFilesItem.Items.Add(noFilesItem);
                }
                reportItem.Items.Add(mediaFilesItem);

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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the collection changed event of the IssueReports collection.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reload the TreeView when the IssueReports collection changes
            LoadIssueReports();
        }
    }
}
//____________________________________EOF_________________________________________________________________________