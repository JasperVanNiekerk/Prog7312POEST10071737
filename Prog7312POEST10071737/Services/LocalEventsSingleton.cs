using Prog7312POEST10071737.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prog7312POEST10071737.Core;

namespace Prog7312POEST10071737.Services
{
    /// <summary>
    /// Singleton class for managing local events.
    /// </summary>
    public class LocalEventsSingleton : ObservableObject
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
        private ObservableCollection<OurEvents> _events;
        public ObservableCollection<OurEvents> Events
        {
            get { return _events; }
            set
            {
                if (_events != null)
                {
                    _events.CollectionChanged -= Events_CollectionChanged;
                }
                _events = value;
                if (_events != null)
                {
                    _events.CollectionChanged += Events_CollectionChanged;
                }
                OnPropertyChanged(nameof(Events));
            }
        }

        private void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (OurEvents newEvent in e.NewItems)
                {
                    AddCategoriesToSet(newEvent);
                    AddEventToLookup(newEvent);
                    AddEventToCategoryLookup(newEvent);
                }
            }
        }

        /// <summary>
        /// Priority queue for announcement events.
        /// </summary>
        public MyPriorityQueue<OurEvents> AnnouncementEventsQueue { get; private set; } = new MyPriorityQueue<OurEvents>();

        /// <summary>
        /// Set of unique categories.
        /// </summary>
        private HashSet<string> _uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyCollection<string> UniqueCategories => _uniqueCategories;

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

        private const string HIGH_PRIORITY_INDICATOR = "⭐ ";
        public static readonly int PRIORITY_THRESHOLD = 4;  // Made public and static

        //___________________________________________________________________________________________________________

        /// <summary>
        /// Populates the events asynchronously.
        /// </summary>
        /// <param name="onEventScraped">The action to be performed on each scraped event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PopulateEventsAsync(Action<OurEvents> onEventScraped)
        {
            var scraper = new ScraperService();
            Events = new ObservableCollection<OurEvents>();

            await scraper.ScrapeEventsAsync(ev =>
            {
                if (AddEventIfNotExists(ev))
                {
                    Events.Add(ev);
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
            return !string.IsNullOrWhiteSpace(ev.Title) && _eventTitles.Add(ev.Title);
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
                    _uniqueCategories.Add(category);
                }
                OnPropertyChanged(nameof(UniqueCategories));
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
            return Events.Where(ev => ev.SearchEvent(searchString)).ToList();
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
            var now = DateTime.Now;
            return Events.Where(ev => ev.StartDate <= now && ev.EndDate >= now).ToList();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events happening this week.
        /// </summary>
        /// <returns>The list of events happening this week.</returns>
        private List<OurEvents> GetEventsThisWeek()
        {
            var now = DateTime.Now;
            var weekFromNow = now.AddDays(7);
            return Events.Where(ev => ev.StartDate >= now && ev.StartDate <= weekFromNow).ToList();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the events happening this month.
        /// </summary>
        /// <returns>The list of events happening this month.</returns>
        private List<OurEvents> GetEventsThisMonth()
        {
            var now = DateTime.Now;
            var monthFromNow = now.AddMonths(1);
            return Events.Where(ev => ev.StartDate >= now && ev.StartDate <= monthFromNow).ToList();
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

        public void PopulateAnnouncementQueue()
        {
            AnnouncementEventsQueue.Clear();
            var now = DateTime.Now;
            var events = Events?.ToList() ?? new List<OurEvents>();
            
            if (events.Count == 0) return;

            // First pass: Add priority events
            foreach (var ev in events.OrderBy(e => e.StartDate))
            {
                var priority = CalculateEventPriority(ev);
                if (priority <= PRIORITY_THRESHOLD)
                {
                    // Add visual indicator for high-priority events
                    ev.DisplayTitle = HIGH_PRIORITY_INDICATOR + ev.Title;
                    AnnouncementEventsQueue.Enqueue(ev, priority);
                }
            }

            // If queue is empty, add all future events with adjusted priority
            if (AnnouncementEventsQueue.Count() == 0)
            {
                var futureEvents = events
                    .Where(e => e.StartDate >= now)
                    .OrderBy(e => e.StartDate);

                foreach (var ev in futureEvents)
                {
                    // Use original title without priority indicator
                    ev.DisplayTitle = ev.Title;
                    AnnouncementEventsQueue.Enqueue(ev, 10 + (int)(ev.StartDate - now).TotalDays);
                }
            }
        }

        public string FormatEventBannerText(OurEvents ev)
        {
            var priority = CalculateEventPriority(ev);
            var timeStatus = GetEventTimeStatus(ev);
            return $"{ev.DisplayTitle} - {ev.Dates} {timeStatus}";
        }

        private string GetEventTimeStatus(OurEvents ev)
        {
            var now = DateTime.Now;
            
            if (ev.StartDate <= now && ev.EndDate >= now)
                return "(Happening Now)";
            
            if (ev.StartDate > now)
            {
                var daysUntil = (ev.StartDate - now).TotalDays;
                if (daysUntil <= 1)
                    return "(Starting Soon)";
                if (daysUntil <= 7)
                    return "(This Week)";
                if (daysUntil <= 30)
                    return "(This Month)";
            }
            
            return "";
        }

        public int CalculateEventPriority(OurEvents ev)
        {
            var now = DateTime.Now;
            var daysUntilStart = (ev.StartDate - now).TotalDays;
            var daysUntilEnd = (ev.EndDate - now).TotalDays;

            // Currently running events get highest priority
            if (ev.StartDate <= now && ev.EndDate >= now)
                return 1;
            
            // Events starting within 24 hours
            if (daysUntilStart >= 0 && daysUntilStart <= 1)
                return 2;
            
            // Events starting within the week
            if (daysUntilStart > 1 && daysUntilStart <= 7)
                return 3;
            
            // Events starting within two weeks
            if (daysUntilStart > 7 && daysUntilStart <= 14)
                return 4;
            
            // Events starting within the month
            if (daysUntilStart > 14 && daysUntilStart <= 30)
                return 5;
            
            // Future events beyond a month
            if (daysUntilStart > 30)
                return 6;
            
            // Past events get lowest priority
            return 7;
        }

        
        public void RefreshCategories()
        {
            _uniqueCategories.Clear();
            if (Events != null)
            {
                foreach (var ev in Events)
                {
                    if (ev.Categories != null)
                    {
                        foreach (var category in ev.Categories)
                        {
                            _uniqueCategories.Add(category);
                        }
                    }
                }
            }
            OnPropertyChanged(nameof(UniqueCategories));
        }

        /// <summary>
        /// Gets events filtered by category and date range.
        /// </summary>
        /// <param name="category">The category to filter by, or null for all categories.</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of events matching the criteria.</returns>
        public List<OurEvents> GetEventsByDateRange(string category, DateTime startDate, DateTime endDate)
        {
            // Start with all events
            var query = Events.AsEnumerable();
            
            // Filter by category if specified
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(ev => 
                    ev.Categories != null && 
                    ev.Categories.Contains(category, StringComparer.OrdinalIgnoreCase));
            }
            
            // Filter by date range
            // An event is within range if:
            // 1. It starts within the range, OR
            // 2. It ends within the range, OR
            // 3. It spans the entire range
            query = query.Where(ev =>
                (ev.StartDate >= startDate && ev.StartDate <= endDate) ||  // Starts within range
                (ev.EndDate >= startDate && ev.EndDate <= endDate) ||      // Ends within range
                (ev.StartDate <= startDate && ev.EndDate >= endDate));     // Spans the range
            
            return query.ToList();
        }
    }
}
//____________________________________EOF_________________________________________________________________________