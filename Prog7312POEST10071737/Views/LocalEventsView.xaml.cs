using Prog7312POEST10071737.Components;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Prog7312POEST10071737.Views
{
    public partial class LocalEventsView : UserControl
    {
        private readonly LocalEventsSingleton _eventsSingleton;
        private DispatcherTimer _timer;
        private int _currentEventIndex = 0;
        private List<OurEvents> _events;
        private bool _timerStarted = false;
        public ObservableCollection<Guid> EventIDs = new ObservableCollection<Guid>();
        private HashSet<Guid> _addedEventIds = new HashSet<Guid>();
//___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the LocalEventsView class.
        /// Sets up the timer, initializes components, and loads events.
        /// </summary>
        public LocalEventsView()
        {
            InitializeComponent();
            _eventsSingleton = LocalEventsSingleton.Instance;
            _events = new List<OurEvents>();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += Timer_Tick;

            BannerButton.Content = "Loading events...";
            LoadEvents();
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the timer tick event. Updates the current event displayed.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentEvent();
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the list of events, clearing existing events and adding new ones from the queue.
        /// </summary>
        private void UpdateEventsList()
        {
            _events.Clear();
            _addedEventIds.Clear();

            while (_eventsSingleton.AnnouncementEventsQueue.Count() > 0)
            {
                var ev = _eventsSingleton.AnnouncementEventsQueue.Dequeue();
                if (_addedEventIds.Add(ev.Id))
                {
                    _events.Add(ev);
                }
            }

            foreach (var ev in _events)
            {
                _eventsSingleton.AnnouncementEventsQueue.Enqueue(ev, GetEventPriority(ev));
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Determines the priority of an event based on its start and end dates.
        /// </summary>
        /// <param name="ev">The event to prioritize.</param>
        /// <returns>An integer representing the event's priority.</returns>
        private int GetEventPriority(OurEvents ev)
        {
            if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                return 1;
            else if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                return 2;
            else
                return 3;
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the display with information about the current event.
        /// </summary>
        private void UpdateCurrentEvent()
        {
            if (_events.Count == 0)
            {
                BannerButton.Content = "No events currently.";
                return;
            }

            if (_currentEventIndex >= _events.Count)
            {
                _currentEventIndex = 0;
            }

            var currentEvent = _events[_currentEventIndex];
            string timeFrame = GetEventTimeFrame(currentEvent);

            BannerButton.Content = $"{currentEvent.Title} is {timeFrame} - Click for details";
            BannerButton.Tag = currentEvent.Id;
            if (currentEvent.Images.Count > 0)
            {
                var imageBrush = new System.Windows.Media.ImageBrush
                {
                    ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(currentEvent.Images[0])),
                    Stretch = System.Windows.Media.Stretch.UniformToFill
                };

                BannerBorder.Background = imageBrush;

                var bitmapImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(currentEvent.Images[0]));
                var averageBrightness = CalculateAverageBrightness(bitmapImage);

                if (averageBrightness < 128)
                {
                    BannerButton.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    BannerButton.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            _currentEventIndex++;
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Determines the time frame of an event relative to the current date.
        /// </summary>
        /// <param name="ev">The event to check.</param>
        /// <returns>A string describing the event's time frame.</returns>
        private string GetEventTimeFrame(OurEvents ev)
        {
            if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                return "happening now";
            else if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                return "starting this week";
            else
                return "starting this month";
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event for the banner button.
        /// </summary>
        private void BannerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid eventId)
            {
                MessageBox.Show($"Event clicked: {eventId}");
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Refreshes the events list and starts the timer if not already started.
        /// </summary>
        public void RefreshEvents()
        {
            UpdateEventsList();
            if (!_timerStarted)
            {
                _timerStarted = true;
                UpdateCurrentEvent();
                _timer.Start();
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Loads events asynchronously and initializes the event card loader.
        /// </summary>
        private async void LoadEvents()
        {
            var eventService = LocalEventsSingleton.Instance;
            await eventService.PopulateEventsAsync(AddEventToUI);
            EventCardLoader();
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Adds a new event to the UI asynchronously.
        /// </summary>
        /// <param name="newEvent">The new event to add.</param>
        private async void AddEventToUI(OurEvents newEvent)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (_addedEventIds.Add(newEvent.Id))
                {
                    _eventsSingleton.AnnouncementEventsQueue.Enqueue(newEvent, GetEventPriority(newEvent));
                    RefreshEvents();
                    if (!EventIDs.Contains(newEvent.Id))
                    {
                        EventIDs.Add(newEvent.Id);
                    }
                }
            });
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Loads event cards and initializes UI components for event display.
        /// </summary>
        public void EventCardLoader()
        {
            EventIDs.Clear();
            _addedEventIds.Clear();

            foreach (var ev in _eventsSingleton.Events)
            {
                if (_addedEventIds.Add(ev.Id))
                {
                    EventIDs.Add(ev.Id);
                }
            }
            EventItemsControl.ItemsSource = EventIDs;
            CategoryCB.ItemsSource = _eventsSingleton.UniqueCategories;
            DateDP.SelectedDate = DateTime.Now;

            var filter = new FilterModel
            {
                FilterCatagory = CategoryCB.SelectedItem as string,
                FilterDate = DateDP.SelectedDate.Value,
                FilterEvents = _events
            };
            _eventsSingleton.AddFilter(filter);

            EventCardsSV.Opacity = 1;
            WarringTB.Opacity = 0;
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the key down event for the search text box.
        /// </summary>
        private void searchTB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string userInput = searchTB.Text;

                var temp = _eventsSingleton.GetSearchedEvents(userInput);
                EventIDs.Clear();
                foreach (var ev in temp)
                {
                    EventIDs.Add(ev.Id);
                }
                if (EventIDs.Count == 0)
                {
                    WarringTB.Text = "No events found";
                    WarringTB.Opacity = 1;
                    EventCardsSV.Opacity = 0;
                }

                e.Handled = true;
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the got focus event for the search text box.
        /// </summary>
        private void searchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text == "Search")
            {
                textBox.Text = string.Empty;
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the lost focus event for the search text box.
        /// </summary>
        private void searchTB_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search";
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event for the filter button.
        /// </summary>
        private void FilterBTN_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCategory = CategoryCB.SelectedItem as string;
            var SelectedDate = DateDP.SelectedDate;

            var CF = _eventsSingleton.GetEventsByCategory(SelectedCategory);
            var DF = new List<OurEvents>();
            foreach (var ev in CF)
            {
                if (ev.StartDate <= SelectedDate.Value && ev.EndDate >= SelectedDate.Value)
                {
                    DF.Add(ev);
                }
            }
            EventIDs.Clear();
            foreach (var ev in DF)
            {
                EventIDs.Add(ev.Id);
            }

            if (EventIDs.Count == 0)
            {
                WarringTB.Text = "No events found";
                WarringTB.Opacity = 1;
                EventCardsSV.Opacity = 0;
            }

            var filter = new FilterModel
            {
                FilterCatagory = SelectedCategory,
                FilterDate = SelectedDate.Value,
                FilterEvents = DF
            };
            _eventsSingleton.AddFilter(filter);
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event for the previous button.
        /// </summary>
        private void PreviousBTN_Click(object sender, RoutedEventArgs e)
        {
            EventIDs.Clear();
            var PrevFilter = _eventsSingleton.RemoveFilter();
            foreach (var ev in PrevFilter.FilterEvents)
            {
                EventIDs.Add(ev.Id);
            }
            CategoryCB.ItemsSource = PrevFilter.FilterCatagory;
            DateDP.SelectedDate = PrevFilter.FilterDate;

            if (WarringTB.Opacity == 1)
            {
                WarringTB.Opacity = 0;
                EventCardsSV.Opacity = 1;
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Calculates the average brightness of a bitmap image.
        /// </summary>
        /// <param name="bitmap">The bitmap image to analyze.</param>
        /// <returns>The average brightness value.</returns>
        private double CalculateAverageBrightness(BitmapSource bitmap)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;

            byte[] pixelData = new byte[width * height * bytesPerPixel];
            bitmap.CopyPixels(pixelData, width * bytesPerPixel, 0);

            double totalBrightness = 0;
            int pixelCount = width * height;

            for (int i = 0; i < pixelCount; i++)
            {
                int r = pixelData[i * bytesPerPixel + 0];
                int g = pixelData[i * bytesPerPixel + 1];
                int b = pixelData[i * bytesPerPixel + 2];

                double brightness = (0.299 * r + 0.587 * g + 0.114 * b);
                totalBrightness += brightness;
            }

            return totalBrightness / pixelCount;
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Asynchronously loads events using the event service.
        /// </summary>
        private async Task LoadEventsAsync()
        {
            var eventService = LocalEventsSingleton.Instance;
            await eventService.PopulateEventsAsync(AddEventToUI);
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the mouse down event for the event display component.
        /// </summary>
        private void EventDisplayComponent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is EventDisplayComponent)
            {
                EventDisplayComponent EDC = (EventDisplayComponent)sender;
                var eventID = EDC.EventId;

                var eventView = new EventView();
                eventView.EventId = eventID;
                ContentControl.Content = eventView;
                WarringTB.Opacity = 0;
                EventCardsSV.Opacity = 0;
                EventCardsSV.IsHitTestVisible = false;
                CCGrid.Opacity = 1;
            }
        }
//___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event for a generic button.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WarringTB.Opacity = 0;
            EventCardsSV.Opacity = 1;
            EventCardsSV.IsHitTestVisible = true;
            CCGrid.Opacity = 0;
        }
    }
//___________________________________________________________________________________________________________

    /// <summary>
    /// Compares OurEvents objects for equality based on their Id.
    /// </summary>
    public class EventComparer : IEqualityComparer<OurEvents>
    {
        public bool Equals(OurEvents x, OurEvents y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(OurEvents obj)
        {
            return obj?.Id.GetHashCode() ?? 0;
        }
    }
}
//____________________________________EOF_________________________________________________________________________