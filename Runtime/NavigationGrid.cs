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
        public bool limitAgentsPerNode;
        public int agentsPerNode;

        public Api.ApiGrid Grid { get; protected set; }

        public void Awake()
        {
            Grid = new Api.ApiGrid(nodeSize, allowDiagonalMove, limitAgentsPerNode, agentsPerNode);
        }

        public List<INode> GetPath(INode start, INode target, IAgent agent) 
        {
            return Grid.GetPath(start, target, agent);
        }

        public INode GetClosestNode(Vector2 point)
        {
            return Grid.GetClosestNode(point);
        }

        public Int2 GetIndex(Vector2 position)
        {
            return Grid.GetIndex(position);
        }

        public INode GetNode(Int2 index)
        {
            return Grid.GetNode(index);
        }

        public INode GetNode(Vector2 position)
        {
            return Grid.GetNode(position);
        }

        public bool CanOccupy(INode node, IAgent agent)
        {
            return Grid.CanOccupy(node, agent);
        }

        public void AddNode(INode node)
        {
            Grid.AddNode(node);
            Grid.UpdateCache(Time.deltaTime);
        }

        public void RemoveNode(INode node)
        {
            RemoveNode(node);
            Grid.UpdateCache(Time.deltaTime);
        }

        public void AddAgent(IAgent agent) 
        {
            Grid.AddAgent(agent);
        }

        public void RemoveAgent(IAgent agent)
        {
            Grid.RemoveAgent(agent);
        }
    }
}
