using System.Linq;
using System.Windows.Controls;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Interaction logic for Heap.xaml
    /// </summary>
    public partial class Heap : UserControl
    {
        private readonly UserSingleton _userSingleton;
        private readonly MaxHeap<IssueReport> _maxHeap;

        public Heap()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            _maxHeap = new MaxHeap<IssueReport>(report => report.SubscribedUsers?.Count ?? 0);

            // Subscribe to collection changes
            _userSingleton.IssueReports.CollectionChanged += IssueReports_CollectionChanged;

            LoadReports();
        }

        private void LoadReports()
        {
            // Clear existing items
            ReportsListView.Items.Clear();

            // Add all reports to the max heap
            foreach (var report in _userSingleton.IssueReports)
            {
                _maxHeap.Insert(report);
            }

            // Get sorted reports and create view models
            var sortedReports = _maxHeap.GetSortedItems();
            foreach (var report in sortedReports)
            {
                ReportsListView.Items.Add(new
                {
                    Name = report.name,
                    SubscriberCount = report.SubscribedUsers?.Count ?? 0
                });
            }
        }

        private void IssueReports_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadReports();
        }
    }
}
