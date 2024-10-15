using Prog7312POEST10071737.Components;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Represents the view for displaying event details.
    /// </summary>
    public partial class EventView : System.Windows.Controls.UserControl
    {

        private List<Guid> Sevents = new List<Guid>();
        private List<string> Images = new List<string>();
        private int currentImageIndex = 0;
        private int ImagesCount = 0;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Identifies the EventId dependency property.
        /// </summary>
        public static readonly DependencyProperty EventIdProperty =
            DependencyProperty.Register("EventId", typeof(Guid), typeof(EventView),
                new PropertyMetadata(Guid.Empty, OnEventIdChanged));
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        public Guid EventId
        {
            get { return (Guid)GetValue(EventIdProperty); }
            set { SetValue(EventIdProperty, value); }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the <see cref="EventView"/> class.
        /// </summary>
        public EventView()
        {
            InitializeComponent();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the event when the EventId property changes.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnEventIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EventView component)
            {
                component.UpdateEventDisplay();
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the event display based on the current EventId.
        /// </summary>
        private void UpdateEventDisplay()
        {
            if (EventId != Guid.Empty)
            {
                var eventService = LocalEventsSingleton.Instance;
                OurEvents eventDetails = eventService.GetEventById(EventId);
                TitelTB.Text = eventDetails.Title;
                DatesTB.Text = eventDetails.Dates;
                VenueTB.Text = eventDetails.Venue;
                PhoneTB.Text = eventDetails.Phone;
                EventImageIMG.Source = new BitmapImage(new Uri(eventDetails.Images[0]));
                DescriptionTB.Text = eventDetails.Description;
                Images = eventDetails.Images;
                var temp = eventService.GetThreeMostSimilarEvents(EventId);
                foreach (var item in temp)
                {
                    Sevents.Add(item.Id);
                }
                EventItemsControl.ItemsSource = Sevents;
                ImagesCount = Images.Count;
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the current image in the event display.
        /// </summary>
        private void DisplayImage()
        {
            if (ImagesCount > 0 && currentImageIndex < ImagesCount)
            {
                EventImageIMG.Source = new BitmapImage(new Uri(Images[currentImageIndex]));
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event of the left arrow button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void leftArrowBTN_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex = (currentImageIndex - 1 + ImagesCount) % ImagesCount;
            DisplayImage();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event of the right arrow button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RightArrowBTN_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % ImagesCount;
            DisplayImage();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the mouse down event of the event display component.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void EventDisplayComponent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is EventDisplayComponent)
            {
                EventDisplayComponent EDC = (EventDisplayComponent)sender;
                var eventID = EDC.EventId;
                EventId = eventID;
                UpdateEventDisplay();
            }
        }
    }
}
//____________________________________EOF_________________________________________________________________________