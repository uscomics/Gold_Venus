using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;

namespace USComics_Entity {
    public class EnemyController : EntityController {
        public static float HEARTBEAT_CHECK_FOR_PLAYERS = 1.0f;
        public static float HEARTBEAT_ATTACK_PLAYERS_DEFAULT = 1.0f;
        
        public EnemyCombatStance CombatStance = EnemyCombatStance.Passive;
        
        protected IEnumerator CheckForPlayers() {
            while (true) {
                yield return new WaitForSeconds(HEARTBEAT_CHECK_FOR_PLAYERS);
                if (EnemyCombatStance.Aggresive == CombatStance) {
                    if (null == CurrentEnemy) {
                        CurrentEnemy = GetTarget();
                        if (null != CurrentEnemy) CurrentEnemy.Targetted(this);
                    }
                    if (!IsInRange(CurrentEnemy) || CurrentEnemy.Dead) CurrentEnemy = null;
                }
            }
        }
        protected IEnumerator AttackPlayer() {
            while (true) {
                float heartbeat = HEARTBEAT_ATTACK_PLAYERS_DEFAULT;
                if (null != CurrentEnemy) {
                    Attack[] attacks = GetUsableAttacks(CurrentEnemy);
                    if (0 != attacks.Length) {
                        int attackIndex = Random.Range(0, attacks.Length);
                        Attack attack = attacks[attackIndex];
                        attack.DoAttack(CurrentEnemy);
                        heartbeat = attack.AttackInfo.Recharge + HEARTBEAT_ATTACK_PLAYERS_DEFAULT;
                    }
                }
                yield return new WaitForSeconds(heartbeat);
            }
        }
        protected EntityController GetTarget() {
            if (null == Vision) return null;
            Collider[] targets = Environment.GetEntitiesInSight(
                LayerMaskValues.PLAYER, 
                transform, 
                GetMaxAttackRange(), 
                Vision.DetectionAngle, 
                Vision.HeightOffset, 
                Vision.MaxHeightDifference, 
                true
            );
            GameObject[] targetsGO = GameObjectUtilities.GetGameObjects(targets);
            GameObject nearest = Environment.GetNearestObject(transform.position, targetsGO);
            if (null == nearest) return null;
            EntityController target = nearest.GetComponent<EntityController>();
            return target;
        }
        protected Attack[] GetUsableAttacks(EntityController target) {
            List<Attack> results = new List<Attack>();
            if (null == target || null == Attacks) return results.ToArray();
            float distance = Direction.GetDistance(transform, target.Entity.transform);
            foreach (var attack in Attacks) { if (attack.InRange(target.Entity)) results.Add(attack); }
            return results.ToArray();
        }
        protected override bool Setup() {
            if (!base.Setup()) return false;
            StartCoroutine(CheckForPlayers());
            StartCoroutine(AttackPlayer());
            return true;
        }
    }
}
