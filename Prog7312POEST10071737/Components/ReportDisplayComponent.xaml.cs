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

        public ReportDisplayComponent()
        {
            InitializeComponent();
            if (IssueReport != null)
            {
                SetReportImage();
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
            if (IssueReport != null && IssueReport.MediaPaths != null && IssueReport.MediaPaths.Count > 0)
            {
                foreach (var uploadedFile in IssueReport.MediaPaths)
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        using (var ms = new MemoryStream(uploadedFile.FileData))
                        {
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();
                            ReportImage.Background = new ImageBrush(bitmap);
                            break; // Display only the first valid image
                        }
                    }
                    catch (NotSupportedException)
                    {
                        ReportImage.Background = null;
                    }
                }
            }
            else
            {
                ReportImage.Background = null;
            }
        }
    }
}
