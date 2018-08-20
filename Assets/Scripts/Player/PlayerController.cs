using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics;
using USComics_Debug;
using USComics_Combat;
using USComics_Movement;
using USComics_Environment;
using USComics_Message_Manager;

namespace USComics_Player
{
    public class PlayerController : EntityController
    {
        void Update()
        {
            UpdateCombat();
            if (initialUpdate)
            {
                simpleMovementScript.StartModule();
                initialUpdate = false;
            }
            if (simpleMovementScript.IsRunning())
            {
                AttackType currentAttack = combatModuleScript.CurrentAttack;

                debugConsoleScript.SetOther1("currentAttack=" + currentAttack);
                if (AttackType.None == currentAttack)
                {
                    Move currentMove = simpleMovementScript.CurrentMove;
                    Vector3 currentVector = simpleMovementScript.CurrentVector;
                    if (Vector3.zero != currentVector) entity.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVector), 0.15F);
                    entity.transform.Translate(currentVector * currentMove.Speed * Time.deltaTime, Space.World);
                    if (DirectionType.Stop != currentMove.Direction) SetHealthPosition(currentMove.Direction);
                    debugConsoleScript.SetCurrentMove(currentMove);
                    debugConsoleScript.SetOther1("currentVector=" + currentVector);
                }

            }
            else if (climbMovementScript.IsRunning())
            {
                ClimbMove currentMove = climbMovementScript.CurrentMove;
                Vector3 currentVector = climbMovementScript.CurrentVector;
                entity.transform.Translate(currentVector * ClimbSpeed.GetSpeed(currentMove.Climb) * Time.deltaTime, Space.World);
                debugConsoleScript.SetCurrentMove(currentMove);
                debugConsoleScript.SetOther1("currentVector=" + currentVector);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Move currentMove = simpleMovementScript.CurrentMove;
            if ("Climbable" == collision.gameObject.tag) climbableInRange = true;
            if (climbableInRange)
            {
                if (!climbMovementScript.IsRunning())
                {
                    Collider[] climbables = Environment.GetClimbables(entity.transform);
                    if (0 != climbables.Length)
                    {
                        movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Simple, ModuleTypes.Climbing);
                    }
                }
                else
                {
                    climbMovementScript.ForceStop();
                    movementTransitionManagerScript.StartTransitionFrom(ModuleTypes.Climbing, ModuleTypes.Simple);

                }
            } else
            {
                Debug.Log("BANG! tag = " + collision.gameObject.tag);
                if ((int)LayerValues.TERRAIN != collision.gameObject.layer) simpleMovementScript.ForceStop();
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            // Debug.Log("STILL BANG! tag = " + collision.gameObject.tag);
        }
        private void OnCollisionExit(Collision collision)
        {
            if ("Climbable" == collision.gameObject.tag) climbableInRange = false;
            // Debug.Log("BANG DONE! tag = " + collision.gameObject.tag);
        }

        private bool PlayerCanClimb()
        {
            Collider[] colliders = Environment.GetClimbables(entity.transform);
            return (0 < colliders.Length);
        }

        private void SetHealthPosition(DirectionType inDirection)
        {
            healthPanel.transform.localRotation = Quaternion.identity;
            if (DirectionType.South == inDirection) { healthPanel.transform.Rotate(Vector3.up, 180); }
            else if (DirectionType.East == inDirection) { healthPanel.transform.Rotate(Vector3.up, 270); }
            else if (DirectionType.West == inDirection) { healthPanel.transform.Rotate(Vector3.up, 90); }
            else if (DirectionType.NW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 45); }
            else if (DirectionType.NE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 315); }
            else if (DirectionType.SW == inDirection) { healthPanel.transform.Rotate(Vector3.up, 135); }
            else if (DirectionType.SE == inDirection) { healthPanel.transform.Rotate(Vector3.up, 225); }
        }

        private void UpdateCombat()
        {
            Collider[] enemies;
            if (null != CurrentEnemy && CurrentEnemyInRange)
            {
                enemies = GetEnemiesInRange(entity.transform);
                if (0 == enemies.Length) combatModuleScript.SetEnemyInRange(false);
                return;
            }
            enemies = GetEnemiesInSight(entity.transform);
            if (0 == enemies.Length)
            {
                combatModuleScript.StopModule();
                return;
            }
            combatModuleScript.StartModule();
            enemies = GetEnemiesInRange(entity.transform);
            bool enemiesInRange = (0 != enemies.Length);
            combatModuleScript.SetEnemyInRange(enemiesInRange);
            if (!enemiesInRange) return;
            simpleMovementScript.ForceStop();
            combatModuleScript.SetEnemyInRange(0 == enemies.Length);
            GameObject[] enemyGameObjects = DirectionUtilities.GetGameObjects(enemies);
            CurrentEnemy = DirectionUtilities.GetNearestObject(entity.transform.position, enemyGameObjects);
    }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            EditorGizmo(transform);
        }
#endif
    }
}
