using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Collections
{
    public class MinHeap<T> where T : IRankable
    {
        private List<T> _items;
        private int _count;

        public MinHeap()
        {
            _items = new List<T>();
        }

        public MinHeap(List<T> items)
        {
            _items = new List<T>();
            foreach (var item in items)
            {
                _items.Add(item);
            }
            Heapify();
        }

        private void Heapify()
        {
            if (_items.Count == 0) return;
            var lastItemParent = GetParent(_items.Count - 1);
            for (int currentIndex = lastItemParent; currentIndex >= 0; currentIndex--)
            {
                Heapify(currentIndex);
            }
        }

        private void Heapify(int index)
        {
            var currentItem = _items[index];
            var leftChildIndex = GetLeftChild(index);
            var rightChildIndex = GetRightChild(index);
            var bestRankedItemIndex = index;
            if (leftChildIndex < _items.Count && _items[leftChildIndex].Rank < _items[bestRankedItemIndex].Rank)
            {
                bestRankedItemIndex = leftChildIndex;
            }
            if (rightChildIndex < _items.Count && _items[rightChildIndex].Rank < _items[bestRankedItemIndex].Rank)
            {
                bestRankedItemIndex = rightChildIndex;
            }
            if (index != bestRankedItemIndex)
            {
                var temp = _items[index];
                _items[index] = _items[bestRankedItemIndex];
                _items[bestRankedItemIndex] = temp;
                Heapify(bestRankedItemIndex);
            }
        }

        public void Add(T item)
        {
            _items.Add(item);
            Heapify();
        }

        public void AddItems(IEnumerable<T> items)
        {
            _items.AddRange(items);
            Heapify();
        }

        public T RemoveItem()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Heap is empty cannot remove any item!");
            var item = _items[0];
            _items[0] = _items[_items.Count - 1];
            _items[_items.Count - 1] = item;
            _items.RemoveAt(_items.Count - 1);
            Heapify();
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0) throw new InvalidOperationException("Heap is empty!");
            var item = _items[0];
            return item;
        }

        public int Count => _items.Count;

        private int GetParent(int childIndex)
        {
            return (childIndex + 1) / 2 - 1;
        }

        private int GetLeftChild(int parentIndex)
        {
            return parentIndex * 2 + 1;
        }

        private int GetRightChild(int parentIndex)
        {
            return parentIndex * 2 + 2;
        }
    }
}
