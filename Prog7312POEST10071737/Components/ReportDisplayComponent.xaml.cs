using Prog7312POEST10071737.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Prog7312POEST10071737.Components
{
    /// <summary>
    /// Represents a user control for displaying a report.
    /// </summary>
    public partial class ReportDisplayComponent : UserControl
    {
        public static readonly DependencyProperty IssueReportProperty =
            DependencyProperty.Register("IssueReport", typeof(IssueReport), typeof(ReportDisplayComponent),
                new PropertyMetadata(null, OnIssueReportChanged));

        /// <summary>
        /// Gets or sets the issue report to be displayed.
        /// </summary>
        public IssueReport IssueReport
        {
            get { return (IssueReport)GetValue(IssueReportProperty); }
            set { SetValue(IssueReportProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDisplayComponent"/> class.
        /// </summary>
        public ReportDisplayComponent()
        {
            InitializeComponent();
            if (IssueReport != null)
            {
                ReportImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(GetFirstImagePath())));
            }

        }

        private static void OnIssueReportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ReportDisplayComponent control)
            {
                control.DataContext = e.NewValue;
                control.SetReportImage(); // Update the image when IssueReport changes
            }
        }

        private void SetReportImage()
        {
            if (IssueReport != null)
            {
                string imagePath = GetFirstImagePath();
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Create an ImageBrush
                    ImageBrush imageBrush = new ImageBrush();

                    // Set the image source
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(GetFirstImagePath(), UriKind.Absolute);
                    bitmap.EndInit();

                    imageBrush.ImageSource = bitmap;

                    ReportImage.Background = imageBrush;
                }
                else
                {
                    ReportImage.Background = null; ; // Set to null if no image is found
                }
            }
        }

        private string GetFirstImagePath()
        {
            var imageExtensions = new List<string> { ".png", ".jpeg", ".jpg" };
            var MediaPaths = IssueReport.MediaPaths;
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
    }
}