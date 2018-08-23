using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Movement;
using USComics_Environment;

namespace USComics_Movement {
    [System.Serializable]
    public enum DirectionType { None, North, East, South, West, NW, NE, SW, SE, Stop };

    public class GameObjectAndDistance {
        public GameObjectAndDistance(GameObject inGameObject, float inDistance) {
            gameObject = inGameObject;
            distance = inDistance;
        }
        public GameObject gameObject;
        public float distance;
    }

    public class DirectionUtilities {
        public static GameObject GetGameObject(Collider collider) { return collider.gameObject; }
        public static GameObject[] GetGameObjects(Collider[] colliders) {
            List<GameObject> results = new List<GameObject>();
            for (int loop = 0; loop < colliders.Length; loop++) results.Add(DirectionUtilities.GetGameObject(colliders[loop]));
            return results.ToArray();
        }
        public static Rigidbody[] GetRigidbodies(GameObject[] gameObjects) {
            List<Rigidbody> results = new List<Rigidbody>();
            foreach (var gameObject in gameObjects) {
                if (null == gameObject) continue;
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                if (null == rigidbody) continue;
                results.Add(rigidbody);
            }
            return results.ToArray();
        }
        public static Transform[] GetTransforms(Collider[] colliders) {
            List<Transform> results = new List<Transform>();
            for (int loop = 0; loop < colliders.Length; loop++) results.Add(DirectionUtilities.GetGameObject(colliders[loop]).transform);
            return results.ToArray();
        }
        public static Transform[] GetTransforms(GameObject[] objects) {
            List<Transform> results = new List<Transform>();
            for (int loop = 0; loop < objects.Length; loop++) results.Add(objects[loop].transform);
            return results.ToArray();
        }
        public static Collider[] GetObjectsInRadius(Vector3 center, float radius, int layerMask = LayerMaskValues.ALL, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            return Physics.OverlapSphere(center, radius, layerMask, queryTriggerInteraction);
        }
        public static Collider[] GetObjectsInRadius(Vector3 center, float radius, string tag, int layerMask = LayerMaskValues.ALL, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            Collider[] colliders = GetObjectsInRadius(center, radius, layerMask, queryTriggerInteraction);
            List<Collider> results = new List<Collider>();
            for (int loop = 0; loop < colliders.Length; loop++) {
                Collider collider = colliders[loop];
                if (collider.tag == tag) results.Add(collider);
            }
            return results.ToArray();
        }
        public static GameObject GetNearestObject(Vector3 position, GameObject[] objects) {
            float nearestDistanceSqr = Mathf.Infinity;
            GameObject nearestObj = null;
            for (int loop = 0; loop < objects.Length; loop++) {
                GameObject obj = objects[loop];
                Vector3 objectPosition = obj.transform.position;
                float distanceSqr = (objectPosition - position).sqrMagnitude;
                if (distanceSqr < nearestDistanceSqr) {
                    nearestObj = obj;
                    nearestDistanceSqr = distanceSqr;
                }
            }
            return nearestObj;
        }
        public static GameObjectAndDistance[] GetDistances(Transform transform, GameObject[] objects) {
            List<GameObjectAndDistance> ObjectsAndDistances = new List<GameObjectAndDistance>();
            for (int loop1 = 0; loop1 < objects.Length; loop1++) {
                GameObject obj = objects[loop1];
                float distance = DirectionUtilities.GetDistance(transform, obj.transform);
                GameObjectAndDistance gameObjectAndDistance = new GameObjectAndDistance(obj, distance);
                ObjectsAndDistances.Add(gameObjectAndDistance);
            }
            return ObjectsAndDistances.ToArray();
        }
        public static GameObjectAndDistance[] SortByDistance(Transform transform, GameObject[] objects) {
            GameObjectAndDistance[] results = DirectionUtilities.GetDistances(transform, objects);
            for (int loop = 0; loop < results.Length; loop++) {
                for (int loop2 = 0; loop2 < results.Length - 1; loop2++) {
                    if (results[loop2].distance > results[loop2 + 1].distance) {
                        GameObjectAndDistance temp = results[loop2 + 1];
                        results[loop2 + 1] = results[loop2];
                        results[loop2] = temp;
                    }
                }
            }
            return results;
        }
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
