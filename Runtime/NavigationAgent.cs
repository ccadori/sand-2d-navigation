using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation

{
    public class NavigationAgent : MonoBehaviour, IAgent
    {
        public NavigationGrid grid;
        public float velocity;

        protected Coroutine walkRoutine;

        public INode CurrentNode { get; set; }
        public INode OccupiedNode { get; set; }

        public float Velocity { get { return velocity; } set { velocity = value; } }
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
            CurrentNode = null;
        }

        public virtual void Teleport(INode node)
        {
            if (!grid.CanOccupy(node, this))
            {
                return;
            }

            OccupiedNode = node;
            CurrentNode = node;
            transform.position = CurrentNode.WorldPosition;
        }

        public virtual void MoveTo(INode target)
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

        protected virtual IEnumerator WalkRoutine(List<INode> path)
        {
            Moving = true;

            if (path != null && path.Count > 0)
            {
                OccupiedNode = path[path.Count - 1];
            }

            while (path != null && path.Count > 0)
            {
                yield return new WaitForFixedUpdate();

                if (CurrentNode != null && (Vector2)transform.position != (Vector2)CurrentNode.WorldPosition)
                {
                    transform.position = Vector2.MoveTowards(transform.position, CurrentNode.WorldPosition, (Velocity / 100));
                }
                else
                {
                    if (path[0] == CurrentNode)
                    {
                        path.RemoveAt(0);
                    }

                    if (path.Count > 0)
                    {
                      CurrentNode = path[0];
                    }
                }
            }

            Moving = false;
        }
    }
}
