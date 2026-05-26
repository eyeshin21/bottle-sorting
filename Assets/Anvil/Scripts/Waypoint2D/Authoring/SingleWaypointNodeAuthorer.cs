using System;
using UnityEngine;

namespace Anvil
{
    [ExecuteInEditMode]
    public class SingleWaypointNodeAuthorer : MonoBehaviour
    {
        // [SerializeField]
        private SingleWaypointNode node;
        public SingleWaypointNode Node
        {
            get
            {
                return node;
            }
        }
        public Vector3 WaypointPosition
        {
            get
            {
                if (node != null && node.Waypoint != null)
                {
                    return node.Waypoint.position;
                }
                return transform.position;
            }
        }

        [SerializeField] private SingleWaypointNodeAuthorer connection0;
        [SerializeField] private SingleWaypointNodeAuthorer connection1;

        private void Awake()
        {
        }

        private void Start()
        {
            // Initialize();
        }

        public void Initialize()
        {
            node = new SingleWaypointNode(transform.position);

        }

        public void InitializeConnection()
        {
            if (connection0 != null)
            {
                node.CreateSingleConnection(connection0.Node);
            }
            if (connection1 != null)
            {
                node.CreateSingleConnection(connection1.Node);
            }
        }

        private void OnDrawGizmos()
        {
            if (node != null)
            {
                node.DrawGizmos();
            }
        }
    }
}
