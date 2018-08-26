using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Movement;

namespace USComics_Environment {
    public class Environment {
        public static Collider[] GetClimbables(Transform transform)  {
            Collider[] colliders = Environment.GetObjectsInRadius(transform.position, 1.0f, "Climbable", LayerMaskValues.LEVEL);
            List<Collider> results = new List<Collider>();
            for (int loop = 0; loop < colliders.Length; loop++) {
                GameObject candidate = GameObjectUtilities.GetGameObject(colliders[loop]);
                if ((0 < Direction.GetForwardOrBehind(transform, candidate.transform))
                && (Direction.AreParallel(transform.forward, candidate.transform.forward, 5.0f))) {
                    results.Add(colliders[loop]);
                }
            }
            return results.ToArray();
        }

        public static Collider[] GetEnemiesInSight(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true) {
            return GetEntitiesInSight(LayerMaskValues.ENEMY, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetEnemiesInFront(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true) {
            return GetEntitiesInFront(LayerMaskValues.ENEMY, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetPlayersInSight(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true) {
            return GetEntitiesInSight(LayerMaskValues.PLAYER, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetPlayersInFront(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true) {
            return GetEntitiesInFront(LayerMaskValues.PLAYER, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetEntitiesInSight(int mask, Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            Collider[] colliders = Environment.GetObjectsInRadius(transform.position, radius, mask);
            List<Collider> results = new List<Collider>();
            float halfDegrees = degrees / 2.0f;
            for (int loop1 = 0; loop1 < colliders.Length; loop1++) {
                GameObject candidate = GameObjectUtilities.GetGameObject(colliders[loop1]);
                if (halfDegrees <= Direction.GetAngle(transform, candidate.transform)) continue;
                Vector3 eyePos = candidate.transform.position + Vector3.up * heightOffset;
                Vector3 toPlayer = transform.position - eyePos;

                // If the enemy is too high or too low ignore him.
                if (useHeightDifference && Mathf.Abs(toPlayer.y + heightOffset) > maxHeightDifference) continue;
                EntityController entity = candidate.GetComponent<EntityController>();
                if (null == entity || entity.dead) continue;
                results.Add(colliders[loop1]);
            }
            return results.ToArray();
        }

        public static Collider[] GetEntitiesInFront(int mask, Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true) {
            Collider[] colliders = Environment.GetEntitiesInSight(mask, transform, radius, degrees, heightOffset, maxHeightDifference, useHeightDifference);
            List<Collider> results = new List<Collider>();
            float halfDegrees = degrees / 2.0f;
            for (int loop = 0; loop < colliders.Length; loop++) {
                GameObject candidate = GameObjectUtilities.GetGameObject(colliders[loop]);
                EntityController entity = candidate.GetComponent<EntityController>();
                if (null == entity || entity.dead) continue;
                if (0 < Direction.GetForwardOrBehind(transform, candidate.transform)) { results.Add(colliders[loop]); }
            }
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
                float distance = Direction.GetDistance(transform, obj.transform);
                GameObjectAndDistance gameObjectAndDistance = new GameObjectAndDistance(obj, distance);
                ObjectsAndDistances.Add(gameObjectAndDistance);
            }
            return ObjectsAndDistances.ToArray();
        }
        public static GameObjectAndDistance[] SortByDistance(Transform transform, GameObject[] objects) {
            GameObjectAndDistance[] results = Environment.GetDistances(transform, objects);
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
    }
}
