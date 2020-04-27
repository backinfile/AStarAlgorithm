using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithm.Core
{
    internal class Node: IComparable<Node>
    {
        public int x;
        public int y;
        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 总权值，越小越好
        /// </summary>
        public float F => G + H;

        /// <summary>
        /// 从起点移动到此的移动代价
        /// </summary>
        public float G { get; set; }

        /// <summary>
        /// 从此移动到终点的估算成本
        /// </summary>
        public float H { get; set; }

        //public readonly List<Node> path = new List<Node>();
        // 用于记录路径
        public Node from = null;
        internal bool isClosed = false;
        internal bool isOpened = false;

        public int CompareTo(Node other)
        {
            return F.CompareTo(other.F);
        }
    }
}
