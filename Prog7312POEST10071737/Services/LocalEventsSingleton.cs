using Prog7312POEST10071737.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog7312POEST10071737.Services
{
    public class LocalEventsSingleton
    {
        private static LocalEventsSingleton instance = null;
        private static readonly object padlock = new object();

        private LocalEventsSingleton()
        {
        }

        public static LocalEventsSingleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LocalEventsSingleton();
                    }
                    return instance;
                }
            }
        }

        public List<OurEvents> Events { get; set; } = new List<OurEvents>();
        public MyPriorityQueue<OurEvents> AnnouncementEventsQueue = new MyPriorityQueue<OurEvents>();
        public HashSet<string> UniqueCategories { get; private set; } = new HashSet<string>();
        public Hashtable EventsLookup { get; private set; } = new Hashtable();


        public async Task PopulateEventsAsync(Action<OurEvents> onEventScraped)
        {
            var scraper = new ScraperService();
            await scraper.ScrapeEventsAsync(ev =>
            {
                Events.Add(ev);
                AddCategoriesToSet(ev);
                AddEventToLookup(ev);
                onEventScraped?.Invoke(ev); // Notify that an event was scraped
            });
        }

        private void AddCategoriesToSet(OurEvents ev)
        {
            if (ev.Categories != null)
            {
                foreach (var category in ev.Categories)
                {
                    UniqueCategories.Add(category);
                }
            }
        }

        private void AddEventToLookup(OurEvents ev)
        {
            EventsLookup[ev.Id] = ev;
        }

        public OurEvents GetEventById(Guid eventId)
        {
            if (EventsLookup.ContainsKey(eventId))
            {
                return (OurEvents)EventsLookup[eventId]; // Cast is required here
            }
            return null; // Or throw an exception if you prefer
        }

        private List<OurEvents> GetEventsNow() {
            var EventsNow = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                {
                    EventsNow.Add(ev);
                }
            }
            return EventsNow;
        }

        private List<OurEvents> GetEventsThisWeek() {
           
            var EventsThisWeek = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                {
                    EventsThisWeek.Add(ev);
                }
            }
            return EventsThisWeek;
        }

        private List<OurEvents> GetEventsThisMonth() {
            var EventsThisMonth = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddMonths(1))
                {
                    EventsThisMonth.Add(ev);
                }
            }
            return EventsThisMonth;
        }

        private void priorityCalc()
        {
            // Get the event lists
            var eventsNow = GetEventsNow();
            var eventsThisWeek = GetEventsThisWeek();
            var eventsThisMonth = GetEventsThisMonth();

            // Assign priorities based on the list
            int priorityNow = 1; // Highest priority
            int priorityThisWeek = 2;
            int priorityThisMonth = 3;

            // Populate the priority queue
            foreach (var ev in eventsNow)
            {
                AnnouncementEventsQueue.Enqueue(ev, priorityNow);
            }

            foreach (var ev in eventsThisWeek)
            {
                AnnouncementEventsQueue.Enqueue(ev, priorityThisWeek);
            }

            foreach (var ev in eventsThisMonth)
            {
                AnnouncementEventsQueue.Enqueue(ev, priorityThisMonth);
            }
        }
    }
}
