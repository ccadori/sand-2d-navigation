using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationNode : MonoBehaviour
    {
        [SerializeField]
        private NavigationGrid grid;
        [SerializeField]
        private int moveCost;
        [SerializeField]
        private bool walkable;

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

        private void Awake()
        {
            Index = grid.GetIndex(transform.position);
        }

        private void Start()
        {
            Grid.AddNode(this);
        }
    }
}
