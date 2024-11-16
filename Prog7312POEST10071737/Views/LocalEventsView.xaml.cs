using Prog7312POEST10071737.Components;
using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<OurEvents> _events;
        private bool _timerStarted = false;
        public ObservableCollection<Guid> EventIDs = new ObservableCollection<Guid>();
        private HashSet<Guid> _addedEventIds = new HashSet<Guid>();
        private bool checkIfPreviousClicked = false;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the LocalEventsView class.
        /// Sets up the timer, initializes components, and loads events.
        /// </summary>
        public LocalEventsView()
        {
            InitializeComponent();
            _eventsSingleton = LocalEventsSingleton.Instance;
            _events = new ObservableCollection<OurEvents>();

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

            // Create a temporary list to sort events before queuing
            var eventsToQueue = new List<(OurEvents Event, int Priority)>();

            // Collect all events with their priorities
            while (_eventsSingleton.AnnouncementEventsQueue.Count() > 0)
            {
                var ev = _eventsSingleton.AnnouncementEventsQueue.Dequeue();
                if (_addedEventIds.Add(ev.Id))
                {
                    _events.Add(ev);
                    eventsToQueue.Add((ev, CalculateEventPriority(ev)));
                }
            }

            // Sort events by priority and recency
            eventsToQueue = eventsToQueue
                .OrderBy(e => e.Priority)
                .ThenBy(e => Math.Abs((e.Event.StartDate - DateTime.Now).TotalDays))
                .ToList();

            // Re-queue events with their calculated priorities
            foreach (var (ev, priority) in eventsToQueue)
            {
                _eventsSingleton.AnnouncementEventsQueue.Enqueue(ev, priority);
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Calculates a more detailed priority score for an event based on multiple factors.
        /// Lower numbers indicate higher priority.
        /// </summary>
        private int CalculateEventPriority(OurEvents ev)
        {
            var now = DateTime.Now;
            var daysUntilStart = (ev.StartDate - now).TotalDays;
            var daysUntilEnd = (ev.EndDate - now).TotalDays;

            // Currently running events get highest priority (1)
            if (ev.StartDate <= now && ev.EndDate >= now)
                return 1;

            // Events starting within 24 hours get priority 2
            if (daysUntilStart >= 0 && daysUntilStart <= 1)
                return 2;

            // Events starting within the week get priority 3
            if (daysUntilStart > 1 && daysUntilStart <= 7)
                return 3;

            // Events starting within two weeks get priority 4
            if (daysUntilStart > 7 && daysUntilStart <= 14)
                return 4;

            // Events starting within the month get priority 5
            if (daysUntilStart > 14 && daysUntilStart <= 30)
                return 5;

            // All other future events get priority 6
            if (daysUntilStart > 30)
                return 6;

            // Past events get lowest priority (7)
            return 7;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the display with information about the current event.
        /// </summary>
        private void UpdateCurrentEvent()
        {
            if (_eventsSingleton.AnnouncementEventsQueue.Count() > 0)
            {
                var currentEvent = _eventsSingleton.AnnouncementEventsQueue.Dequeue();
                BannerButton.Content = _eventsSingleton.FormatEventBannerText(currentEvent);

                // Set the banner's background to the first image of the current event
                if (currentEvent.Images != null && currentEvent.Images.Count > 0)
                {
                    var firstImage = currentEvent.Images[0];
                    var imageBrush = new ImageBrush(new BitmapImage(new Uri(firstImage)))
                    {
                        Stretch = Stretch.UniformToFill
                    };
                    BannerButton.Background = imageBrush;
                }
                // Re-queue the event at the end to create a circular display
                _eventsSingleton.AnnouncementEventsQueue.Enqueue(currentEvent,
                    _eventsSingleton.CalculateEventPriority(currentEvent));
            }
            else
            {
                BannerButton.Content = "No upcoming events available";
            }
        }

        // Update the banner style based on event priority
        private void UpdateBannerStyle(OurEvents currentEvent)
        {
            var priority = _eventsSingleton.CalculateEventPriority(currentEvent);

            if (priority <= LocalEventsSingleton.PRIORITY_THRESHOLD)
            {
                BannerButton.FontWeight = FontWeights.Bold;
                BannerButton.Background = new SolidColorBrush(Colors.LightYellow);
            }
            else
            {
                BannerButton.FontWeight = FontWeights.Normal;
                BannerButton.Background = new SolidColorBrush(Colors.Transparent);
            }
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
        private void LoadEvents()
        {
            var eventService = LocalEventsSingleton.Instance;
            BannerButton.Content = "Loading events...";

            // Initialize UI components
            EventCardLoader();

            // Ensure banner is visible
            BannerButton.Visibility = Visibility.Visible;

            // Start loading events with the callback
            eventService.PopulateEventsAsync(AddEventToUI).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_events.Count == 0)
                    {
                        BannerButton.Content = "No events currently available";
                        return;
                    }

                    // Populate queue and start updates
                    eventService.PopulateAnnouncementQueue();
                    StartBannerUpdates();
                });
            });
        }

        private void StartBannerUpdates()
        {
            if (!_timerStarted && _eventsSingleton.AnnouncementEventsQueue.Count() > 0)
            {
                _timerStarted = true;
                UpdateCurrentEvent();
                _timer.Start();
            }
            else if (_eventsSingleton.AnnouncementEventsQueue.Count() == 0)
            {
                BannerButton.Content = "Stay tuned for upcoming events!";
            }
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
                    // Add to events collection first
                    _events.Add(newEvent);

                    // Add to announcement queue
                    _eventsSingleton.AnnouncementEventsQueue.Enqueue(newEvent, CalculateEventPriority(newEvent));

                    // Update EventIDs collection for display
                    if (!EventIDs.Contains(newEvent.Id))
                    {
                        EventIDs.Add(newEvent.Id);
                    }

                    // Update categories dropdown if needed
                    if (newEvent.Categories != null)
                    {
                        foreach (var category in newEvent.Categories)
                        {
                            if (!CategoryCB.Items.Contains(category))
                            {
                                CategoryCB.Items.Add(category);
                            }
                        }
                    }

                    // Start banner updates only if we have events and timer isn't started
                    if (!_timerStarted && _events.Count > 0)
                    {
                        _timerStarted = true;
                        UpdateCurrentEvent(); // Update banner immediately
                        _timer.Start();
                        BannerButton.Visibility = Visibility.Visible;
                    }

                    // Update loading message
                    if (WarringTB.Opacity > 0)
                    {
                        WarringTB.Opacity = 0;
                        EventCardsSV.Opacity = 1;
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

            if (_eventsSingleton.Events != null)
            {
                foreach (var ev in _eventsSingleton.Events)
                {
                    if (_addedEventIds.Add(ev.Id))
                    {
                        EventIDs.Add(ev.Id);
                    }
                }
            }
            else
            {
                // Handle the case where Events is null
                WarringTB.Opacity = 1;
                return;
            }

            EventItemsControl.ItemsSource = EventIDs;
            CategoryCB.ItemsSource = _eventsSingleton.UniqueCategories;
            StartDateDP.SelectedDate = DateTime.Now;
            EndDateDP.SelectedDate = DateTime.Now.AddDays(1);

            var filter = new FilterModel
            {
                FilterCategory = CategoryCB.SelectedItem as string,
                StartDate = StartDateDP.SelectedDate.Value,
                EndDate = EndDateDP.SelectedDate.Value,
                FilterEvents = _events
            };
            _eventsSingleton.AddFilter(filter);
            EventCardsSV.Opacity = 1;
            WarringTB.Opacity = 0;
        }
        //___________________________________________________________________________________________________________

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void searchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
                e.Handled = true;
            }
        }

        private void PerformSearch()
        {
            string searchTerm = searchTB.Text?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Reset to show all events
                EventIDs.Clear();
                foreach (var ev in _eventsSingleton.Events)
                {
                    EventIDs.Add(ev.Id);
                }
                WarringTB.Opacity = 0;
                EventCardsSV.Opacity = 1;
                return;
            }

            var searchResults = _eventsSingleton.Events
                .Where(ev => ev.SearchEvent(searchTerm))
                .ToList();

            EventIDs.Clear();
            foreach (var ev in searchResults)
            {
                EventIDs.Add(ev.Id);
            }

            // Update UI visibility based on results
            if (EventIDs.Count == 0)
            {
                WarringTB.Text = "No events found matching your search";
                WarringTB.Opacity = 1;
                EventCardsSV.Opacity = 0;
            }
            else
            {
                WarringTB.Opacity = 0;
                EventCardsSV.Opacity = 1;
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
            var selectedCategory = CategoryCB.SelectedItem as string;
            var startDate = StartDateDP.SelectedDate ?? DateTime.Now;
            var endDate = EndDateDP.SelectedDate ?? startDate.AddDays(1);

            var filteredEvents = _eventsSingleton.GetEventsByDateRange(selectedCategory, startDate, endDate);

            EventIDs.Clear();
            foreach (var ev in filteredEvents)
            {
                EventIDs.Add(ev.Id);
            }

            if (EventIDs.Count == 0)
            {
                WarringTB.Text = "No events found in the selected date range";
                WarringTB.Opacity = 1;
                EventCardsSV.Opacity = 0;
            }
            else
            {
                WarringTB.Opacity = 0;
                EventCardsSV.Opacity = 1;
            }

            var currentFilter = new FilterModel
            {
                FilterCategory = selectedCategory,
                StartDate = startDate,
                EndDate = endDate,
                FilterEvents = new ObservableCollection<OurEvents>(filteredEvents)
            };

            // Only add to stack if different from current top
            if (!_eventsSingleton.Filters.Any() || !currentFilter.Equals(_eventsSingleton.Filters.Peek()))
            {
                _eventsSingleton.AddFilter(currentFilter);
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event for the previous button.
        /// </summary>
        private void PreviousBTN_Click(object sender, RoutedEventArgs e)
        {
            EventIDs.Clear();
            var PrevFilter = _eventsSingleton.RemoveFilter();

            // Check if the button was clicked before
            if (!checkIfPreviousClicked)
            {
                // Go back two filters if clicked before
                var SecondPrevFilter = _eventsSingleton.RemoveFilter();
                foreach (var ev in SecondPrevFilter.FilterEvents)
                {
                    EventIDs.Add(ev.Id);
                }
                checkIfPreviousClicked = true; // Set to true after first click
            }
            else
            {
                // Go back one filter if first click
                foreach (var ev in PrevFilter.FilterEvents)
                {
                    EventIDs.Add(ev.Id);
                }
            }

            CategoryCB.ItemsSource = PrevFilter.FilterCategory;
            StartDateDP.SelectedDate = PrevFilter.StartDate;
            EndDateDP.SelectedDate = PrevFilter.EndDate;

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

        public void RefreshView()
        {
            _timerStarted = false;
            _timer.Stop();

            // Clear existing data
            _events.Clear();
            _addedEventIds.Clear();
            EventIDs.Clear();

            // Repopulate queue
            _eventsSingleton.PopulateAnnouncementQueue();

            // Restart banner updates
            LoadEvents();
        }

        private void LoadCategories()
        {
            var categories = _eventsSingleton.UniqueCategories;
            CategoryCB.ItemsSource = categories;

            // Preserve selected category if it exists
            if (CategoryCB.SelectedItem != null)
            {
                var currentCategory = CategoryCB.SelectedItem.ToString();
                if (categories.Contains(currentCategory))
                {
                    CategoryCB.SelectedItem = currentCategory;
                }
            }
        }

        private void DateRange_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (StartDateDP.SelectedDate.HasValue && EndDateDP.SelectedDate.HasValue)
            {
                if (EndDateDP.SelectedDate < StartDateDP.SelectedDate)
                {
                    EndDateDP.SelectedDate = StartDateDP.SelectedDate.Value.AddDays(1);
                }
            }
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