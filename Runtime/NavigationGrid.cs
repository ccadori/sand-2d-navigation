using Sand.Navigation.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationGrid : MonoBehaviour
    {
        public Vector2 nodeSize;
        public bool allowDiagonalMove;
        public bool limitAgentsWalkingPerNode;
        public int agentsWalkingPerNode;
        public bool limitAgentsPerNode;
        public int agentsPerNode;

        public List<NavigationAgent> Agents { get; set; }
        public Dictionary<Int2, NavigationNode> Nodes { get; set; }
        
        private float lastCacheUpdate;

        public void Awake()
        {
            Agents = new List<NavigationAgent>();
            Nodes = new Dictionary<Int2, NavigationNode>();
            lastCacheUpdate = 0;
        }

        public List<NavigationNode> GetPath(NavigationNode start, NavigationNode target, NavigationAgent agent) 
        {
            if (!CanOccupy(target, agent))
            {
                return null;
            }

            return Utils.Math.GetPath(start, target, agent, this);
        }

        public NavigationNode GetClosestNode(Vector2 point)
        {
            NavigationNode closestNode = null;
            float distance = Mathf.Infinity;

            foreach (KeyValuePair<Int2, NavigationNode> node in Nodes)
            {
                if (distance > Vector2.Distance(node.Value.transform.position, point))
                {
                    closestNode = node.Value;
                    distance = Vector2.Distance(node.Value.transform.position, point);
                }
            }

            return closestNode;
        }

        public Int2 GetIndex(Vector2 position)
        {
            return Utils.Math.CalculateIndexFromPosition(nodeSize, position);
        }

        public NavigationNode GetNode(Int2 index)
        {
            if (Nodes.ContainsKey(index))
                return Nodes[index];
            else
                return null;
        }

        public NavigationNode GetNode(Vector2 position)
        {
            return GetNode(GetIndex(position));
        }

        public List<NavigationNode> GetNeighbors(NavigationNode root)
        {
            if (root.LastNeighborUpdate > lastCacheUpdate)
            {
                return root.CachedNeighbors;
            }

            List<NavigationNode> neighbors = new List<NavigationNode>();

            var left = GetNode(new Int2(root.Index.x - 1, root.Index.y));
            var right = GetNode(new Int2(root.Index.x + 1, root.Index.y));
            var top = GetNode(new Int2(root.Index.x, root.Index.y + 1));
            var bottom = GetNode(new Int2(root.Index.x, root.Index.y - 1));

            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);
            if (top != null) neighbors.Add(top);
            if (bottom != null) neighbors.Add(bottom);

            if (allowDiagonalMove)
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
            root.LastNeighborUpdate = Time.deltaTime;
            return neighbors;
        }

        internal bool CanOccupy(NavigationNode node, NavigationAgent agent)
        {
            if (limitAgentsPerNode)
            {
                return GetAgentsOccupyingNode(node, agent) < agentsPerNode;
            }

            return true;
        }

        internal bool CanWalkThrough(NavigationNode node, NavigationAgent agent)
        {
            if (limitAgentsWalkingPerNode)
            {
                return GetAgentsWalkingOnNode(node, agent) < agentsWalkingPerNode;
            }
            
            return true;
        }

        public void UpdateCache()
        {
            lastCacheUpdate = Time.deltaTime;
        }

        public void AddNode(NavigationNode node)
        {
            if (Nodes == null) Nodes = new Dictionary<Int2, NavigationNode>();

            var index = Utils.Math.CalculateIndexFromPosition(nodeSize, node.transform.position);
            node.Index = index;

            if (Nodes.ContainsKey(node.Index))
            {
                Nodes[node.Index] = node;
            }
            else
            {
                Nodes.Add(node.Index, node);
            }

            UpdateCache();
        }

        public void RemoveNode(NavigationNode node)
        {
            if (Nodes == null) Nodes = new Dictionary<Int2, NavigationNode>();

            Nodes.Remove(node.Index);

            UpdateCache();
        }

        public void AddAgent(NavigationAgent agent) 
        {
            if (Agents.Find((a) => agent == a) != null) 
                return; 

            Agents.Add(agent);
        }

        public void RemoveAgent(NavigationAgent agent)
        {
            Agents.Remove(agent);
        }

        public int GetAgentsOccupyingNode(NavigationNode node, NavigationAgent agent)
        {
            int count = 0;

            foreach (NavigationAgent a in Agents)
            {
                if (a != agent && a.OccupiedNode == node)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetAgentsWalkingOnNode(NavigationNode node, NavigationAgent agent)
        {
            int count = 0;

            foreach (NavigationAgent a in Agents)
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
