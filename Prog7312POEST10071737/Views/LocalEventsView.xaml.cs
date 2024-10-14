using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Diagnostics;

namespace Prog7312POEST10071737.Views
{
    public partial class LocalEventsView : UserControl
    {
        private readonly LocalEventsSingleton _eventsSingleton;
        private DispatcherTimer _timer;
        private int _currentEventIndex = 0;
        private List<OurEvents> _events;
        private bool _timerStarted = false;

        public LocalEventsView()
        {
            InitializeComponent();
            _eventsSingleton = LocalEventsSingleton.Instance;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += Timer_Tick;

            BannerButton.Content = "Loading events...";
            LoadEvents();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentEvent();
        }

        private void UpdateEventsList()
        {
            _events = new List<OurEvents>();

            Debug.WriteLine($"AnnouncementEventsQueue Count: {_eventsSingleton.AnnouncementEventsQueue.Count}");

            while (_eventsSingleton.AnnouncementEventsQueue.Count > 0)
            {
                var ev = _eventsSingleton.AnnouncementEventsQueue.Dequeue();
                _events.Add(ev);
            }

            foreach (var ev in _events)
            {
                _eventsSingleton.AnnouncementEventsQueue.Enqueue(ev, GetEventPriority(ev));
            }

            Debug.WriteLine($"_events Count after update: {_events.Count}");
        }

        private int GetEventPriority(OurEvents ev)
        {
            if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                return 1;
            else if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                return 2;
            else
                return 3;
        }

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
            _currentEventIndex++;
        }

        private string GetEventTimeFrame(OurEvents ev)
        {
            if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                return "happening now";
            else if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                return "starting this week";
            else
                return "starting this month";
        }

        private void BannerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid eventId)
            {
                MessageBox.Show($"Event clicked: {eventId}");
            }
        }

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

        private async void LoadEvents()
        {
            var eventService = LocalEventsSingleton.Instance;
            await eventService.PopulateEventsAsync(AddEventToUI);
        }

        private void AddEventToUI(OurEvents newEvent)
        {
            Dispatcher.Invoke(() =>
            {
                Debug.WriteLine($"New event added: {newEvent.Title}");
                _eventsSingleton.AnnouncementEventsQueue.Enqueue(newEvent, GetEventPriority(newEvent));
                RefreshEvents();

                Debug.WriteLine($"Total events after add: {_events.Count}");
            });
        }
    }
}