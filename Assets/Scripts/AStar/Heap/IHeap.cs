using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithm.Heap
{
    public interface IHeap<T>
    {
        T PopTop();

        void Add(T t);

        void AddRange(IEnumerable<T> ts);

        bool Empty { get; }
    }
}
