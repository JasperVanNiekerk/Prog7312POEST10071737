using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
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

namespace Prog7312POEST10071737.Components
{
    /// <summary>
    /// Interaction logic for EventDisplayComponent.xaml
    /// </summary>
    public partial class EventDisplayComponent : UserControl
    {
        /// <summary>
        /// Identifies the EventId dependency property.
        /// </summary>
        public static readonly DependencyProperty EventIdProperty =
            DependencyProperty.Register("EventId", typeof(Guid), typeof(EventDisplayComponent),
                new PropertyMetadata(Guid.Empty, OnEventIdChanged));
//___________________________________________________________________________________________________________

        /// <summary>
        /// Gets or sets the EventId.
        /// </summary>
        public Guid EventId
        {
            get { return (Guid)GetValue(EventIdProperty); }
            set { SetValue(EventIdProperty, value); }
        }
//___________________________________________________________________________________________________________

        public EventDisplayComponent()
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
            if (d is EventDisplayComponent component)
            {
                component.UpdateEventDisplay();
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the event display based on the EventId.
        /// </summary>
        private void UpdateEventDisplay()
        {
            if (EventId != Guid.Empty)
            {
                // Fetch event details using the EventId
                // This is a placeholder - replace with your actual event fetching logic
                var EventService = LocalEventsSingleton.Instance;
                OurEvents eventDetails = EventService.GetEventById(EventId);

                if (eventDetails != null)
                {
                    EventNameTB.Text = eventDetails.Title;
                    EventDateTB.Text = eventDetails.Dates;
                    DescriptionTB.Text = eventDetails.Description;
                    CatagoriesIC.ItemsSource = eventDetails.Categories;
                    SetImage(eventDetails.Images);
                }
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Sets the image for the event display.
        /// </summary>
        /// <param name="images">The list of image URLs.</param>
        private void SetImage(List<string> images)
        {
            if (images.Count > 0)
            {
                var imageBrush = new System.Windows.Media.ImageBrush
                {
                    ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(images[0])),
                    Stretch = System.Windows.Media.Stretch.UniformToFill
                };
                BackgroundBorder.Background = imageBrush;
            }
        }

        private void BackgroundBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
//____________________________________EOF_________________________________________________________________________