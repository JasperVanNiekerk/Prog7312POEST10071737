using ModernWpf;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using Prog7312POEST10071737.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Windows.UI.Xaml.Controls;

namespace Prog7312POEST10071737
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// constructor for the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UpdateBackground();
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to update the background based on the current time of day
        /// </summary>
        private void UpdateBackground()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            Storyboard storyboard = new Storyboard();

            // Time ranges for blending (adjust these as necessary)
            var dayStart = TimeSpan.FromHours(6);
            var duskStart = TimeSpan.FromHours(18);
            var nightStart = TimeSpan.FromHours(20);

            DoubleAnimation dayAnimation = new DoubleAnimation();
            DoubleAnimation duskAnimation = new DoubleAnimation();
            DoubleAnimation nightAnimation = new DoubleAnimation();

            if (currentTime >= dayStart && currentTime < duskStart)
            {
                // Between day and dusk
                var fractionOfDay = (currentTime - dayStart).TotalHours / (duskStart - dayStart).TotalHours;

                dayAnimation.To = Math.Min(1, 1 - fractionOfDay);
                duskAnimation.To = Math.Min(1, fractionOfDay);
                nightAnimation.To = 0;
            }
            else if (currentTime >= duskStart && currentTime < nightStart)
            {
                // Between dusk and night
                var fractionOfDusk = (currentTime - duskStart).TotalHours / (nightStart - duskStart).TotalHours;

                dayAnimation.To = 0;
                duskAnimation.To = Math.Min(1, 1 - fractionOfDusk);
                nightAnimation.To = Math.Min(1, fractionOfDusk);
            }
            else
            {
                // Early morning or night
                if (currentTime < dayStart)
                {
                    // Night and early morning blend
                    var fractionOfNight = (currentTime.TotalHours / dayStart.TotalHours);

                    nightAnimation.To = Math.Min(1, 1 - fractionOfNight);
                    dayAnimation.To = Math.Min(1, fractionOfNight);
                }
                else
                {
                    // Full night
                    dayAnimation.To = 0;
                    duskAnimation.To = 0;
                    nightAnimation.To = 1;
                }
            }

            // Set durations (e.g., 2 seconds)
            dayAnimation.Duration = duskAnimation.Duration = nightAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            // Set target properties
            Storyboard.SetTarget(dayAnimation, DayImage);
            Storyboard.SetTargetProperty(dayAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(duskAnimation, DuskImage);
            Storyboard.SetTargetProperty(duskAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(nightAnimation, NightImage);
            Storyboard.SetTargetProperty(nightAnimation, new PropertyPath("Opacity"));

            // Add animations to the storyboard
            storyboard.Children.Add(dayAnimation);
            storyboard.Children.Add(duskAnimation);
            storyboard.Children.Add(nightAnimation);

            storyboard.Begin();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event for the first control radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new ReportViewsView();
            GridMain.Opacity = 0;
            GridMain.IsHitTestVisible = false;
            GridActivity.Opacity = 1;
            GridActivity.IsHitTestVisible = true;
            RB1.IsChecked = true;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event for the second control radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new LocalEventsView();
            GridMain.Opacity = 0;
            GridMain.IsHitTestVisible = false;
            GridActivity.Opacity = 1;
            GridActivity.IsHitTestVisible = true;
            RB2.IsChecked = true;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event for the third control radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control3RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = null;
            GridMain.Opacity = 0;
            GridMain.IsHitTestVisible = false;
            GridActivity.Opacity = 1;
            GridActivity.IsHitTestVisible = true;
            RB3.IsChecked = true;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to open the feedback form in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedbackButtonClicked(object sender, RoutedEventArgs e)
        {
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSeVlfuNeJuATkLLp7Uocl7shU4O2-Xb4yt2SLxTnoT4HNDg2A/viewform?usp=sf_link";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event for the back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = null;
            GridMain.Opacity = 1;
            GridMain.IsHitTestVisible = true;
            GridActivity.Opacity = 0;
            GridActivity.IsHitTestVisible = false;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to handle the click event for the exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //___________________________________________________________________________________________________________
    }
}
//i love you!
//i love you too!
//____________________________________EOF_________________________________________________________________________