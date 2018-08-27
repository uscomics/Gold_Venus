using UnityEngine;

namespace USComics_Entity {
    [System.Serializable]
    public class Vision
    {
        public float HeightOffset = 0.0f;
        public float DetectionRadius = 10;
        [Range(0.0f, 360.0f)] 
        public float DetectionAngle = 360;    
        public float MaxHeightDifference = 4.0f;
    }
}



