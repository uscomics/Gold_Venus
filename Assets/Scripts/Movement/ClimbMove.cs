using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement {
    [System.Serializable]
    public class ClimbMove
    {
        public ClimbMove(DirectionType directionType, ClimbType climbType)
        {
            Direction = directionType;
            Climb = climbType;
        }
        public DirectionType Direction { get; set; }
        public ClimbType Climb { get; set; }
    }
}
