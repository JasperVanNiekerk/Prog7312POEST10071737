using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for ReportFormView.xaml
    /// </summary>
    public partial class ReportFormView : UserControl
    {
        /// <summary>
        /// declare variables
        /// </summary>
        private List<string> MediaPaths = new List<string>();
        private string location;
        private string category;
        private string description;
        private string FirstPhotoPath;
        private bool userPresent = false;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// ReportFormView constructor
        /// </summary>
        public ReportFormView()
        {
            InitializeComponent();
            CatagoryCB.ItemsSource = Models.Category.GetAllCategories();
            var SingletonService = Services.UserSingleton.Instance;
            userPresent = SingletonService.UserExists();
            if (userPresent == false)
            {
                SubscribeCB.IsEnabled = false;
                SubscribeBTN.IsEnabled = false;
                SubscribeCB.ToolTip = "You need to be logged in to subscribe to reports";
                SubscribeBTN.ToolTip = "You need to be logged in to subscribe to reports";
                ReportConfirmationCB.IsEnabled = false;
                ReportConfirmationBTN.IsEnabled = false;
                ReportConfirmationCB.ToolTip = "You need to be logged in to receive report confirmation";
                ReportConfirmationBTN.ToolTip = "You need to be logged in to receive report confirmation";
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(DescriptionTB.Document.ContentStart, DescriptionTB.Document.ContentEnd);
            description = textRange.Text.Trim();

            if (LocationTB.Text == "" || CatagoryCB.Text == "" || description == "")
            {
                MessageBox.Show("Please fill in all fields", "Error Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                location = LocationTB.Text;
                category = CatagoryCB.Text;
                var media = MediaPaths;

                var SingletonService = Services.UserSingleton.Instance;

                if (SubscribeCB.IsChecked == true)
                {
                    var user = SingletonService.GetUserGUID();
                    SingletonService.AddIssueReport(description, location, media, category, user);
                }
                else
                {
                    SingletonService.AddIssueReport(description, location, media, category);
                }

                ReportConfirmation();
                LocationTB.Text = "";
                CatagoryCB.Text = "";
                DescriptionTB.Document.Blocks.Clear();
                ImageDisplayIMG.Source = null;
                MessageBox.Show("Issue Submitted", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the add media button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMediaButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.ShowDialog();

            foreach (var file in dialog.FileNames)
            {
                MediaPaths.Add(file);
            }

            GetFirstImagePath();
            UpdateFirstImage();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to get the first image path in the media paths list
        /// </summary>
        /// <returns></returns>
        private string GetFirstImagePath()
        {
            var imageExtensions = new List<string> { ".png", ".jpeg", ".jpg" };

            foreach (var path in MediaPaths)
            {
                string extension = System.IO.Path.GetExtension(path).ToLower();
                if (imageExtensions.Contains(extension))
                {
                    return path;
                }
            }

            return null; // Return null if no image is found
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to display the first image in the media paths list
        /// </summary>
        private void UpdateFirstImage()
        {
            FirstPhotoPath = GetFirstImagePath();
            if (FirstPhotoPath != null)
            {
                ImageDisplayIMG.Source = new BitmapImage(new Uri(FirstPhotoPath));
            }
            else
            {
                ImageDisplayIMG.Source = null;
            }
        }
        //___________________________________________________________________________________________________________


        /// <summary>
        /// method to send a report confirmation email
        /// </summary>
        private void ReportConfirmation()
        {
            if (ReportConfirmationCB.IsChecked == true)
            {
                var EmailService = new Services.MyEmailService();
                var subject = "Report Confirmation";
                var body = "your report for " + location + " has been submitted.";
                EmailService.EmailSender(subject, body);
            }

        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the placeholder text of the rich textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderTB.Visibility = string.IsNullOrEmpty(new TextRange(DescriptionTB.Document.ContentStart, DescriptionTB.Document.ContentEnd).Text.Trim()) ? Visibility.Visible : Visibility.Collapsed;
        }
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________