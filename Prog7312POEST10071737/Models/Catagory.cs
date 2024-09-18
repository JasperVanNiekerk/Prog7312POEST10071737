using System;
using System.Collections.Generic;

namespace Prog7312POEST10071737.Models
{
    internal class Category
    {
        private int CategoryId;
        private string CategoryName;

        
        public Category(int id, string name)
        {
            CategoryId = id;
            CategoryName = name;
        }

        
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
        public override string ToString()
        {
            return CategoryName;
        }
    }
}
