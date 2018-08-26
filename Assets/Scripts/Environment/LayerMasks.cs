using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;
using USComics_Movement;

namespace USComics_Environment
{
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
}
