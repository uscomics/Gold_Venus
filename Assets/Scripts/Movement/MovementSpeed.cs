using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;

namespace USComics_Movement {
    [System.Serializable]
    public class MovementSpeed {
        public const float SNEAK_SPEED = 1.0f;
        public const float WALK_SPEED = 5.0f;
        public const float RUN_SPEED = 10.0f;
        public const float STANDING_SPEED = 0.0f;
        public static float GetSpeed(MovementType movementType) {
            if (MovementType.Sneaking == movementType) return MovementSpeed.SNEAK_SPEED;
            if (MovementType.Walking == movementType) return MovementSpeed.WALK_SPEED;
            if (MovementType.Running == movementType) return MovementSpeed.RUN_SPEED;
            if (MovementType.Standing == movementType) return MovementSpeed.STANDING_SPEED;
            return 0.0f;
        }
        public static MovementType GetMovementType(float speed) {
            if (MovementSpeed.SNEAK_SPEED > speed) return MovementType.Standing;
            if (MovementSpeed.WALK_SPEED > speed) return MovementType.Sneaking;
            if (MovementSpeed.RUN_SPEED > speed) return MovementType.Walking;
            return MovementType.Running;
        }
    }
}
