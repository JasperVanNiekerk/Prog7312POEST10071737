using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Represents the AVLandRedBlackTree user control.
    /// </summary>
    public partial class AVLandRedBlackTree : UserControl
    {
        /// <summary>
        /// The instance of the UserSingleton class.
        /// </summary>
        private readonly UserSingleton _userSingleton;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The AVL tree used for storing issue reports.
        /// </summary>
        private AVLTree<string, IssueReport> _avlTree;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The Red-Black tree used for storing issue reports.
        /// </summary>
        private RedBlackTree<string, IssueReport> _redBlackTree;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Indicates whether the user control has been initialized.
        /// </summary>
        private bool _isInitialized = false;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="AVLandRedBlackTree"/> class.
        /// </summary>
        public AVLandRedBlackTree()
        {
            InitializeComponent();

            // Initialize core components
            _userSingleton = UserSingleton.Instance;
            _avlTree = new AVLTree<string, IssueReport>();
            _redBlackTree = new RedBlackTree<string, IssueReport>();

            // Setup initial UI state after loading
            this.Loaded += AVLandRedBlackTree_Loaded;
            this.Unloaded += UserControl_Unloaded;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Loaded event of the AVLandRedBlackTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void AVLandRedBlackTree_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                // Setup category combobox
                var categories = new List<Category> { new Category(0, "All Categories") };
                categories.AddRange(Category.GetAllCategories());
                CategoryCB.ItemsSource = categories;
                CategoryCB.SelectedIndex = 0; // Select "All Categories" by default

                // Set default selection for radio buttons
                AVLTreeRB.IsChecked = true;

                // Initial load of reports
                LoadReports();

                _isInitialized = true;
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the TreeType_Changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void TreeType_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                LoadReports();
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the CategoryCB_SelectionChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized)
            {
                // Clear and rebuild the tree with the new category
                ReportsTreeView.Items.Clear();
                LoadReports();
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Loads the issue reports.
        /// </summary>
        private void LoadReports()
        {
            try
            {
                if (ReportsTreeView == null)
                {
                    return;
                }

                // Clear existing items
                ReportsTreeView.Items.Clear();

                var selectedCategory = CategoryCB.SelectedItem as Category;
                var reports = _userSingleton?.IssueReports;

                if (reports == null || reports.Count == 0)
                {
                    var noReportsItem = new TreeViewItem
                    {
                        Header = "No reports available",
                        IsExpanded = true
                    };
                    ReportsTreeView.Items.Add(noReportsItem);
                    return;
                }

                // Clear and reinitialize trees
                _avlTree = new AVLTree<string, IssueReport>();
                _redBlackTree = new RedBlackTree<string, IssueReport>();

                // Subscribe to collection changes
                if (_userSingleton != null)
                {
                    _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;
                }

                // Filter and load reports into appropriate tree
                foreach (var report in reports)
                {
                    // Include all reports if "All Categories" is selected or if the categories match
                    if (selectedCategory == null ||
                        selectedCategory.CategoryName == "All Categories" ||
                        report.Category == selectedCategory.ToString())
                    {
                        if (AVLTreeRB.IsChecked == true)
                        {
                            _avlTree.Insert(report.Category, report);
                        }
                        else
                        {
                            _redBlackTree.Insert(report.Category, report);
                        }
                    }
                }

                // Display reports in TreeView
                DisplayReports();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the issue reports in the TreeView.
        /// </summary>
        private void DisplayReports()
        {
            try
            {
                var selectedCategory = CategoryCB.SelectedItem as Category;
                List<IssueReport> reports = new List<IssueReport>();

                // If "All Categories" is selected, get all reports from the tree
                if (selectedCategory == null || selectedCategory.CategoryName == "All Categories")
                {
                    if (AVLTreeRB.IsChecked == true)
                    {
                        // Get all reports from AVL tree
                        foreach (var category in Category.GetAllCategories())
                        {
                            var categoryReports = _avlTree.Search(category.ToString());
                            if (categoryReports != null)
                            {
                                reports.AddRange(categoryReports);
                            }
                        }
                    }
                    else
                    {
                        // Get all reports from Red-Black tree
                        foreach (var category in Category.GetAllCategories())
                        {
                            var categoryReports = _redBlackTree.Search(category.ToString());
                            if (categoryReports != null)
                            {
                                reports.AddRange(categoryReports);
                            }
                        }
                    }
                }
                else
                {
                    // Get reports for specific category
                    if (AVLTreeRB.IsChecked == true)
                    {
                        reports = _avlTree.Search(selectedCategory.ToString()) ?? new List<IssueReport>();
                    }
                    else
                    {
                        reports = _redBlackTree.Search(selectedCategory.ToString()) ?? new List<IssueReport>();
                    }
                }

                if (reports.Count == 0)
                {
                    var noReportsItem = new TreeViewItem
                    {
                        Header = "No reports found for selected category",
                        IsExpanded = true
                    };
                    ReportsTreeView.Items.Add(noReportsItem);
                    return;
                }

                // Group reports by category when showing all categories
                if (selectedCategory == null || selectedCategory.CategoryName == "All Categories")
                {
                    var groupedReports = reports.GroupBy(r => r.Category);
                    foreach (var group in groupedReports)
                    {
                        var categoryItem = new TreeViewItem
                        {
                            Header = $"Category: {group.Key}",
                            IsExpanded = true
                        };

                        foreach (var report in group)
                        {
                            categoryItem.Items.Add(CreateReportTreeViewItem(report));
                        }

                        ReportsTreeView.Items.Add(categoryItem);
                    }
                }
                else
                {
                    // Display reports directly when a specific category is selected
                    foreach (var report in reports)
                    {
                        ReportsTreeView.Items.Add(CreateReportTreeViewItem(report));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Creates a TreeViewItem for an issue report.
        /// </summary>
        /// <param name="report">The issue report.</param>
        /// <returns>The TreeViewItem representing the issue report.</returns>
        private TreeViewItem CreateReportTreeViewItem(IssueReport report)
        {
            var reportItem = new TreeViewItem
            {
                Header = $"{report.name}",
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
        /// Handles the CollectionChanged event of the IssueReports.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Ensure UI updates happen on the UI thread
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => IssueReports_CollectionChanged(sender, e));
                return;
            }

            LoadReports(); // Reload the entire tree when the collection changes
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Unloaded event of the UserControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_userSingleton != null)
            {
                _userSingleton.IssueReports.CollectionChanged -= IssueReports_CollectionChanged;
            }
        }
    }
}
//____________________________________EOF_________________________________________________________________________