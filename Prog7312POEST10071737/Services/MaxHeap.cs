using System;
using System.Collections.Generic;

namespace Prog7312POEST10071737.Services
{
    public class MaxHeap<T>
    {
        private List<T> heap;
        private readonly Func<T, int> getPriority;

        public MaxHeap(Func<T, int> prioritySelector)
        {
            heap = new List<T>();
            getPriority = prioritySelector;
        }

        public int Count => heap.Count;

        public void Insert(T item)
        {
            heap.Add(item);
            HeapifyUp(heap.Count - 1);
        }

        public T ExtractMax()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            T max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            if (heap.Count > 0)
                HeapifyDown(0);

            return max;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (getPriority(heap[index]) <= getPriority(heap[parentIndex]))
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int largest = index;
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;

                if (leftChild < heap.Count && getPriority(heap[leftChild]) > getPriority(heap[largest]))
                    largest = leftChild;

                if (rightChild < heap.Count && getPriority(heap[rightChild]) > getPriority(heap[largest]))
                    largest = rightChild;

                if (largest == index)
                    break;

                Swap(index, largest);
                index = largest;
            }
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public List<T> GetSortedItems()
        {
            List<T> sorted = new List<T>();
            List<T> tempHeap = new List<T>(heap);

            while (heap.Count > 0)
            {
                sorted.Add(ExtractMax());
            }

            heap = tempHeap;
            return sorted;
        }
    }
}
