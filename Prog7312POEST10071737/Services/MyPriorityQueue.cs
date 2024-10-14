using System;
using System.Collections.Generic;

namespace Prog7312POEST10071737.Services
{
    public class MyPriorityQueue<T>
    {
        private List<(T item, int priority)> elements = new List<(T, int)>();

        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
            elements.Sort((x, y) => x.priority.CompareTo(y.priority)); // Sort by priority
        }

        public T Dequeue()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            var highestPriorityItem = elements[0];
            elements.RemoveAt(0); // Remove the item with the highest priority
            return highestPriorityItem.item;
        }

        public int Count => elements.Count;
    }
}
