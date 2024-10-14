using HtmlAgilityPack;
using Prog7312POEST10071737.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Prog7312POEST10071737.Services
{
    public class ScraperService
    {

        private readonly string url = "https://www.sa-venues.com/events/westerncape/"; // The website's event page

        private async Task<List<string>> ScrapeEventLinksAsync()
        {
            var urls = new List<string>
            {
                "https://www.sa-venues.com/events/westerncape/",
                "https://www.sa-venues.com/events/westerncape/page/2/",
                "https://www.sa-venues.com/events/westerncape/page/3/"
            };

            var eventLinks = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                foreach (var url in urls)
                {
                    var html = await client.GetStringAsync(url);
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);

                    // Adjust the XPath to target the relevant h4/a link inside the event listing
                    var eventNodes = document.DocumentNode.SelectNodes("//div[@class='details']//h4/a");
                    if (eventNodes != null)
                    {
                        foreach (var node in eventNodes)
                        {
                            // Extract the href (link) of the event title
                            var eventLink = node.GetAttributeValue("href", string.Empty);
                            if (!string.IsNullOrEmpty(eventLink))
                            {
                                eventLinks.Add(eventLink);
                            }
                        }
                    }
                }
            }

            return eventLinks;
        }



        private async Task<OurEvents> ScrapeEventDetailsAsync(string eventUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var html = await client.GetStringAsync(eventUrl);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                // Extract the event title (H1)
                var titleNode = document.DocumentNode.SelectSingleNode("//h1");
                string title = titleNode != null ? titleNode.InnerText.Trim() : string.Empty;

                // Extract the images from the gallery (all img srcset)
                var imageNodes = document.DocumentNode.SelectNodes("//div[@id='img_gallery']//img");
                var images = new List<string>();
                if (imageNodes != null)
                {
                    foreach (var img in imageNodes)
                    {
                        var imageSrc = img.GetAttributeValue("srcset", string.Empty);
                        if (!string.IsNullOrEmpty(imageSrc))
                        {
                            images.Add(imageSrc);
                        }
                    }
                }

                // Extract the description from the "ttd_about" section
                var descriptionNodes = document.DocumentNode.SelectNodes("//div[@class='section ttd_about mar_45b']//p");
                string description = "";
                if (descriptionNodes != null)
                {
                    foreach (var descNode in descriptionNodes)
                    {
                        description += descNode.InnerText.Trim() + " ";
                    }
                    description = description.Trim(); // Remove trailing space
                }
                else
                {
                    Console.WriteLine("Description Nodes Not Found");
                }

                // Extract the event date (when)
                var dateNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'box bkg_lrgrey card')]//p[b[contains(text(), 'When')]]");
                string date = dateNode != null ? dateNode.InnerText.Replace("When", "").Trim() : string.Empty;

                // Alternatively, you could extract the content of the same <p> tag:
                if (dateNode != null)
                {
                    date = dateNode.InnerText.Substring(dateNode.InnerText.IndexOf("When") + 5).Trim(); // Adjusting to get only the date part
                }

                // Extract the venue information (location)
                var venueNode = document.DocumentNode.SelectSingleNode("//p[b[contains(text(), 'Venue')]]");
                string venue = venueNode != null ? venueNode.InnerText.Replace("Venue:", "").Trim() : string.Empty;

                // Extract the telephone number
                var phoneNode = document.DocumentNode.SelectSingleNode("//p[b[contains(text(), 'Telephone')]]");
                string phone = phoneNode != null ? phoneNode.InnerText.Replace("Telephone", "").Trim() : string.Empty;

                // Extract event types (categories)
                var categoryNodes = document.DocumentNode.SelectNodes("//div[@class='section est_categories']//a");
                var categories = new List<string>();
                if (categoryNodes != null)
                {
                    foreach (var cat in categoryNodes)
                    {
                        var category = cat.InnerText.Trim();
                        if (!string.IsNullOrEmpty(category))
                        {
                            categories.Add(category);
                        }
                    }
                }

                return new OurEvents(title, description, date, venue, phone, images, categories);
            }
        }

        public async Task ScrapeEventsAsync(Action<OurEvents> onEventScraped)
        {
            var eventLinks = await ScrapeEventLinksAsync();

            foreach (var link in eventLinks)
            {
                var eventDetails = await ScrapeEventDetailsAsync(link);
                onEventScraped?.Invoke(eventDetails);  // Notify that an event was scraped
            }
        }
    }
}
