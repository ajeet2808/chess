using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Collections
{
    public interface IPriorityQueue<T> : IQueue<T> where T : IRankable
    {

    }
}
