using System.Collections.Generic;
using UnityEngine;

namespace USComics {
    public class GameObjectUtilities {
        public static GameObject GetGameObject(Collider collider) { return collider.gameObject; }
        public static GameObject[] GetGameObjects(Collider[] colliders) {
            List<GameObject> results = new List<GameObject>();
            for (int loop = 0; loop < colliders.Length; loop++) results.Add(GameObjectUtilities.GetGameObject(colliders[loop]));
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
            for (int loop = 0; loop < colliders.Length; loop++) results.Add(GameObjectUtilities.GetGameObject(colliders[loop]).transform);
            return results.ToArray();
        }
        public static Transform[] GetTransforms(GameObject[] objects) {
            List<Transform> results = new List<Transform>();
            for (int loop = 0; loop < objects.Length; loop++) results.Add(objects[loop].transform);
            return results.ToArray();
        }
    }
}
