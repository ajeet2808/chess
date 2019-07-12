using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Collections
{
    public class PriorityQueue<T> : IPriorityQueue<T> where T : IRankable
    {
        private MinHeap<T> _minHeap;
        public PriorityQueue()
        {
            _minHeap = new MinHeap<T>();
        }
        public PriorityQueue(List<T> items)
        {
            _minHeap = new MinHeap<T>(items);
        }
        public T Dequeue()
        {
            var item = _minHeap.RemoveItem();
            return item;
        }

        public void Enqueue(T item)
        {
            _minHeap.Add(item);
        }

        public void Enqueue(IEnumerable<T> item)
        {
            _minHeap.AddItems(item);
        }

        public T Peek()
        {
            var item = _minHeap.Peek();
            return item;
        }

        public int Count => _minHeap.Count;
    }
}
