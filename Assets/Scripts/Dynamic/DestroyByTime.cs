using UnityEngine;
using System.Collections;

namespace USComics_Dynamic
{
    public class DestroyByTime : MonoBehaviour
    {
        public float lifetime = 1.0f;
        private DynamicObjectManager DynamicObjectManagerScript;

        private float startTime;

        void Start()
        {
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
            if (null == DynamicObjectManagerScript) { Debug.LogError("DestroyByTime.Start: DynamicObjectManagerScript is null."); }
            if (null == DynamicObjectManagerScript) { return; }

            startTime = Time.time;
        } // Start

        void Update()
        {
            if (Time.time >= startTime + lifetime)
            {
                DynamicObjectManagerScript.DestroyClone(gameObject);
            }
        }

        public void ResetTime()
        {
            startTime = Time.time;
        }
    }
}
