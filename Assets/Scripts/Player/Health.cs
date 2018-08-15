using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Player
{
    public class Health : MonoBehaviour
    {
        public static float MAX_HEALTH = 100;
        public static int MAX_LIVES = 6;

        public float health;
        public int lives;

        private GameObject healthBar;
        // private float initialHealthBarScale = 0.25f;
        private float initialHealthBarScale = 0.5f;
        private GameObject life1;
        private GameObject life2;
        private GameObject life3;
        private GameObject life4;
        private GameObject life5;
        private GameObject life6;

        // Use this for initialization
        void Start()
        {
            healthBar = GameObject.FindWithTag("HealthBar") as GameObject;
            life1 = GameObject.FindWithTag("Life1") as GameObject;
            life2 = GameObject.FindWithTag("Life2") as GameObject;
            life3 = GameObject.FindWithTag("Life3") as GameObject;
            life4 = GameObject.FindWithTag("Life4") as GameObject;
            life5 = GameObject.FindWithTag("Life5") as GameObject;
            life6 = GameObject.FindWithTag("Life6") as GameObject;

            if (null == healthBar) { Debug.LogError("Health.Start: healthBar is null."); }
            if (null == life1) { Debug.LogError("Health.Start: life1 is null."); }
            if (null == life2) { Debug.LogError("Health.Start: life2 is null."); }
            if (null == life3) { Debug.LogError("Health.Start: life3 is null."); }
            if (null == life4) { Debug.LogError("Health.Start: life4 is null."); }
            if (null == life5) { Debug.LogError("Health.Start: life5 is null."); }
            if (null == life6) { Debug.LogError("Health.Start: life6 is null."); }

            if (null == healthBar) { return; }
            if (null == life1) { return; }
            if (null == life2) { return; }
            if (null == life3) { return; }
            if (null == life4) { return; }
            if (null == life5) { return; }
            if (null == life6) { return; }

            health = Health.MAX_HEALTH;
            lives = 3;
        }

        // Update is called once per frame
        void Update()
        {
            float healthRemainingPrecentage = health / Health.MAX_HEALTH;
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

        void HideGameObject(GameObject gameObject) { gameObject.SetActive(false); }
        void ShowGameObject(GameObject gameObject) { gameObject.SetActive(true); }
    }
}
