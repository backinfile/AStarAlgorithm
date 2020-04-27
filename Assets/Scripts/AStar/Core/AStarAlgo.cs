using AStarAlgorithm.Heap;
using System;
using System.Collections.Generic;

namespace AStarAlgorithm.Core
{
    public class AStarAlgo
    {
        #region public var
        public int Width => map.GetLength(0);
        public int Height => map.GetLength(1);
        public int OffsetX => 0;
        public int OffsetY => 0;
        #endregion

        #region private var
        internal readonly Node[,] nodes;
        private readonly int[,] map;

        internal readonly MinHeap<Node> OpenList = new MinHeap<Node>();

        private static readonly int[] dx = { 1, -1, 0, 0, 1, -1, 1, -1 };
        private static readonly int[] dy = { 0, 0, 1, -1, 1, -1, -1, 1 };
        #endregion

        #region public method

        public AStarAlgo(int[,] map)
        {
            this.map = map;
            nodes = new Node[map.GetLength(0), map.GetLength(1)];
        }

        public List<(int x, int y)> GetPath((int x, int y) from, (int x, int y) to, Func<int,bool> IsWall = null)
        {
            if (IsWall == null) IsWall = value => value == -1;
            ClearAllNode();
            Node startNode = GetNodeByPoint(from);
            Node endNode = GetNodeByPoint(to);
            OpenList.Clear();
            OpenList.Add(startNode);

            while (!OpenList.Empty)
            {
                // poptop
                var node = OpenList.PopTop();
                // check reach & return path
                if (node.x == endNode.x && node.y == endNode.y)
                {
                    return GetPath(node);
                }
                // get nodes and filter
                List<Node> nodes = GetRoundedNodes(node, IsWall);
                // 设置权值
                foreach (var n in nodes)
                {
                    SetF(n, node, endNode);
                    // join in open list
                    if (n.isOpened)
                    {
                        OpenList.RebuildByElement(n);
                    }
                    else
                    {
                        OpenList.Add(n);
                        n.isOpened = true;
                    }
                }
                // put cur node in closed node
                node.isClosed = true;
            }
            return null;
        }

        #endregion

        #region private method

        private void SetF(Node node, Node curNode, Node endNode)
        {
            if (node.from == null) SetNodeFrom(node, curNode, endNode);
            else
            {
                var oldFrom = node.from;
                var oldF = node.F;
                var newF = SetNodeFrom(node, curNode, endNode);
                if (oldF < newF)
                {
                    SetNodeFrom(node, oldFrom, endNode);
                }
            }
        }
        private float SetNodeFrom(Node node, Node from, Node endNode)
        {
            node.from = from;
            if (node.from == null)
            {
                node.G = 0;
            }
            else
            {
                int cnt = 0;
                if (node.x != node.from.x) cnt++;
                if (node.y != node.from.y) cnt++;
                if (cnt == 2) node.G = node.from.G + 1.4f;
                else node.G = node.from.G + 1;
            }
            node.H = Math.Abs(node.x - endNode.x) + Math.Abs(node.y - endNode.y);
            //node.H = GetDistance(node, endNode);
            return node.F;
        }

        private float GetDistance(Node node1, Node node2)
        {
            var dx = node1.x - node2.x;
            var dy = node1.y - node2.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private List<(int x, int y)> GetPath(Node node)
        {
            List<(int x, int y)> path = new List<(int x, int y)>();
            path.Add((node.x, node.y));
            while(node.from != null)
            {
                node = node.from;
                path.Add((node.x, node.y));
            }
            path.Reverse();
            return path;
        }

        private List<Node> GetRoundedNodes(Node node, Func<int, bool> isWall)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < dx.Length; i++)
            {
                int nx = node.x + dx[i];
                int ny = node.y + dy[i];

                // 不能超出边界
                if (nx >= OffsetX && nx < OffsetX + Width && ny >= OffsetY && ny < OffsetY + Height) 
                {
                    // 不能到墙上
                    if (isWall(map[nx, ny])) continue;

                    // 两边都是墙时，不能以对角线跨越墙
                    if (dx[i] != 0 && dy[i] != 0)
                    {
                        int cx1 = node.x + dx[i];
                        int cy1 = node.y + 0;
                        int cx2 = node.x + 0;
                        int cy2 = node.y + dy[i];

                        if (isWall(map[cx1, cy1]) && isWall(map[cx2, cy2])) continue;
                    }

                    Node nNode = GetNodeByPoint((nx, ny));
                    // 节点位于关闭集合
                    if (nNode.isClosed) continue;

                    nodes.Add(nNode);
                }
            }
            return nodes;
        }


        private Node GetNodeByPoint((int x, int y) point)
        {
            if (nodes[point.x, point.y] == null)
            {
                nodes[point.x, point.y] = new Node(point.x, point.y);
            }
            return nodes[point.x, point.y];
        }
        private void ClearAllNode()
        {
            for (int i = OffsetX; i < OffsetX + Width; i++) 
            {
                for (int j = OffsetY; j < OffsetY + Height; j++)
                {
                    Node node = nodes[i, j];
                    if (node != null)
                    {
                        node.isClosed = false;
                        node.isOpened = false;
                        node.from = null;
                    }
                }
            }
        }

        #endregion
    }
}
