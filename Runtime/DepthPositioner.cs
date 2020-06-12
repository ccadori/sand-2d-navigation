using UnityEngine;

namespace Sand.Navigation
{
    [ExecuteInEditMode]
    public class DepthPositioner : MonoBehaviour
    {
        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y / 10f));
        }
    }
}
