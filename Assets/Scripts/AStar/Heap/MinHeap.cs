using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithm.Heap
{
    public class MinHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private readonly List<T> values = new List<T>();


        public void Add(T t)
        {
            values.Add(t);
            RebuildUp(values.Count-1);
        }
        public void AddRange(IEnumerable<T> ts)
        {
            foreach(var item in ts)
            {
                Add(item);
            }
        }

        public T PopTop()
        {
            T retValue = this[0];
            Swap(0, Count - 1);
            RemoveAt(Count - 1);
            RebuildDown(0);
            return retValue;
        }


        #region rebuild

        public void RebuildByElement(T ele)
        {
            int position = values.IndexOf(ele);
            RebuildUp(position);
            RebuildDown(position);
        }

        /// <summary>
        /// 上浮
        /// </summary>
        /// <param name="position"></param>
        private void RebuildUp(int position)
        {
            if (position == 0) return;
            int father = GetParentIndex(position);
            if (this[father].CompareTo(this[position]) > 0)
            {
                Swap(father, position);
                RebuildUp(father);
            }
        }

        /// <summary>
        /// 下沉
        /// </summary>
        /// <param name="position"></param>
        private void RebuildDown(int position)
        {
            int left = GetLeftChildIndex(position);
            int right = GetRightChildIndex(position);
            int length = Count;
            int min = -1;
            if (left < length && right < length)
            {
                min = this[left].CompareTo(this[right]) > 0 ? right : left;
            }
            else if (left < length)
            {
                min = left;
            }
            else if (right < length)
            {
                min = right;
            }
            if (min >= 0 && this[min].CompareTo(this[position]) < 0)
            {
                Swap(min, position);
                RebuildDown(min);
            }
        }
        
        #endregion

        #region from values list

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveAt(int v)
        {
            values.RemoveAt(v);
        }
        public bool Empty => !values.Any();
        public int Count => values.Count;

        public void Clear()
        {
            values.Clear();
        }
        private T this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }

        #endregion


        #region inline function

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetParentIndex(int index)
        {
            return (index-1) / 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLeftChildIndex(int index)
        {
            return index * 2 + 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetRightChildIndex(int index)
        {
            return index * 2 + 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Swap(int first, int second)
        {
            T tmp = this[first];
            this[first] = values[second];
            this[second] = tmp;
        }

        #endregion
    }
}
