using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Movement;

namespace USComics_Environment
{
    [System.Serializable]
    public enum LayerValues
    {
        DEFAULT = 0,
        TRANSPARENT_FX = 1,
        IGNORE_RAYCAST = 2,
        WATER = 4,
        UI = 5,
        SKYBOX = 8,
        PLAYER = 9,
        POSTPROCESSING = 10,
        SET_DRESSING = 11,
        VEHICLES = 14,
        NAVMESH = 15,
        ENVIRONMENT = 16,
        CHARACTERS = 17,
        VEGETATION = 18,
        EFFECTS = 19,
        RAGDOLL = 20,
        WATER_GEOMETRY = 21,
        TERRAIN = 22,
        ENEMY = 23,
        CAMERAS = 24,
        PROPS = 25,
        AVOID = 26,
        CAMERA_COLLIDER = 27,
        COLLIDER = 28,
        CHECKPOINT = 29,
        DO_NOT_DRAW = 30,
        LEVEL = 31
    }

    [System.Serializable]
    public class LayerMaskValues
    {
        public const int ALL = -1;
        public static int DEFAULT = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.DEFAULT));
        public static int TRANSPARENT_FX = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.TRANSPARENT_FX));
        public static int IGNORE_RAYCAST = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.IGNORE_RAYCAST));
        public static int WATER = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.WATER));
        public static int UI = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.UI));
        public static int SKYBOX = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.SKYBOX));
        public static int PLAYER = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.PLAYER));
        public static int POSTPROCESSING = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.POSTPROCESSING));
        public static int SET_DRESSING = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.SET_DRESSING));
        public static int VEHICLES = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.VEHICLES));
        public static int NAVMESH = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.NAVMESH));
        public static int ENVIRONMENT = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.ENVIRONMENT));
        public static int CHARACTERS = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.CHARACTERS));
        public static int VEGETATION = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.VEGETATION));
        public static int EFFECTS = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.EFFECTS));
        public static int RAGDOLL = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.RAGDOLL));
        public static int WATER_GEOMETRY = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.WATER_GEOMETRY));
        public static int TERRAIN = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.TERRAIN));
        public static int ENEMY = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.ENEMY));
        public static int CAMERAS = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.CAMERAS));
        public static int PROPS = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.PROPS));
        public static int AVOID = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.AVOID));
        public static int CAMERA_COLLIDER = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.CAMERA_COLLIDER));
        public static int COLLIDER = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.COLLIDER));
        public static int CHECKPOINT = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.CHECKPOINT));
        public static int DO_NOT_DRAW = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.DO_NOT_DRAW));
        public static int LEVEL = LayerMask.GetMask(LayerMask.LayerToName((int)LayerValues.LEVEL));
    }

    public class Environment
    {
        public static Collider[] GetClimbables(Transform transform)
        {
            Collider[] colliders = Direction.GetObjectsInRadius(transform.position, 1.0f, "Climbable", LayerMaskValues.LEVEL);
            List<Collider> results = new List<Collider>();
            for (int loop = 0; loop < colliders.Length; loop++)
            {
                GameObject candidate = Direction.GetGameObject(colliders[loop]);
                if ((0 < Direction.GetForwardOrBehind(transform, candidate.transform))
                && (Direction.AreParallel(transform.forward, candidate.transform.forward, 5.0f)))
                {
                    results.Add(colliders[loop]);
                }
            }
            return results.ToArray();
        }

        public static Collider[] GetEnemiesInSight(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            return GetEntitiesInSight(LayerMaskValues.ENEMY, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetEnemiesInRange(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            return GetEntitiesInRange(LayerMaskValues.ENEMY, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetPlayersInSight(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            return GetEntitiesInSight(LayerMaskValues.PLAYER, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetPlayersInRange(Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            return GetEntitiesInRange(LayerMaskValues.PLAYER, transform, radius, degrees, heightOffset, maxHeightDifference,useHeightDifference);
        }

        public static Collider[] GetEntitiesInSight(int mask, Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            Collider[] colliders = Direction.GetObjectsInRadius(transform.position, radius, mask);
            List<Collider> results = new List<Collider>();
            float halfDegrees = degrees / 2.0f;
            for (int loop1 = 0; loop1 < colliders.Length; loop1++)
            {
                GameObject candidate = Direction.GetGameObject(colliders[loop1]);
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

        public static Collider[] GetEntitiesInRange(int mask, Transform transform, float radius, float degrees, float heightOffset, float maxHeightDifference, bool useHeightDifference = true)
        {
            Collider[] colliders = Environment.GetEntitiesInSight(mask, transform, radius, degrees, heightOffset, maxHeightDifference, useHeightDifference);
            List<Collider> results = new List<Collider>();
            float halfDegrees = degrees / 2.0f;
            for (int loop = 0; loop < colliders.Length; loop++)
            {
                GameObject candidate = Direction.GetGameObject(colliders[loop]);
                EntityController entity = candidate.GetComponent<EntityController>();
                if (null == entity || entity.dead) continue;
                if (0 < Direction.GetForwardOrBehind(transform, candidate.transform))
                {
                    results.Add(colliders[loop]);
                }
            }
            return results.ToArray();
        }
    }
}
