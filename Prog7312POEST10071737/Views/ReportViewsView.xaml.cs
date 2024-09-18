using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Prog7312POEST10071737.Models;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for ReportViewsView.xaml
    /// </summary>
    public partial class ReportViewsView : UserControl
    {

        public ObservableCollection<IssueReport> IssueReports;
        public ReportViewsView()
        {
            InitializeComponent();
            var singletonService = Services.UserSingleton.Instance;

            // Directly use the IssueReports ObservableCollection
            IssueReports = singletonService.IssueReports;
            ReportItemsControl.ItemsSource = IssueReports;

            // For debugging
            foreach (var report in singletonService.IssueReports)
            {
                System.Diagnostics.Debug.WriteLine($"Report: {report.Location} - {report.Description}");
            }
        }

        private void MakeReportButtonClicked(object sender, RoutedEventArgs e)
        {
            
            if (ContentControlReport.Content == null)
            {
                ContentControlReport.Content = new ReportFormView();
                GridForm.Opacity = 1;
                GridReports.Opacity = 0;
                ReportButtonTB.Text = "Back";
            }
            else
            {
                ContentControlReport.Content = null;
                GridForm.Opacity = 0;
                GridReports.Opacity = 1;
                ReportButtonTB.Text = "See an Issue Report it";
            }
        }
    }
}

