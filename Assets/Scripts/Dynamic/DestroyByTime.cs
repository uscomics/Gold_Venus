using UnityEngine;
using System.Collections;

namespace USComics_Dynamic
{
    public class DestroyByTime : MonoBehaviour
    {
        public float lifetime = 1.0f;

        private float startTime;

        void Start()
        {
            startTime = Time.time;
        } // Start

        void Update()
        {
            if (Time.time >= startTime + lifetime)
            {
                DynamicObjectManager.INSTANCE.DestroyClone(gameObject);
            }
        }

        public void ResetTime()
        {
            startTime = Time.time;
        }
    }
}
