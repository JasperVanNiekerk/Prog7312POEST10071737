using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prog7312POEST10071737.Models
{
    public class OurEvents
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public string Dates { get; set; }
        public string Venue { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public List<string> Images { get; set; }
        public List<string> Categories { get; set; }

        // Constructor to initialize the event with dates
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
        }

        public static (DateTime StartDate, DateTime EndDate) ExtractDates(string dateRange)//this method was written by copilot
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
    }
} 
