using UnityEngine;
using USComics;
using USComics_Entity;
using USComics_Environment;

namespace USComics_FX {
    public abstract class AbstractTargetedFX : AbstractFX {
        public Transform Origin;
        public Vision Vision = new Vision();
        public bool IsAoE;
        public TargetDetectionType TargetDetectionType = TargetDetectionType.Player;

        protected GameObject[] GetTargets() {
            if (null == Origin || null == Vision) return null;
            int mask = 0;
            if (TargetDetectionType.Enemy == TargetDetectionType || TargetDetectionType.Both == TargetDetectionType) mask = LayerMaskValues.ENEMY;
            if (TargetDetectionType.Player == TargetDetectionType || TargetDetectionType.Both == TargetDetectionType) mask |= LayerMaskValues.PLAYER;
            Collider[] targets = Environment.GetEntitiesInSight(
                mask, 
                Origin, 
                Vision.DetectionRadius, 
                Vision.DetectionAngle, 
                Vision.HeightOffset, 
                Vision.MaxHeightDifference, 
                true
            );
            GameObject[] targetsGO = GameObjectUtilities.GetGameObjects(targets);
            if (IsAoE) return targetsGO;
            GameObject[] nearest = {Environment.GetNearestObject(Origin.position, targetsGO)};
            return nearest;
        }
    }
}
