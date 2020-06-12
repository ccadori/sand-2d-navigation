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

        static public List<NavigationNode> GetVertices(List<NavigationNode> path)
        {
            List<NavigationNode> vertices = new List<NavigationNode>();

            for (int i = 0; i < path.Count; i++)
            {
                if (i == 0 || i == (path.Count - 1))
                {
                    vertices.Add(path[i]);
                    continue;
                }
                 
                Int2 result = ((path[i].index - path[i + 1].index) + (path[i].index - path[i - 1].index));

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
        
        static public List<NavigationNode> GetPath(NavigationNode start, NavigationNode target, NavigationGrid grid)
        {
            List<NavigationNode> openSet = new List<NavigationNode>();
            HashSet<NavigationNode> closedSet = new HashSet<NavigationNode>();
            NavigationNode currentNode;

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                currentNode = openSet[0];

                for (int count = 0; count < openSet.Count; count++)
                {
                    if (openSet[count].FCost < currentNode.FCost ||
                        openSet[count].FCost == currentNode.FCost &&
                        openSet[count].hcost < currentNode.hcost)
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

                foreach (NavigationNode neighbor in currentNode.GetNeighbors())
                {
                    if (!neighbor.walkable)
                        continue;

                    if (neighbor == null)
                        continue;

                    if (closedSet.Contains(neighbor))
                        continue;

                    if (grid.IsNodeOccupied(neighbor))
                        continue;

                    float costToNeighbour = currentNode.gcost + neighbor.moveCost + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                    if (costToNeighbour < neighbor.gcost || !openSet.Contains(neighbor))
                    {
                        neighbor.gcost = costToNeighbour;
                        neighbor.hcost = Vector2.Distance(currentNode.transform.position, neighbor.transform.position);
                        neighbor.parentNodeInPath = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }
        
        static private List<NavigationNode> TracePath(NavigationNode start, NavigationNode target)
        {
            List<NavigationNode> path = new List<NavigationNode>();
            NavigationNode currentNode = target;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parentNodeInPath;
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}
