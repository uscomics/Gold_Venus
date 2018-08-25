using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Movement;
using USComics_Environment;

namespace USComics_Movement {
    public class GameObjectAndDistance {
        public GameObjectAndDistance(GameObject inGameObject, float inDistance) {
            gameObject = inGameObject;
            distance = inDistance;
        }
        public GameObject gameObject;
        public float distance;
    }
}
