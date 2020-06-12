using Sand.Navigation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationNode : MonoBehaviour
    {
        public NavigationGrid grid;
        public int moveCost;
        public bool walkable;

        [HideInInspector]
        public float gcost;
        [HideInInspector]
        public float hcost;
        [HideInInspector]
        public NavigationNode parentNodeInPath;

        [HideInInspector]
        public Int2 index;
        [HideInInspector]
        public List<NavigationNode> cachedNeighbors;
        [HideInInspector]
        public float lastNeighborUpdate;

        public float FCost { get { return hcost + gcost; } }

        public void SetGrid(NavigationGrid grid)
        {
            this.grid = grid;
        }

        protected void OnEnable()
        {
            if (grid == null) return;

            grid.AddNode(this);
        }

        protected void OnDisable()
        {
            if (grid == null) return;

            grid.RemoveNode(this);
        }

        public List<NavigationNode> GetNeighbors() 
        {
            var result = grid.GetNeighbors(this, lastNeighborUpdate);
            
            lastNeighborUpdate = result.cache;
            cachedNeighbors = result.neighbors;

            return cachedNeighbors;
        }
    }
}
