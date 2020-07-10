using UnityEngine;
using System.Collections.Generic;
using Sand.Navigation.Utils;

namespace Sand.Navigation.Api
{
    public class ApiGrid
    {
        public Vector2 NodeSize { get; set; }
        public bool AllowDiagonalMove { get; set; }
        public bool LimitAgentsPerNode { get; set; }
        public int AgentsPerNode { get; set; }
        public List<IAgent> Agents { get; set; }
        public Dictionary<Int2, INode> Nodes { get; set; }
        private float LastCacheUpdate { get; set; }

        public ApiGrid(Vector2 nodeSize, bool allowDiagonalMove, bool limitAgentsPerNode, int agentsPerNode)
        {
            NodeSize = nodeSize;
            AllowDiagonalMove = allowDiagonalMove;
            LimitAgentsPerNode = limitAgentsPerNode;
            AgentsPerNode = agentsPerNode;
            Agents = new List<IAgent>();
            Nodes = new Dictionary<Int2, INode>();
            LastCacheUpdate = 0;
        }

        public List<INode> GetPath(INode start, INode target, IAgent agent)
        {
            if (!CanOccupy(target, agent))
            {
                return null;
            }

            return Math.GetPath(start, target, this);
        }

        public INode GetClosestNode(Vector2 point)
        {
            INode closestNode = null;
            float distance = Mathf.Infinity;

            foreach (KeyValuePair<Int2, INode> node in Nodes)
            {
                if (distance > Vector2.Distance(node.Value.WorldPosition, point))
                {
                    closestNode = node.Value;
                    distance = Vector2.Distance(node.Value.WorldPosition, point);
                }
            }

            return closestNode;
        }

        public Int2 GetIndex(Vector2 position)
        {
            return Math.CalculateIndexFromPosition(NodeSize, position);
        }

        public INode GetNode(Int2 index)
        {
            if (Nodes.ContainsKey(index))
                return Nodes[index];
            else
                return null;
        }

        public INode GetNode(Vector2 position)
        {
            return GetNode(GetIndex(position));
        }

        public List<INode> GetNeighbors(INode root)
        {
            if (root.LastNeighborUpdate > LastCacheUpdate)
            {
                return root.CachedNeighbors;
            }

            List<INode> neighbors = new List<INode>();

            var left = GetNode(new Int2(root.Index.x - 1, root.Index.y));
            var right = GetNode(new Int2(root.Index.x + 1, root.Index.y));
            var top = GetNode(new Int2(root.Index.x, root.Index.y + 1));
            var bottom = GetNode(new Int2(root.Index.x, root.Index.y - 1));

            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);
            if (top != null) neighbors.Add(top);
            if (bottom != null) neighbors.Add(bottom);

            if (AllowDiagonalMove)
            {
                var topLeft = GetNode(new Int2(root.Index.x - 1, root.Index.y + 1));
                var topRight = GetNode(new Int2(root.Index.x + 1, root.Index.y + 1));
                var bottomLeft = GetNode(new Int2(root.Index.x - 1, root.Index.y - 1));
                var bottomRight = GetNode(new Int2(root.Index.x + 1, root.Index.y - 1));

                if (topLeft != null) neighbors.Add(topLeft);
                if (topRight != null) neighbors.Add(topRight);
                if (bottomLeft != null) neighbors.Add(bottomLeft);
                if (bottomRight != null) neighbors.Add(bottomRight);
            }

            root.CachedNeighbors = neighbors;

            return neighbors;
        }

        public bool CanOccupy(INode node, IAgent agent)
        {
            if (LimitAgentsPerNode)
            {
                return GetAgentsOccupyingNode(node, agent) < AgentsPerNode;
            }

            return true;
        }

        internal bool CanWalkThrough(INode node, IAgent agent)
        {
            return true;
        }

        public void UpdateCache(float time)
        {
            LastCacheUpdate = time;
        }

        public void AddNode(INode node)
        {
            if (Nodes == null) Nodes = new Dictionary<Int2, INode>();

            var index = Utils.Math.CalculateIndexFromPosition(NodeSize, node.WorldPosition);
            node.Index = index;

            if (Nodes.ContainsKey(node.Index))
            {
                Nodes[node.Index] = node;
            }
            else
            {
                Nodes.Add(node.Index, node);
            }
        }

        public void RemoveNode(INode node)
        {
            if (Nodes == null) Nodes = new Dictionary<Int2, INode>();

            Nodes.Remove(node.Index);
        }

        public void AddAgent(IAgent agent)
        {
            if (Agents.IndexOf(agent) != -1) 
                return;

            Agents.Add(agent);
        }

        public void RemoveAgent(IAgent agent)
        {
            Agents.Remove(agent);
        }

        public int GetAgentsOccupyingNode(INode node, IAgent agent)
        {
            int count = 0;

            foreach (IAgent a in Agents)
            {
                if (a != agent && a.OccupiedNode == node)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetAgentsWalkingOnNode(INode node, IAgent agent)
        {
            int count = 0;

            foreach (IAgent a in Agents)
            {
                if (a != agent && a.CurrentNode == node)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
