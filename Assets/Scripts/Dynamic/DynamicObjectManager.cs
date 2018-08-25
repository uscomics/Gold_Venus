using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Dynamic {
    public class DynamicObjectManager : MonoBehaviour {
        private List<GameObject> dynamicallyAllocaredObjects = new List<GameObject>();

        void OnDestroy() { DestroyDynamicObjects(); }
        public GameObject Clone(GameObject obj, Vector3 position, float angleX, float angleY, float angleZ) {
            GameObject clone = Instantiate(obj, position, Quaternion.Euler(angleX, angleY, angleZ)) as GameObject;
            dynamicallyAllocaredObjects.Add(clone);
            return clone;
        }
        public bool DestroyClone(GameObject clone) {
            for (int loop = dynamicallyAllocaredObjects.Count - 1; loop >= 0; loop--) {
                if (clone == dynamicallyAllocaredObjects[loop]) {
                    Destroy(dynamicallyAllocaredObjects[loop]);
                    dynamicallyAllocaredObjects.RemoveAt(loop);
                    return true;
                }
            }
            return false;
        }
        private void DestroyDynamicObjects()  {
            for (int loop = dynamicallyAllocaredObjects.Count - 1; loop >= 0; loop--) {
                if (null != dynamicallyAllocaredObjects[loop]) {
                    Destroy(dynamicallyAllocaredObjects[loop]);
                    dynamicallyAllocaredObjects.RemoveAt(loop);
                }
            }
        }
    }
}
