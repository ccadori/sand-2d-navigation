using Sand.Navigation.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sand.Navigation
{
    public class NavigationAgent : MonoBehaviour
    {
        public NavigationGrid grid;
        public float velocity;

        [HideInInspector]
        public NavigationNode currentNode;
        [HideInInspector]
        public NavigationNode movingToNode;
        [HideInInspector]
        public bool moving;

        protected Coroutine walkRoutine;

        private void OnEnable()
        {
            DefineInitialNode();
        }

        private void OnDisable()
        {
            if (walkRoutine != null)
            {
                StopCoroutine(walkRoutine);
                moving = false;
                movingToNode = null;
            }
        }

        public void SetGrid(NavigationGrid grid)
        {
            this.grid = grid;
        }

        public void DefineInitialNode()
        {
            currentNode = grid.GetClosestNode(transform.position);
            transform.position = currentNode.transform.position;
        }

        public void MoveTo(NavigationNode target)
        {
            if (target == null || currentNode == null || target == currentNode)
                return;

            var path = Math.Getpath(currentNode, target, this);

            if (walkRoutine != null)
                StopCoroutine(walkRoutine);

            walkRoutine = StartCoroutine(WalkRoutine(path));
        }

        public void MoveTo(Vector2 position)
        {
            MoveTo(grid.GetNode(position));
        }

        internal IEnumerator WalkRoutine(List<NavigationNode> path)
        {
            moving = true;
            movingToNode = path[0];

            while (path.Count > 0 && true)
            {
                yield return new WaitForFixedUpdate();

                if (movingToNode != null && (Vector2)transform.position != (Vector2)movingToNode.transform.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, movingToNode.transform.position, (velocity / 100));
                }
                else
                {
                    if (path[0] == movingToNode)
                    {
                        path.RemoveAt(0);
                    }

                    if (path.Count > 0)
                    {
                        currentNode = path[0];
                        movingToNode = path[0];
                    }
                }
            }

            movingToNode = null;
            moving = false;
        }
    }
}
