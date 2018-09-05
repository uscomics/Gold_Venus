using UnityEngine;

namespace USComics_Movement {
    public class Direction {
        //  Returns a value between 0 and 180
        public static float GetAngle(Transform transform1, Transform transform2) { return Vector3.Angle(transform1.forward, transform2.position); }
        public static bool AreParallel(Vector3 vector1, Vector3 vector2, float threshold) { return threshold >= Vector3.Angle(vector1, vector2); }
        // Positive if left, negitive if right, zero if exactly even
        public static float GetLeftOrRight(Transform transform1, Transform transform2) { return Vector3.Dot(transform1.right, transform2.position); }
        // Positive if forward, negitive if behind, zero if exactly even
        public static float GetForwardOrBehind(Transform transform1, Transform transform2) { return Vector3.Dot(transform1.forward, transform2.position); }
        // The distance from transform1 to transform2
        // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
        public static float GetDistance(Transform transform1, Transform transform2) { return (transform2.position - transform1.position).magnitude; }
        // The direction from transform1 to transform2
        // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
        public static Vector3 GetVectorDirection(Transform transform1, Transform transform2) {
            Vector3 heading = transform2.position - transform1.position;
            return heading / heading.magnitude;
        }
        // The direction from transform1 to transform2
        public static DirectionType GetDirection(Transform transform1, Transform transform2) {
            float angle = GetAngle(transform1, transform2);
            float leftOrRight = GetLeftOrRight(transform1, transform2);

            if (0 < leftOrRight) {
                if (23 > angle) return DirectionType.North;
                if (68 > angle) return DirectionType.NE;
                if (113 > angle) return DirectionType.East;
                if (158 > angle) return DirectionType.SE;
                return DirectionType.South;
            } else if (0 > leftOrRight) {
                if (23 > angle) return DirectionType.North;
                if (68 > angle) return DirectionType.NW;
                if (113 > angle) return DirectionType.West;
                if (158 > angle) return DirectionType.SW;
                return DirectionType.South;
            } else {
                float forwardOrBehind = GetForwardOrBehind(transform1, transform2);
                if (0 < forwardOrBehind) return DirectionType.North;
                return DirectionType.South;
            }
        }
        public static Vector3 ConvertDirectionToVector(DirectionType direction, Vector3 previousVector) {
            if (DirectionType.Stop == direction) return Vector3.zero;
            if (DirectionType.North == direction) return Vector3.forward;
            if (DirectionType.South == direction) return -Vector3.forward;
            if (DirectionType.East == direction) return Vector3.right;
            if (DirectionType.West == direction) return -Vector3.right;
            if (DirectionType.NW == direction) return new Vector3(-0.66f, 0.0f, 0.66f);
            if (DirectionType.NE == direction) return new Vector3(0.66f, 0.0f, 0.66f);
            if (DirectionType.SW == direction) return new Vector3(-0.66f, 0.0f, -0.66f);
            if (DirectionType.SE == direction) return new Vector3(0.66f, 0.0f, -0.66f);
            if (DirectionType.None == direction) return previousVector;
            return Vector3.zero;
        }
    }
}
