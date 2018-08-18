using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Player;
using USComics_Movement;
using USComics_Environment;

namespace USComics_Combat
{
    [System.Serializable]
    public class TargetScanner
    {
        public float heightOffset = 0.0f;
        public float detectionRadius = 10;
        [Range(0.0f, 360.0f)]
        public float detectionAngle = 270;
        public float maxHeightDifference = 4.0f;
        public LayerMask viewBlockerLayerMask;

        public Collider[] GetEnemiesInSight(Transform player, bool useHeightDifference = true)
        {
            Collider[] enemies = Environment.GetEnemiesInSight(player, detectionRadius, detectionAngle, heightOffset, maxHeightDifference, useHeightDifference);
            return enemies;
        }

        public Collider[] GetEnemiesInRange(Transform player, bool useHeightDifference = true)
        {
            Collider[] enemies = Environment.GetEnemiesInRange(player, detectionRadius, detectionAngle, heightOffset, maxHeightDifference, useHeightDifference);
            return enemies;
        }


#if UNITY_EDITOR

        public void EditorGizmo(Transform transform)
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);

            UnityEditor.Handles.color = c;
            Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);

            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, 0.2f);
        }

#endif
    }

}