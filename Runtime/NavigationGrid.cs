using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationGrid : MonoBehaviour
    {
        public Vector2 nodeSize;
        public bool allowDiagonalMove;
        
        public Dictionary<Int2, NavigationNode> nodes;
        private float lastCacheUpdate;
        private bool setupDone;

        public void Awake()
        {
            Setup();
        }

        protected void Setup()
        {
            if (setupDone) return;
            
            setupDone = true;
            
            var childrenNodes = GetComponentsInChildren<NavigationNode>();
            
            foreach(NavigationNode node in childrenNodes)
            {
                node.SetGrid(this);
                AddNode(node);
            }
            
            var agents = GetComponentsInChildren<NavigationAgent>();

            foreach (NavigationAgent agent in agents)
            {
                agent.SetGrid(this);
            }
        }

        public NavigationNode GetClosestNode(Vector2 point)
        {
            if (!setupDone) Setup();

            NavigationNode closestNode = null;
            float distance = Mathf.Infinity;

            foreach (KeyValuePair<Int2, NavigationNode> node in nodes)
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
            return Math.CalculateIndexFromPosition(nodeSize, position);
        }

        public NavigationNode GetNode(Int2 index)
        {
            if (!setupDone) Setup();

            if (nodes.ContainsKey(index))
                return nodes[index];
            else
                return null;
        }

        public NavigationNode GetNode(Vector2 position)
        {
            if (!setupDone) Setup();

            Debug.Log(GetIndex(position));

            return GetNode(GetIndex(position));
        }

        public (List<NavigationNode> neighbors, float cache) GetNeighbors(NavigationNode root, float lastUpdate)
        {
            if (lastUpdate > lastCacheUpdate)
            {
                return (root.cachedNeighbors, Time.deltaTime);
            }

            List<NavigationNode> neighbors = new List<NavigationNode>();

            var left = GetNode(new Int2(root.index.x - 1, root.index.y));
            var right = GetNode(new Int2(root.index.x + 1, root.index.y));
            var top = GetNode(new Int2(root.index.x, root.index.y + 1));
            var bottom = GetNode(new Int2(root.index.x, root.index.y - 1));

            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);
            if (top != null) neighbors.Add(top);
            if (bottom != null) neighbors.Add(bottom);

            if (allowDiagonalMove)
            {
                var topLeft = GetNode(new Int2(root.index.x - 1, root.index.y + 1));
                var topRight = GetNode(new Int2(root.index.x + 1, root.index.y + 1));
                var bottomLeft = GetNode(new Int2(root.index.x - 1, root.index.y - 1));
                var bottomRight = GetNode(new Int2(root.index.x + 1, root.index.y - 1));

                if (topLeft != null) neighbors.Add(topLeft);
                if (topRight != null) neighbors.Add(topRight);
                if (bottomLeft != null) neighbors.Add(bottomLeft);
                if (bottomRight != null) neighbors.Add(bottomRight);
            }

            return ( neighbors, Time.deltaTime );
        }

        public void UpdateCache()
        {
            lastCacheUpdate = Time.deltaTime;
        }

        public void AddNode(NavigationNode node)
        {
            if (nodes == null) nodes = new Dictionary<Int2, NavigationNode>();

            var index = Math.CalculateIndexFromPosition(nodeSize, node.transform.position);
            node.index = index;

            Debug.Log(index);

            if (nodes.ContainsKey(node.index))
            {
                nodes[node.index] = node;
            }
            else
            {
                nodes.Add(node.index, node);
            }

            UpdateCache();
        }

        public void RemoveNode(NavigationNode node)
        {
            if (nodes == null) nodes = new Dictionary<Int2, NavigationNode>();

            nodes.Remove(node.index);

            UpdateCache();
        }
    }
}
