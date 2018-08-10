using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Vision
{
    public class LayerMaskValues {
        public static int MASK_ALL = -1;
        public static int MASK_DEFAULT = LayerMask.GetMask(LayerMask.LayerToName(0));
        public static int MASK_TRANSPARENT_FX = LayerMask.GetMask(LayerMask.LayerToName(1));
        public static int MASK_IGNORE_RAYCAST = LayerMask.GetMask(LayerMask.LayerToName(2));
        public static int MASK_WATER = LayerMask.GetMask(LayerMask.LayerToName(4));
        public static int MASK_UI = LayerMask.GetMask(LayerMask.LayerToName(5));
        public static int MASK_SKYBOX = LayerMask.GetMask(LayerMask.LayerToName(8));
        public static int MASK_PLAYER = LayerMask.GetMask(LayerMask.LayerToName(9));
        public static int MASK_POSTPROCESSING = LayerMask.GetMask(LayerMask.LayerToName(10));
        public static int MASK_SET_DRESSING = LayerMask.GetMask(LayerMask.LayerToName(11));
        public static int MASK_VEHICLES = LayerMask.GetMask(LayerMask.LayerToName(14));
        public static int MASK_NAVMESH = LayerMask.GetMask(LayerMask.LayerToName(15));
        public static int MASK_ENVIRONMENT = LayerMask.GetMask(LayerMask.LayerToName(16));
        public static int MASK_CHARACTERS = LayerMask.GetMask(LayerMask.LayerToName(17));
        public static int MASK_VEGETATION = LayerMask.GetMask(LayerMask.LayerToName(18));
        public static int MASK_EFFECTS = LayerMask.GetMask(LayerMask.LayerToName(19));
        public static int MASK_RAGDOLL = LayerMask.GetMask(LayerMask.LayerToName(20));
        public static int MASK_WATER_GEOMETRY = LayerMask.GetMask(LayerMask.LayerToName(21));
        public static int MASK_TERRAIN = LayerMask.GetMask(LayerMask.LayerToName(22));
        public static int MASK_ENEMIES = LayerMask.GetMask(LayerMask.LayerToName(23));
        public static int MASK_CAMERAS = LayerMask.GetMask(LayerMask.LayerToName(24));
        public static int MASK_PROPS = LayerMask.GetMask(LayerMask.LayerToName(25));
        public static int MASK_AVOID = LayerMask.GetMask(LayerMask.LayerToName(26));
        public static int MASK_CAMERA_COLLIDER = LayerMask.GetMask(LayerMask.LayerToName(27));
        public static int MASK_COLLIDER = LayerMask.GetMask(LayerMask.LayerToName(28));
        public static int MASK_CHECKPOINT = LayerMask.GetMask(LayerMask.LayerToName(29));
        public static int MASK_DO_NOT_DRAW = LayerMask.GetMask(LayerMask.LayerToName(30));
        public static int MASK_LEVEL = LayerMask.GetMask(LayerMask.LayerToName(31));
    }

    public class LayerValues
    {
        public static int DEFAULT = 0;
        public static int TRANSPARENT_FX = 1;
        public static int IGNORE_RAYCAST = 2;
        public static int WATER = 4;
        public static int UI = 5;
        public static int SKYBOX = 8;
        public static int PLAYER = 9;
        public static int POSTPROCESSING = 10;
        public static int SET_DRESSING = 11;
        public static int VEHICLES = 14;
        public static int NAVMESH = 15;
        public static int ENVIRONMENT = 16;
        public static int CHARACTERS = 17;
        public static int VEGETATION = 18;
        public static int EFFECTS = 19;
        public static int RAGDOLL = 20;
        public static int WATER_GEOMETRY = 21;
        public static int TERRAIN = 22;
        public static int ENEMIES = 23;
        public static int CAMERAS = 24;
        public static int PROPS = 25;
        public static int AVOID = 26;
        public static int CAMERA_COLLIDER = 27;
        public static int COLLIDER = 28;
        public static int CHECKPOINT = 29;
        public static int DO_NOT_DRAW = 30;
        public static int LEVEL = 31;
    }

    public class Vision {
        public static Collider[] GetAllObjectsWithinRadius(Vector3 center, float radius, int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
            return Physics.OverlapSphere(center, radius, layerMask, queryTriggerInteraction);
        }
    }
}
