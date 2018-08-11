﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_User_Input;
using USComics_Environment;

namespace USComics_Vision
{
    [System.Serializable]
    public enum Direction { None, North, East, South, West, NW, NE, SW, SE, Stop };

    public class Vision {
        public static GameObject GetGameObject(Collider collider) {
            return collider.gameObject;
        }

        public static Collider[] GetObjectsInRadius(Vector3 center, float radius, int layerMask = LayerMaskValues.ALL, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            return Physics.OverlapSphere(center, radius, layerMask, queryTriggerInteraction);
        }

        public static Collider[] GetObjectsInRadius(Vector3 center, float radius, string tag, int layerMask = LayerMaskValues.ALL, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            Collider[] colliders = GetObjectsInRadius(center, radius, layerMask, queryTriggerInteraction);
            List<Collider> results = new List<Collider>();
            for (int loop = 0; loop < colliders.Length; loop++)
            {
                Collider collider = colliders[loop];
                if (collider.tag == tag) results.Add(collider);
            }
            return results.ToArray();
        }

        //  Returns a value between 0 and 180
        public static float GetAngle(Transform transform1, Transform transform2)
        {
            return Vector3.Angle(transform1.forward, transform2.position);
        }

        public static bool AreParallel(Vector3 vector1, Vector3 vector2, float threshold)
        {
            return threshold >= Vector3.Angle(vector1, vector2);
        }

        // Positive if left, negitive if right, zero if exactly even
        public static float GetLeftOrRight(Transform transform1, Transform transform2)
        {
            return Vector3.Dot(transform1.right, transform2.position);
        }

        // Positive if forward, negitive if behind, zero if exactly even
        public static float GetForwardOrBehind(Transform transform1, Transform transform2)
        {
            return Vector3.Dot(transform1.forward, transform2.position);
        }

        // The distance from transform1 to transform2
        // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
        public static float GetDistance(Transform transform1, Transform transform2) {
            return (transform2.position - transform1.position).magnitude;
        }

        // The direction from transform1 to transform2
        // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
        public static Vector3 GetVectorDirection(Transform transform1, Transform transform2) {
            Vector3 heading = transform2.position - transform1.position;
            return heading / heading.magnitude;
        }

        // The direction from transform1 to transform2
        public static Direction GetDirection(Transform transform1, Transform transform2)
        {
            float angle = GetAngle(transform1, transform2);
            float leftOrRight = GetLeftOrRight(transform1, transform2);

            if (0 < leftOrRight) {
                if (23 > angle) return Direction.North;
                if (68 > angle) return Direction.NE;
                if (113 > angle) return Direction.East;
                if (158 > angle) return Direction.SE;
                return Direction.South;
            }   
            else if (0 > leftOrRight){
                if (23 > angle) return Direction.North;
                if (68 > angle) return Direction.NW;
                if (113 > angle) return Direction.West;
                if (158 > angle) return Direction.SW;
                return Direction.South;
            }
            else {
                float forwardOrBehind = GetForwardOrBehind(transform1, transform2);
                if (0 < forwardOrBehind) return Direction.North;
                return Direction.South;
            }
        }

        public static Vector3 ConvertDirectionToVector(Direction direction, Vector3 previousVector)
        {
            if (Direction.Stop == direction) return Vector3.zero;
            if (Direction.North == direction) return Vector3.forward;
            if (Direction.South == direction) return -Vector3.forward;
            if (Direction.East == direction) return Vector3.right;
            if (Direction.West == direction) return -Vector3.right;
            if (Direction.NW == direction) return new Vector3(-0.66f, 0.0f, 0.66f);
            if (Direction.NE == direction) return new Vector3(0.66f, 0.0f, 0.66f);
            if (Direction.SW == direction) return new Vector3(-0.66f, 0.0f, -0.66f);
            if (Direction.SE == direction) return new Vector3(0.66f, 0.0f, -0.66f);
            if (Direction.None == direction) return previousVector;
            return Vector3.zero;
        }
    }
}
