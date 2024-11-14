using System;
using System.Collections.Generic;
using System.Linq; 

namespace Prog7312POEST10071737.Services
{
    /// <summary>
    /// Represents a priority queue data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the priority queue.</typeparam>
    public class MyPriorityQueue<T>
    {
        private List<(T item, int priority)> elements = new List<(T, int)>();
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds an item with the specified priority to the priority queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="priority">The priority of the item.</param>
        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
            elements.Sort((x, y) => x.priority.CompareTo(y.priority)); // Sort by priority
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Removes and returns the item with the highest priority from the priority queue.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Dequeue()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            var highestPriorityItem = elements[0];
            elements.RemoveAt(0); // Remove the item with the highest priority
            return highestPriorityItem.item;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the number of items in the priority queue.
        /// </summary>
        /// <returns>The number of items in the priority queue.</returns>
        public int Count()
        {
            return elements.Count;
        }

        /// <summary>
        /// Attempts to peek at the next item without removing it.
        /// </summary>
        public bool TryPeek(out T item)
        {
            if (elements.Count > 0)
            {
                item = elements[0].item;
                return true;
            }
            item = default;
            return false;
        }

        /// <summary>
        /// Clears all items from the queue.
        /// </summary>
        public void Clear()
        {
            elements.Clear();
        }

        /// <summary>
        /// Returns all items in priority order.
        /// </summary>
        public IEnumerable<T> GetAllItems()
        {
            return elements.Select(e => e.item);
        }
    }
}
//____________________________________EOF_________________________________________________________________________