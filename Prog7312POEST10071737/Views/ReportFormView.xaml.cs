﻿using System;
using System.Collections.Generic;
using System.IO;
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
        private List<byte[]> Media = new List<byte[]>();
        private List<string> MediaNames = new List<string>();
        private string location;
        private string category;
        private string description;
        private string FirstMedia;
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
                var media = Media;

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
                MediaNames.Add(GetFileName(file));
                Media.Add(FileToBitArray(file));
            }

            UpdateFirstImage();
            UpdateMediaList();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to convert file path to byte array
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] FileToBitArray(string filePath)
        {
            // Read all bytes from the file
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return fileBytes;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to get the file name from the file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to display the first image in the media paths list
        /// </summary>
        private void UpdateFirstImage()
        {
            if (Media.Count > 0)
            {
                var count = 0;
                bool imageLoaded = false;

                while (Media.Count > count && !imageLoaded)
                {
                    byte[] firstMediaArray = Media[count];

                    using (var ms = new MemoryStream(firstMediaArray))
                    {
                        try
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            // Force free resources after loading the image
                            bitmap.Freeze();

                            ImageDisplayIMG.Source = bitmap;
                            imageLoaded = true; // Image successfully loaded, exit loop
                        }
                        catch (NotSupportedException)
                        {
                            count++;
                            ImageDisplayIMG.Source = null;
                        }
                    }
                }

                // No valid image found
                if (!imageLoaded)
                {
                    ImageDisplayIMG.Source = null;
                }
            }
            else
            {
                ImageDisplayIMG.Source = null;
            }
        }
        //___________________________________________________________________________________________________________


        private void UpdateMediaList()
        {
            var mediaList = "";
            if (Media.Count > 0)
            {
                foreach (var media in MediaNames)
                {
                    mediaList += media + "\n";
                }
                MediaList.FontSize = 8;
            }
            else
            {
                mediaList = "No media added";
            }
            MediaList.Text = mediaList;
        }

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