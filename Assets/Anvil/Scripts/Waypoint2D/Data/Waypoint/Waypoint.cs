using System;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    [Serializable]
    public class Waypoint
    {
        public static int idIndexer = -1;

        public int id = -1;
        public Vector3 position;

        public Waypoint(Vector3 pos)
        {
            id = CreateId();
            position = pos;
        }

        public Waypoint(Vector3 pos, int Id)
        {
            if (Id < 0)
            {
                Id = CreateId();
            }

            this.id = Id;
            position = pos;
        }

        public static int CreateId()
        {
            //Debug.Log($"waypoint indexer {idIndexer}");
            int ret = idIndexer++;
            //idIndexer++;
            return ret;
        }

        public void DrawGizmos()
        {
            GizmosHelper.DrawCross(position, Color.yellow);
            GizmosHelper.DrawText(id.ToString(), (position + Vector3.right * 0.1f) + Vector3.up * 0.1f, Color.yellow,
                90);
        }
    }

    public static partial class ExtensionMethods
    {
        public static int TryGetId(this Waypoint wp)
        {
            if (wp == null)
            {
                return -1;
            }

            return wp.id;
        }
    }
}
