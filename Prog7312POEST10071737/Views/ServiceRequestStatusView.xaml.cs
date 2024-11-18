using System.Windows;
using System.Windows.Controls;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for ServiceRequestStatusView.xaml
    /// </summary>
    public partial class ServiceRequestStatusView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestStatusView"/> class.
        /// </summary>
        public ServiceRequestStatusView()
        {
            InitializeComponent();
            TreeCC.Content = new TreeDataStructureViews.BasicTree();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Click event of the AllServiceRequestsBTN control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AllServiceRequestsBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.BasicTree();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Click event of the FilterBTN control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FilterBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.AVLandRedBlackTree();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Click event of the AlphaBeticalBTN control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AlphaBeticalBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.BinaryTree();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Click event of the PopularityBTN control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PopularityBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.Heap();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Click event of the GraphBTN control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GraphBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.Graph();
        }
    }
}
//____________________________________EOF_________________________________________________________________________