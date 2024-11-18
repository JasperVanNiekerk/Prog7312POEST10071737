using System.Collections.Generic;

namespace Prog7312POEST10071737.Models
{
    internal class Category
    {
        /// <summary>
        /// defines the category of the issue
        /// </summary>
        private int CategoryId;
        public string CategoryName;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// constructor for the category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Category(int id, string name)
        {
            CategoryId = id;
            CategoryName = name;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns list of all the categories
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetAllCategories()
        {
            return new List<Category>
            {
                new Category(1, "Roads and Transport"),
                new Category(2, "Water and Sanitation"),
                new Category(3, "Electricity"),
                new Category(4, "Waste Management"),
                new Category(5, "Public Safety"),
                new Category(6, "Parks and Recreation"),
                new Category(7, "Housing and Building"),
                new Category(8, "Environmental Issues"),
                new Category(9, "Health and Social Services"),
                new Category(10, "General Inquiries")
            };
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// returns the category name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CategoryName;
        }
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________