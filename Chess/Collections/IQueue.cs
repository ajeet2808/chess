using System.Collections.Generic;

namespace Green.Collections
{
    public interface IQueue<T>
    {
        void Enqueue(T item);
        void Enqueue(IEnumerable<T> item);
        T Dequeue();
        T Peek();

        int Count { get; }
    }
}