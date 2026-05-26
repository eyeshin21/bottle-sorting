using Anvil;
using UnityEngine;

namespace Waypoint2D
{
    [System.Serializable]
    public class StaticPositionWaypointAuthorer : IWaypointAuthorer
    {
        [SerializeField] Vector3 _position;
        private Waypoint _waypoint;
        public Waypoint Waypoint
        {
            get
            {
                if (_waypoint == null)
                {
                    _waypoint = new Waypoint(_position);
                }
                return _waypoint;
            }
        }
    }
}
