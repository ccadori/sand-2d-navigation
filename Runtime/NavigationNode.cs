using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationNode : MonoBehaviour, INode
    {
        [SerializeField]
        protected NavigationGrid grid;
        [SerializeField]
        protected int moveCost;
        [SerializeField]
        protected bool walkable;

        public Int2 Index { get; set; }
        public Vector2 WorldPosition { get => transform.position; }
        public int MoveCost { get { return moveCost; } set { moveCost = value; } }
        public bool Walkable { get { return walkable; } set { walkable = value; } }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return HCost + GCost; } }
        public INode ParentNodeInPath { get; set; }
        public List<INode> CachedNeighbors { get; set; }
        public float LastNeighborUpdate { get; set; }

        protected virtual void Start()
        {
            grid.AddNode(this as INode);
            Index = grid.GetIndex(transform.position);
        }
    }
}
