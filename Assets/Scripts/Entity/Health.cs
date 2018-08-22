using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Entity
{
    public class Health : MonoBehaviour
    {
        public static int MAX_LIVES = 6;

        public float health;
        public int lives;
        public GameObject healthBar;
        public GameObject life1;
        public GameObject life2;
        public GameObject life3;
        public GameObject life4;
        public GameObject life5;
        public GameObject life6;

        // private float initialHealthBarScale = 0.25f;
        private float initialHealthBarScale = 0.5f;
        private float maxHeath;

        // Use this for initialization
        void Start() { maxHeath = health; }

        // Update is called once per frame
        void Update() {
            float healthRemainingPrecentage = health / maxHeath;
            float healthBarPercentage = healthRemainingPrecentage * initialHealthBarScale;
            Vector3 healthBarScale = healthBar.transform.localScale;
            healthBar.transform.localScale = new Vector3(healthBarScale.x, healthBarPercentage, healthBarScale.z);

            if (6 > lives) HideGameObject(life6); else ShowGameObject(life6);
            if (5 > lives) HideGameObject(life5); else ShowGameObject(life5);
            if (4 > lives) HideGameObject(life4); else ShowGameObject(life4);
            if (3 > lives) HideGameObject(life3); else ShowGameObject(life3);
            if (2 > lives) HideGameObject(life2); else ShowGameObject(life2);
            if (1 > lives) HideGameObject(life1); else ShowGameObject(life1);
        }
        public void AddHealth(float amount) {
            health += amount;
            if (health > maxHeath) health = maxHeath;
        }
        public void AddLife() {
            if (lives == Health.MAX_LIVES) return;
            lives++;
        }
        private void HideGameObject(GameObject gameObject) { gameObject.SetActive(false); }
        private void ShowGameObject(GameObject gameObject) { gameObject.SetActive(true); }
    }
}
