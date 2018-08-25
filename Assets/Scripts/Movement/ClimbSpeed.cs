namespace USComics_Movement {
    [System.Serializable]
    public class ClimbSpeed {
        public const float CLIMB_SPEED = 1.0f;
        public const float IDLE_SPEED = 0.0f;
        public static float GetSpeed(ClimbType climbType) {
            if (ClimbType.Climbing == climbType) return ClimbSpeed.CLIMB_SPEED;
            if (ClimbType.Idle == climbType) return ClimbSpeed.IDLE_SPEED;
            return 0.0f;
        }
    }
}
