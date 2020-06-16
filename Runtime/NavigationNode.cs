using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationNode : MonoBehaviour
    {
        [SerializeField]
        protected NavigationGrid grid;
        [SerializeField]
        protected int moveCost;
        [SerializeField]
        protected bool walkable;

        public Int2 Index { get; set; }
        public NavigationGrid Grid { get { return grid; } private set { grid = value; } }
        public int MoveCost { get { return moveCost; } set { moveCost = value; } }
        public bool Walkable { get { return walkable; } set { walkable = value; } }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return HCost + GCost; } }
        public NavigationNode ParentNodeInPath { get; set; }
        public List<NavigationNode> CachedNeighbors { get; set; }
        public float LastNeighborUpdate { get; set; }

        protected virtual void Awake()
        {
            Index = grid.GetIndex(transform.position);
        }

        protected virtual void Start()
        {
            Grid.AddNode(this);
        }
    }
}
