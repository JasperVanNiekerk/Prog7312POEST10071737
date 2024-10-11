using Prog7312POEST10071737.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Prog7312POEST10071737.Components
{
    /// <summary>
    /// Interaction logic for ReportDisplayComponent.xaml
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
                ReportImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(GetFirstImagePath().ToString())));
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
                BitmapImage bitmap = GetFirstImagePath();
                if (bitmap != null)
                {
                    // Set the image source for the Image control
                    ReportImage.Background = new ImageBrush(bitmap);
                }
                else
                {
                    ReportImage.Background = null; // Clear image if no valid image data is found
                }
            }
        }
        private BitmapImage GetFirstImagePath()
        {
            var Media = IssueReport.MediaPaths;
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
                            imageLoaded = true; // Image successfully loaded, exit loop
                            return bitmap;
                        }
                        catch (NotSupportedException)
                        {
                            count++;
                        }
                    }
                }
            }
            return null; // Return null if no valid image data is found
        }

    }
}
