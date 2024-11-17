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

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for ServiceRequestStatusView.xaml
    /// </summary>
    public partial class ServiceRequestStatusView : UserControl
    {
        public ServiceRequestStatusView()
        {
            InitializeComponent();
            TreeCC.Content = new TreeDataStructureViews.BasicTree();
        }

        private void AllServiceRequestsBTN_Click(object sender, RoutedEventArgs e)
        {
            TreeCC.Content = new TreeDataStructureViews.BasicTree();
        }

        private void FilterBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AlphaBeticalBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GraphBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MSTBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
