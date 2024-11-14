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
        public string FilterCatagory { get; set; }

        /// <summary>
        /// Gets or sets the filter date.
        /// </summary>
        public DateTime FilterDate { get; set; }

        /// <summary>
        /// Gets or sets the filter events.
        /// </summary>
        public ObservableCollection<OurEvents> FilterEvents { get; set; }
    }
}
//____________________________________EOF_________________________________________________________________________