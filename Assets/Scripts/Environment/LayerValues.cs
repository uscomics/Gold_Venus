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
        SPAWN_POINT = 12,
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
}
