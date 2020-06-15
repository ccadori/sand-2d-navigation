using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation

{
    public class NavigationAgent : MonoBehaviour
    {
        public NavigationGrid grid;
        public float velocity;

        protected Coroutine walkRoutine;

        public NavigationNode CurrentNode { get; set; }
        public NavigationNode MovingToNode { get; set; }
        public bool Moving { get; set; }
        protected bool Enabled { get; set; }

        private void Start()
        {
            grid.AddAgent(this);
            Teleport(grid.GetClosestNode(transform.position));
        }

        public virtual void Disable()
        {
            if (walkRoutine != null)
            {
                StopCoroutine(walkRoutine);
            }

            grid.RemoveAgent(this);
            Moving = false;
            MovingToNode = null;
        }

        public virtual void Teleport(NavigationNode node)
        {
            CurrentNode = node;
            transform.position = CurrentNode.transform.position;
        }

        public virtual void MoveTo(NavigationNode target)
        {
            if (target == null || CurrentNode == null || target == CurrentNode)
                return;

            var path = grid.GetPath(CurrentNode, target, this);
            
            if (walkRoutine != null)
                StopCoroutine(walkRoutine);

            walkRoutine = StartCoroutine(WalkRoutine(path));
        }

        public virtual void MoveTo(Vector2 position)
        {
            MoveTo(grid.GetNode(position));
        }

        protected virtual IEnumerator WalkRoutine(List<NavigationNode> path)
        {
            Moving = true;

            while (path != null && path.Count > 0 && true)
            {
                yield return new WaitForFixedUpdate();

                if (MovingToNode != null && (Vector2)transform.position != (Vector2)MovingToNode.transform.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, MovingToNode.transform.position, (velocity / 100));
                }
                else
                {
                    if (path[0] == MovingToNode)
                    {
                        path.RemoveAt(0);
                    }

                    if (path.Count > 0)
                    {
                        CurrentNode = path[0];
                        MovingToNode = path[0];
                    }
                }
            }

            MovingToNode = null;
            Moving = false;
        }
    }
}
