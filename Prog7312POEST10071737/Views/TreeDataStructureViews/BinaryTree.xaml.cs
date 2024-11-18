using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System.Windows.Controls;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Represents a binary tree view control.
    /// </summary>
    public partial class BinaryTree : UserControl
    {
        private readonly UserSingleton _userSingleton;
        private readonly BinarySearchTree _binaryTree;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTree"/> class.
        /// </summary>
        public BinaryTree()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            _binaryTree = new BinarySearchTree();

            // Subscribe to collection changes
            _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;

            LoadReports();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Loads the issue reports into the binary tree and displays them in the TreeView.
        /// </summary>
        private void LoadReports()
        {
            IssueReportsTreeView.Items.Clear();

            // Populate binary tree
            foreach (var report in _userSingleton.IssueReports)
            {
                _binaryTree.Insert(report);
            }

            // Display reports in TreeView in alphabetical order
            _binaryTree.InorderTraversal(report =>
            {
                var reportItem = CreateReportTreeViewItem(report);
                IssueReportsTreeView.Items.Add(reportItem);
            });
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Creates a TreeViewItem for the given issue report.
        /// </summary>
        /// <param name="report">The issue report.</param>
        /// <returns>The TreeViewItem representing the issue report.</returns>
        private TreeViewItem CreateReportTreeViewItem(IssueReport report)
        {
            var reportItem = new TreeViewItem
            {
                Header = report.name,
                IsExpanded = false
            };

            // Add ID
            reportItem.Items.Add(new TreeViewItem { Header = $"ID: {report.Id}" });

            // Add Description
            reportItem.Items.Add(new TreeViewItem { Header = $"Description: {report.Description}" });

            // Add Location
            reportItem.Items.Add(new TreeViewItem { Header = $"Location: {report.Location}" });

            // Add Media Paths
            var mediaItem = new TreeViewItem { Header = "Media Files:", IsExpanded = false };
            if (report.MediaPaths != null && report.MediaPaths.Count > 0)
            {
                foreach (var media in report.MediaPaths)
                {
                    mediaItem.Items.Add(new TreeViewItem { Header = media.FileName });
                }
            }
            else
            {
                mediaItem.Items.Add(new TreeViewItem { Header = "No media files" });
            }
            reportItem.Items.Add(mediaItem);

            // Add Status
            reportItem.Items.Add(new TreeViewItem { Header = $"Status: {report.Status}" });

            // Add Category
            reportItem.Items.Add(new TreeViewItem { Header = $"Category: {report.Category}" });

            // Add Subscribed Users
            var subscribedUsersItem = new TreeViewItem { Header = "Subscribed Users:", IsExpanded = false };
            if (report.SubscribedUsers != null && report.SubscribedUsers.Count > 0)
            {
                foreach (var userId in report.SubscribedUsers)
                {
                    subscribedUsersItem.Items.Add(new TreeViewItem { Header = userId });
                }
            }
            else
            {
                subscribedUsersItem.Items.Add(new TreeViewItem { Header = "No subscribed users" });
            }
            reportItem.Items.Add(subscribedUsersItem);

            return reportItem;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the collection changed event of the IssueReports collection.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadReports();
        }
    }
}
//____________________________________EOF_________________________________________________________________________