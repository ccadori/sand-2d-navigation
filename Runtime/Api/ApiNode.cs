using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation.Api 
{
    public class ApiNode : INode
    {
        public Int2 Index { get; set; }
        public Vector2 WorldPosition { get; private set; }
        public ApiGrid Grid { get; private set; }
        public int MoveCost { get; set; }
        public bool Walkable { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get; set; }
        public INode ParentNodeInPath { get; set; }
        public List<INode> CachedNeighbors { get; set; }
        public float LastNeighborUpdate { get; set; }

        public ApiNode(Int2 index, Vector2 worldPosition, Api.ApiGrid grid, int moveCost, bool walkable)
        {
            Index = index;
            WorldPosition = worldPosition;
            Grid = grid;
            MoveCost = moveCost;
            Walkable = walkable;
            GCost = 0;
            HCost = 0;
            FCost = 0;
            LastNeighborUpdate = -Mathf.Infinity;
        }
    }
}