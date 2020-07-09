using System.Collections.Generic;
using Sand.Navigation.Utils;
using UnityEngine;

namespace Sand.Navigation
{
    public interface INode
    {
        Int2 Index { get; set; }
        Vector2 WorldPosition { get; }
        int MoveCost { get; set; }
        bool Walkable { get; set; }
        float GCost { get; set; }
        float HCost { get; set; }
        float FCost { get; }
        INode ParentNodeInPath { get; set; }
        List<INode> CachedNeighbors { get; set; }
        float LastNeighborUpdate { get; set; }
    }
}
