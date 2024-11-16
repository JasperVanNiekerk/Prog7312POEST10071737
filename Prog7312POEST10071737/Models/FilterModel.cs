using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog7312POEST10071737.Models
{
    public class FilterModel
    {
        /// <summary>
        /// Gets or sets the filter category.
        /// </summary>
        public string FilterCategory { get; set; }

        /// <summary>
        /// Gets or sets the filter start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the filter end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the filter events.
        /// </summary>
        public ObservableCollection<OurEvents> FilterEvents { get; set; }

        /// <summary>
        /// Clones the current filter model.
        /// </summary>
        public FilterModel Clone()
        {
            return new FilterModel
            {
                FilterCategory = this.FilterCategory,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                FilterEvents = new ObservableCollection<OurEvents>(this.FilterEvents)
            };
        }

        /// <summary>
        /// Determines if the current filter model is equal to another object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is FilterModel other)
            {
                return FilterCategory == other.FilterCategory &&
                       StartDate == other.StartDate &&
                       EndDate == other.EndDate;
            }
            return false;
        }
    }
}
//____________________________________EOF_________________________________________________________________________