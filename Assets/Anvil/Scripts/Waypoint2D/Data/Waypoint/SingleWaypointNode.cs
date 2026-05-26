using Anvil.Legacy;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class SingleWaypointNode : IWaypointNode
    {
        private Waypoint waypoint;
        private WaypointConnector connector0;
        private WaypointConnector connector1;

        public Waypoint Waypoint => waypoint;

        public SingleWaypointNode(Waypoint wp)
        {
            waypoint = wp;
        }
        public SingleWaypointNode(Vector3 pos)
        {
            waypoint = new Waypoint(pos);
        }

        public bool CreateSingleConnection(IWaypointNode toNode)
        {
            if (toNode.ConnectionExist(connector1) || toNode.ConnectionExist(connector0))
            {
                Debug.LogWarning("connection already exist");
                return false;
            }
            WaypointConnector connector = WaypointConnector.CreateConnector(waypoint);
            bool connected = toNode.ValidateConnection(ref connector, this);
            if (connected)
            {
                AssignConnector(connector, toNode);
                return true;
            }

            return false;
        }

        public bool ValidateConnection(ref WaypointConnector commonConnector,IWaypointNode fromNode)
        {
            if (connector0 == null)
            {
                connector0 = commonConnector;
            }
            else if (connector1 == null)
            {
                connector1 = commonConnector;
            }
            else return false;

            commonConnector.SetNode(waypoint);
            return true;
        }

        public bool AssignConnector(WaypointConnector connector,IWaypointNode fromNode)
        {
            if (connector0 == null)
            {
                connector0 = connector;
            }
            else if (connector1 == null)
            {
                connector1 = connector;
            }
            else return false;

            return true;
        }

        public bool ConnectionExist(WaypointConnector connector)
        {
            if (connector == null)
            {
                return false;
            }
            return connector0 == connector || connector1 == connector;
        }

        public void DrawGizmos()
        {
            if (waypoint != null)
            {
                waypoint.DrawGizmos();
            }

            if (connector0 != null)
            {
                Waypoint target = connector0.GetConnection(waypoint);
                if (target != null)
                {
                    GizmosHelper.DrawLine(waypoint.position, target.position, Color.green);

                    Vector3 anotatedPos = waypoint.position + (target.position - waypoint.position).normalized * 0.3f;
                    anotatedPos = anotatedPos +
                                  Vector3.Cross((target.position - waypoint.position), Vector3.forward).normalized *
                                  0.15f;
                    GizmosHelper.DrawText($"c0 - {target.id}", anotatedPos, Color.green, 50);
                }
            }

            if (connector1 != null)
            {
                Waypoint target = connector1.GetConnection(waypoint);
                if (target != null)
                {
                    GizmosHelper.DrawLine(waypoint.position, target.position, Color.green);

                    Vector3 anotatedPos = waypoint.position + (target.position - waypoint.position).normalized * 0.3f;
                    anotatedPos = anotatedPos +
                                  Vector3.Cross((target.position - waypoint.position), Vector3.forward).normalized *
                                  0.15f;
                    GizmosHelper.DrawText($"c1 - {target.id}", anotatedPos, Color.green, 50);
                }
            }
            GizmosHelper.DrawCircle(waypoint.position, 1, color: Color.cyan);
        }
    }

    public interface IWaypointNode
    {
        public bool ValidateConnection(ref WaypointConnector commonConnector, IWaypointNode fromNode);
        public bool AssignConnector(WaypointConnector connector, IWaypointNode fromNode);
        public bool ConnectionExist(WaypointConnector connector);
    }


}
