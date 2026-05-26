using System;
using UnityEngine;

namespace Anvil
{
    [Serializable]
    public class WaypointConnector
    {
        public Waypoint waypoint0;
        public Waypoint waypoint1;

        public void SetNode(Waypoint node)
        {
            if (node == null)
            {
                Debug.LogError("SetConnectorNode: node is null");
            }

            if (waypoint0 == null)
            {
                waypoint0 = node;
            }
            else if (waypoint1 == null)
            {
                waypoint1 = node;
            }
            else
            {
                Debug.LogError("connectorinit: all node is already set");
            }
        }


        public bool Contain(Waypoint waypoint)
        {
            if (waypoint == waypoint1 || waypoint == waypoint0)
            {
                return true;
            }

            return false;
        }

        public bool Connected()
        {
            if (waypoint0 != null && waypoint1 != null)
            {
                return true;
            }

            return false;
        }
        public static WaypointConnector CreateConnector(Waypoint from, Waypoint to)
        {
            if (from == null || to == null)
            {
                return null;
            }

            WaypointConnector connector = new WaypointConnector();
            connector.waypoint0 = from;
            connector.waypoint1 = to;
            return connector;
        }
        public static WaypointConnector CreateConnector(Waypoint waypoint)
        {
            if (waypoint == null)
            {
                return null;
            }
            WaypointConnector connector = new WaypointConnector();
            connector.waypoint0 = waypoint;
            return connector;
        }
        //
        // public Waypoint GetConnection(Waypoint origin)
        // {
        //     if (origin == waypoint0)
        //     {
        //         return waypoint1;
        //     }
        //     else if (origin == waypoint1)
        //     {
        //         return waypoint0;
        //     }
        //
        //     return null;
        // }

        public void DrawGizmos()
        {
        }
    }
    public static partial class ExtensionMethods
    {

        public static Waypoint GetConnection(this WaypointConnector originConnector, Waypoint fromWp)
        {
            if (originConnector == null)
            {
                return null;
            }
            if (fromWp == originConnector.waypoint0)
            {
                return originConnector.waypoint1;
            }
            else if (fromWp == originConnector.waypoint1)
            {
                return originConnector.waypoint0;
            }

            return null;
        }
    }
}
