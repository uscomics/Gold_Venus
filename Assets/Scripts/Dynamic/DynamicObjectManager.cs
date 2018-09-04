using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Dynamic {
    public class DynamicObjectManager : MonoBehaviour {
        public static DynamicObjectManager INSTANCE;

        private List<GameObject> _dynamicallyAllocaredObjects = new List<GameObject>();

        void OnDestroy() { DestroyDynamicObjects(); }
        public GameObject Clone(GameObject obj, Vector3 position, float angleX, float angleY, float angleZ) {
            GameObject clone = Instantiate(obj, position, Quaternion.Euler(angleX, angleY, angleZ)) as GameObject;
            _dynamicallyAllocaredObjects.Add(clone);
            return clone;
        }
        public bool DestroyClone(GameObject clone) {
            for (int loop = _dynamicallyAllocaredObjects.Count - 1; loop >= 0; loop--) {
                if (clone == _dynamicallyAllocaredObjects[loop]) {
                    Destroy(_dynamicallyAllocaredObjects[loop]);
                    _dynamicallyAllocaredObjects.RemoveAt(loop);
                    return true;
                }
            }
            return false;
        }
        private void DestroyDynamicObjects()  {
            for (int loop = _dynamicallyAllocaredObjects.Count - 1; loop >= 0; loop--) {
                if (null != _dynamicallyAllocaredObjects[loop]) {
                    Destroy(_dynamicallyAllocaredObjects[loop]);
                    _dynamicallyAllocaredObjects.RemoveAt(loop);
                }
            }
        }
    }
}
