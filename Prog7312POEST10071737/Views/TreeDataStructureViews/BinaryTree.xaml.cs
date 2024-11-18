using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Interaction logic for BinaryTree.xaml
    /// </summary>
    public partial class BinaryTree : UserControl
    {
        private readonly UserSingleton _userSingleton;
        private readonly BinarySearchTree _binaryTree;

        public BinaryTree()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            _binaryTree = new BinarySearchTree();
            
            // Subscribe to collection changes
            _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;
            
            LoadReports();
        }

        private void LoadReports()
        {
            IssueReportsTreeView.Items.Clear();

            // Populate binary tree
            foreach (var report in _userSingleton.IssueReports)
            {
                _binaryTree.Insert(report);
            }

            // Display reports in TreeView in alphabetical order
            _binaryTree.InorderTraversal(report => {
                var reportItem = CreateReportTreeViewItem(report);
                IssueReportsTreeView.Items.Add(reportItem);
            });
        }

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

        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadReports();
        }
    }
}
