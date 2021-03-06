﻿using UnityEngine;
using USComics_Movement;

namespace USComics_Entity {
    public class Health : MonoBehaviour {
        public static int MAX_LIVES = 6;

        public GameObject HealthPanel;
        public bool HealthPanelVisible;
        public bool LivesVisible;
        public float HealthPoints;
        public int Lives;
        public EntityController Entity;
        public TextMesh EntityName;
        public GameObject HealthBar;
        public GameObject Life1;
        public GameObject Life2;
        public GameObject Life3;
        public GameObject Life4;
        public GameObject Life5;
        public GameObject Life6;
        public Camera Camera;

        private float _initialHealthBarScale = 0.5f;
        private float _maxHeath;

        void Start() {
            _maxHeath = HealthPoints;
            if (HealthPanelVisible) ShowHealth(); else HideHealth();
        }

        void Update() {
            float healthRemainingPrecentage = HealthPoints / _maxHeath;
            float healthBarPercentage = healthRemainingPrecentage * _initialHealthBarScale;
            Vector3 healthBarScale = HealthBar.transform.localScale;
            HealthBar.transform.localScale = new Vector3(healthBarScale.x, healthBarPercentage, healthBarScale.z);
            SetHealthPosition();

            if (null != EntityName && null != Entity) EntityName.text = Entity.Name;
            if (!LivesVisible) {
                HideGameObject(Life6);
                HideGameObject(Life5);
                HideGameObject(Life4);
                HideGameObject(Life3);
                HideGameObject(Life2);
                HideGameObject(Life1);
            } else {
                if (6 > Lives) HideGameObject(Life6); else ShowGameObject(Life6);
                if (5 > Lives) HideGameObject(Life5); else ShowGameObject(Life5);
                if (4 > Lives) HideGameObject(Life4); else ShowGameObject(Life4);
                if (3 > Lives) HideGameObject(Life3); else ShowGameObject(Life3);
                if (2 > Lives) HideGameObject(Life2); else ShowGameObject(Life2);
                if (1 > Lives) HideGameObject(Life1); else ShowGameObject(Life1);
            }
        }
        public void ShowHealth() {
            Renderer[] childComponents = HealthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = true; }
        }
        public void HideHealth() {
            Renderer[] childComponents = HealthPanel.GetComponentsInChildren<Renderer>();
            for (int loop = 0; loop < childComponents.Length; loop++) { childComponents[loop].enabled = false; }
        }
        public void AddHealth(float amount) {
            HealthPoints += amount;
            if (HealthPoints > _maxHeath) HealthPoints = _maxHeath;
        }
        public float GetMaxHealth() { return _maxHeath; }
        public void AddLife() {
            if (Lives >= Health.MAX_LIVES) return;
            Lives++;
        }
        public void SetHealthPosition() {
            HealthPanel.transform.LookAt(Camera.transform);
            HealthPanel.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        public void Spawn() {
            if (0 == Lives) return;
            Lives--;
            HealthPoints = _maxHeath;
            ShowHealth();
        }

        private void HideGameObject(GameObject gameObject) { gameObject.SetActive(false); }
        private void ShowGameObject(GameObject gameObject) { gameObject.SetActive(true); }
    }
}
