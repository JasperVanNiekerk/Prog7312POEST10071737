using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prog7312POEST10071737.Models
{
    public class OurEvents
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the title of the event.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the dates of the event as a string.
        /// </summary>
        public string Dates { get; set; }

        /// <summary>
        /// Gets or sets the venue of the event.
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the event.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets the start date of the event.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the end date of the event.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Gets or sets the list of images associated with the event.
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// Gets or sets the list of categories associated with the event.
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the display title (including any priority indicators)
        /// </summary>
        public string DisplayTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OurEvents"/> class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dates"></param>
        /// <param name="venue"></param>
        /// <param name="phone"></param>
        /// <param name="images"></param>
        /// <param name="categories"></param>
        public OurEvents(string title, string description, string dates, string venue, string phone, List<string> images, List<string> categories)
        {
            Title = title;
            Description = description;
            Dates = dates;
            Venue = venue;
            Phone = phone;
            Images = images;
            Categories = categories;

            // Extract dates from the dates string
            try
            {
                (StartDate, EndDate) = ExtractDates(dates);
            }
            catch (FormatException ex)
            {
                // Handle any format exceptions that may arise
                Console.WriteLine($"Error parsing dates: {ex.Message}");
                StartDate = DateTime.MinValue; // Set to a default value or throw an exception based on your needs
                EndDate = DateTime.MinValue;   // Same as above
            }

            DisplayTitle = Title;  // Initialize with regular title
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Extracts the start and end dates from the given date range string.
        /// </summary>
        /// <param name="dateRange">The date range string in the format "startDay-endDay month year".</param>
        /// <returns>A tuple containing the start and end dates.</returns>
        public static (DateTime StartDate, DateTime EndDate) ExtractDates(string dateRange)
        {
            string[] parts = dateRange.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int startDay = int.Parse(parts[0]);
            int endDay = int.Parse(parts[1]);
            string month = parts[2];
            int year = int.Parse(parts[3]);

            DateTime startDate = DateTime.ParseExact($"{startDay} {month} {year}", "d MMMM yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact($"{endDay} {month} {year}", "d MMMM yyyy", CultureInfo.InvariantCulture);

            return (startDate, endDate);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Searches for the given search string in the event's title or description.
        /// </summary>
        /// <param name="searchString">The search string to look for.</param>
        /// <returns>True if the search string is found in the title or description, otherwise false.</returns>
        public bool SearchEvent(string searchString)
        {
            // Ensure search is case-insensitive by converting both strings to lower case
            string lowerSearchString = searchString.ToLower();

            // Check if the search string is in the Title or Description
            bool isInTitle = Title?.ToLower().Contains(lowerSearchString) ?? false;
            bool isInDescription = Description?.ToLower().Contains(lowerSearchString) ?? false;

            // Return true if the string is found in either the title or description
            return isInTitle || isInDescription;
        }
    }
}
//____________________________________EOF_________________________________________________________________________