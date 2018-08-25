using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement {
    [System.Serializable]
    public class Move {
        public Move(DirectionType directionType, float speed) {
            Direction = directionType;
            Speed = speed;
        }
        public DirectionType Direction { get; set; }
        public float Speed { get; set; }
    }
}
