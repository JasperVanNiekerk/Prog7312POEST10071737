using Prog7312POEST10071737.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prog7312POEST10071737.Services
{
    /// <summary>
    /// Singleton class for managing local events.
    /// </summary>
    public class LocalEventsSingleton
    {
        private static LocalEventsSingleton instance = null;
        private static readonly object padlock = new object();

        private LocalEventsSingleton()
        {
        }

        /// <summary>
        /// Gets the instance of the LocalEventsSingleton class.
        /// </summary>
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

        /// <summary>
        /// Data structure to hold all the events.
        /// </summary>
        public List<OurEvents> Events { get; set; } = new List<OurEvents>();

        /// <summary>
        /// Priority queue for announcement events.
        /// </summary>
        public MyPriorityQueue<OurEvents> AnnouncementEventsQueue = new MyPriorityQueue<OurEvents>();

        /// <summary>
        /// Set of unique categories.
        /// </summary>
        public HashSet<string> UniqueCategories { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Lookup table for events.
        /// </summary>
        public Hashtable EventsLookup { get; private set; } = new Hashtable();

        /// <summary>
        /// Lookup table for events by category.
        /// </summary>
        public Hashtable CategoryEventsLookup { get; private set; } = new Hashtable();

        /// <summary>
        /// Stack of filter models.
        /// </summary>
        public Stack<FilterModel> Filters { get; set; } = new Stack<FilterModel>();

        private HashSet<string> _eventTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //___________________________________________________________________________________________________________

        /// <summary>
        /// Populates the events asynchronously.
        /// </summary>
        /// <param name="onEventScraped">The action to be performed on each scraped event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PopulateEventsAsync(Action<OurEvents> onEventScraped)
        {
            var scraper = new ScraperService();
            await scraper.ScrapeEventsAsync(ev =>
            {
                if (AddEventIfNotExists(ev))
                {
                    AddCategoriesToSet(ev);
                    AddEventToLookup(ev);
                    AddEventToCategoryLookup(ev);
                    onEventScraped?.Invoke(ev);
                }
            });
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds an event to the Events list if it does not already exist.
        /// </summary>
        /// <param name="ev">The event to add.</param>
        /// <returns>True if the event was added, false otherwise.</returns>
        private bool AddEventIfNotExists(OurEvents ev)
        {
            if (!string.IsNullOrWhiteSpace(ev.Title) && _eventTitles.Add(ev.Title))
            {
                Events.Add(ev);
                return true;
            }
            return false;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds the categories of an event to the set of unique categories.
        /// </summary>
        /// <param name="ev">The event.</param>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds an event to the Events lookup table.
        /// </summary>
        /// <param name="ev">The event to add.</param>
        private void AddEventToLookup(OurEvents ev)
        {
            EventsLookup[ev.Id] = ev;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves an event by its ID.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>The event with the specified ID, or null if not found.</returns>
        public OurEvents GetEventById(Guid eventId)
        {
            if (EventsLookup.ContainsKey(eventId))
            {
                return (OurEvents)EventsLookup[eventId]; // Cast is required here
            }
            return null; // Or throw an exception if you prefer
        }

        /// <summary>
        /// Adds an event to the Events lookup table.
        /// </summary>
        /// <param name="ev">The event to add.</param>
        private void AddEventToCategoryLookup(OurEvents ev)
        {
            if (ev.Categories != null)
            {
                foreach (var category in ev.Categories)
                {
                    if (!CategoryEventsLookup.ContainsKey(category))
                    {
                        CategoryEventsLookup[category] = new List<OurEvents>();
                    }
                    ((List<OurEvents>)CategoryEventsLookup[category]).Add(ev);
                }
            }
        }

        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The list of events with the specified category.</returns>
        public List<OurEvents> GetEventsByCategory(string category)
        {
            if (CategoryEventsLookup.ContainsKey(category))
            {
                return (List<OurEvents>)CategoryEventsLookup[category];
            }
            return new List<OurEvents>();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds a filter to the stack.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        public void AddFilter(FilterModel filter)
        {
            Filters.Push(filter);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Removes a filter from the stack.
        /// </summary>
        /// <returns>The removed filter, or null if the stack is empty.</returns>
        public FilterModel RemoveFilter()
        {
            if (Filters.Count > 0)
            {
                return Filters.Pop();
            }
            return null;
        }

        /// <summary>
        /// Gets the events that match the search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>The list of events that match the search string.</returns>
        public List<OurEvents> GetSearchedEvents(string searchString)
        {
            List<OurEvents> searchedEvents = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.SearchEvent(searchString))
                {
                    searchedEvents.Add(ev);
                }
            }
            return searchedEvents;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the three most similar events to the specified event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>The list of three most similar events.</returns>
        public List<OurEvents> GetThreeMostSimilarEvents(Guid eventId)
        {
            var sourceEvent = GetEventById(eventId);
            if (sourceEvent == null || sourceEvent.Categories == null || sourceEvent.Categories.Count == 0)
            {
                return new List<OurEvents>();
            }

            var similarityScores = new List<(OurEvents Event, double Similarity)>();

            foreach (var ev in Events)
            {
                if (ev.Id != eventId && ev.Categories != null && ev.Categories.Count > 0)
                {
                    double similarity = CalculateJaccardSimilarity(sourceEvent.Categories, ev.Categories);
                    similarityScores.Add((ev, similarity));
                }
            }

            return similarityScores
                .OrderByDescending(x => x.Similarity)
                .Take(3)
                .Select(x => x.Event)
                .ToList();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Calculates the Jaccard similarity between two sets.
        /// </summary>
        /// <param name="set1">The first set.</param>
        /// <param name="set2">The second set.</param>
        /// <returns>The Jaccard similarity between the two sets.</returns>
        private double CalculateJaccardSimilarity(List<string> set1, List<string> set2)
        {
            var intersection = set1.Intersect(set2).Count();
            var union = set1.Union(set2).Count();
            return (double)intersection / union;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events happening now.
        /// </summary>
        /// <returns>The list of events happening now.</returns>
        private List<OurEvents> GetEventsNow()
        {
            var eventsNow = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate <= DateTime.Now && ev.EndDate >= DateTime.Now)
                {
                    eventsNow.Add(ev);
                }
            }
            return eventsNow;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events happening this week.
        /// </summary>
        /// <returns>The list of events happening this week.</returns>
        private List<OurEvents> GetEventsThisWeek()
        {
            var eventsThisWeek = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddDays(7))
                {
                    eventsThisWeek.Add(ev);
                }
            }
            return eventsThisWeek;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events happening this month.
        /// </summary>
        /// <returns>The list of events happening this month.</returns>
        private List<OurEvents> GetEventsThisMonth()
        {
            var eventsThisMonth = new List<OurEvents>();
            foreach (var ev in Events)
            {
                if (ev.StartDate >= DateTime.Now && ev.StartDate <= DateTime.Now.AddMonths(1))
                {
                    eventsThisMonth.Add(ev);
                }
            }
            return eventsThisMonth;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Calculates the priority of events and populates the priority queue.
        /// </summary>
        private void PriorityCalc()
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
//____________________________________EOF_________________________________________________________________________