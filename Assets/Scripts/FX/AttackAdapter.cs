using System.Collections;
using UnityEngine;
using USComics_Combat;
using USComics_Entity;
using USComics_Movement;
using USComics_Environment;

namespace USComics_FX {
    public class AttackAdapter : AbstractFX {
        public GameObject Attacker;
        public TargetDetectionType TargetDetectionType = TargetDetectionType.Player;
        public Attack Attack;

        private bool Playing;
      
        public override IEnumerator Play() {
            GameObject[] targets = GetEntitiesInRange();
            if (null != targets) {
                Playing = true;
                foreach (var target in targets) {
                    if (null != target) {
                        EntityController targetEntityController = target.GetComponent<EntityController>();
                        Attack.entity = Attacker;
                        if (null != targetEntityController) Attack.DoAttack(targetEntityController);
                    }
                }
                Playing = false;
            }
            yield break;
        }
        public override bool IsPlaying() { return Playing;}
        public override void Stop() { Attack.StopAll(); Playing = false; }
        public override AbstractFX CreateNew(GameObject parent) { return parent.AddComponent<AttackAdapter>(); }
        
        protected GameObject[] GetEntitiesInRange() {
            if (null == Attack) return null;
            if (null == Attacker) return null;
            EntityController AttackerEntityControllerScript = Attacker.GetComponent<EntityController>();
            if (null == AttackerEntityControllerScript) return null;
            int mask = 0;
            if (TargetDetectionType.Enemy == TargetDetectionType || TargetDetectionType.Both == TargetDetectionType) mask = LayerMaskValues.ENEMY;
            if (TargetDetectionType.Player == TargetDetectionType || TargetDetectionType.Both == TargetDetectionType) mask |= LayerMaskValues.PLAYER;
            Collider[] targets = Environment.GetEntitiesInSight(
                mask, 
                Attacker.transform, 
                Attack.AttackInfo.Range, 
                AttackerEntityControllerScript.vision.detectionAngle, 
                AttackerEntityControllerScript.vision.heightOffset, 
                AttackerEntityControllerScript.vision.maxHeightDifference, 
                true
            );
            GameObject[] targetsGO = GameObjectUtilities.GetGameObjects(targets);
            if (Attack.AttackInfo.Damage.IsAoE) return targetsGO;
            GameObject[] nearest = {Environment.GetNearestObject(Attacker.transform.position, targetsGO)};
            return nearest;
        }
    }
}
