using UnityEngine;
using System.Collections.Generic;
    
namespace Sand.Navigation.Utils
{
    public static class Math
    {
        static public float GetNearestFloat(float target, float mod)
        {
            int count = 0;
            float value = 0f;
            float absTarget = Mathf.Abs(target);

            while (value <= absTarget)
            {
                count++;
                value = mod * count;
            }

            float min = value - mod;
            float max = value;

            float nearest = ((absTarget - min) < (max - absTarget)) ? min : max;

            if (target < 0)
                nearest = -nearest;

            return nearest;
        }

        static public List<INode> GetVertices(List<INode> path)
        {
            List<INode> vertices = new List<INode>();

            for (int i = 0; i < path.Count; i++)
            {
                if (i == 0 || i == (path.Count - 1))
                {
                    vertices.Add(path[i]);
                    continue;
                }
                 
                Int2 result = ((path[i].Index - path[i + 1].Index) + (path[i].Index - path[i - 1].Index));

                if (!(result.x == 0) || !(result.y == 0))
                {
                    vertices.Add(path[i]);
                }
            }

            return vertices;
        }

        static public Int2 CalculateIndexFromPosition(Vector2 nodeSize, Vector2 position)
        {
            return new Int2(
                Mathf.RoundToInt(position.x / nodeSize.x),
                Mathf.RoundToInt(position.y / nodeSize.y)
            );
        }
        
        static public List<INode> GetPath(INode start, INode target, Api.ApiGrid grid)
        {
            List<INode> openSet = new List<INode>();
            HashSet<INode> closedSet = new HashSet<INode>();
            INode currentNode;

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                currentNode = openSet[0];

                for (int count = 0; count < openSet.Count; count++)
                {
                    if (openSet[count].FCost < currentNode.FCost ||
                        openSet[count].FCost == currentNode.FCost &&
                        openSet[count].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[count];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == target)
                {
                    return TracePath(start, target);
                }

                foreach (INode neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.Walkable)
                        continue;

                    if (neighbor == null)
                        continue;

                    if (closedSet.Contains(neighbor))
                        continue;

                    float costToNeighbour = currentNode.GCost + neighbor.MoveCost + Vector2.Distance(currentNode.WorldPosition, neighbor.WorldPosition);

                    if (costToNeighbour < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = costToNeighbour;
                        neighbor.HCost = Vector2.Distance(currentNode.WorldPosition, neighbor.WorldPosition);
                        neighbor.ParentNodeInPath = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            
            return null;
        }
        
        static private List<INode> TracePath(INode start, INode target)
        {
            List<INode> path = new List<INode>();
            INode currentNode = target;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.ParentNodeInPath;
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}
