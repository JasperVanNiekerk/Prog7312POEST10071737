using Prog7312POEST10071737.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Prog7312POEST10071737.Components;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for ReportViewsView.xaml
    /// </summary>
    public partial class ReportViewsView : UserControl
    {
        /// <summary>
        /// declaration of the IssueReports ObservableCollection
        /// </summary>
        public ObservableCollection<IssueReport> IssueReports;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Constructor for the ReportViewsView
        /// </summary>
        public ReportViewsView()
        {
            InitializeComponent();
            var singletonService = Services.UserSingleton.Instance;

            // Directly use the IssueReports ObservableCollection
            IssueReports = singletonService.IssueReports;
            ReportItemsControl.ItemsSource = IssueReports;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event of the ReportButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________