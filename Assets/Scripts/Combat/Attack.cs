using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics;
using USComics_Debug;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Message_Manager;

namespace USComics_Combat
{
    public class Attack : MonoBehaviour
    {
        public AttackInfo attackInfo = new AttackInfo();
        public GameObject character;

        protected EntityController EntityControllerScript;
        protected GameObject healthPanel;
        protected Animator Anim;
        protected MessageManager messageManagerScript;
        protected GameObject combatPanel;
        protected GameObject superBar;
        protected CombatPad CombatPadScript;
        protected Keyboard KeyboardScript;
        protected DynamicObjectManager DynamicObjectManagerScript;
        protected DebugConsole debugConsoleScript;

        public Attack() { }
        public Attack(GameObject obj) { character = obj; }
        public Attack(GameObject obj, AttackInfo attack)
        {
            character = obj;
            attackInfo = attack;
        }

        // Use this for initialization
        void Start()
        {
            if (null != character) EntityControllerScript = character.GetComponent<EntityController>();
            if (null != character) Anim = character.GetComponent<Animator>();
            if (null != character) messageManagerScript = character.GetComponent<MessageManager>();
            healthPanel = GameObject.FindWithTag("HealthGameObject") as GameObject;
            GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
            if (null != debugConsole) debugConsoleScript = debugConsole.GetComponent<DebugConsole>();
            GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
            if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
            combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
            if (null != combatPanel) CombatPadScript = combatPanel.GetComponent<CombatPad>();
            superBar = GameObject.FindWithTag("SuperBar") as GameObject;
            GameObject movementPad = GameObject.FindWithTag("MovementPad") as GameObject;
            if (null != movementPad) KeyboardScript = movementPad.GetComponent<Keyboard>();

            if (null == character) { Debug.LogError("AbstractAttack.Start: character is null."); }
            if (null == EntityControllerScript) { Debug.LogError("AbstractAttack.Start: EntityControllerScript is null."); }
            if (null == Anim) { Debug.LogError("AbstractAttack.Start: Anim is null."); }
            if (null == messageManagerScript) { Debug.LogError("AbstractAttack.Start: messageManagerScript is null."); }
            if (null == healthPanel) { Debug.LogError("AbstractAttack.Start: healthPanel is null."); }
            if (null == debugConsole) { Debug.LogError("AbstractAttack.Start: debugConsole is null."); }
            if (null == DynamicObjectManagerScript) { Debug.LogError("AbstractAttack.Start: DynamicObjectManagerScript is null."); }
            if (null == combatPanel) { Debug.LogError("AbstractAttack.Start: combatPanel is null."); }
            if (null == superBar) { Debug.LogError("AbstractAttack.Start: superBar is null."); }
            if (null == CombatPadScript) { Debug.LogError("AbstractAttack.Start: CombatPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("AbstractAttack.Start: KeyboardScript is null."); }

            if (null == character) { return; }
            if (null == Anim) { return; }
            if (null == messageManagerScript) { return; }
            if (null == healthPanel) { return; }
            if (null == debugConsole) { return; }
            if (null == DynamicObjectManagerScript) { return; }
            if (null == combatPanel) { return; }
            if (null == superBar) { return; }
            if (null == CombatPadScript) { return; }
            if (null == KeyboardScript) { return; }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DoAttack()
        {

        }
        public bool IsCharged(GameObject obj) { return attackInfo.lastUsed + attackInfo.recharge <= Time.time; }
        public bool InRange(GameObject obj) { return DirectionUtilities.GetDistance(character.transform, obj.transform) <= attackInfo.range; }
        public bool AnyInRange(GameObject[] objs)
        {
            for (int loop = 0; loop < objs.Length; loop++)
            {
                if (InRange(objs[loop])) return true;
            }
            return false;
        }
        public GameObject[] AllInRange(GameObject[] objs)
        {
            List<GameObject> result = new List<GameObject>();
            for (int loop = 0; loop < objs.Length; loop++)
            {
                if (InRange(objs[loop])) result.Add(objs[loop]);
            }
            return result.ToArray();
        }
        public void ActivateGUI()
        {
            if (null == attackInfo.uiImage) return;
        }
        public void PlayAll()
        {
            PlaySounds();
            PlayAnimation();
            PlayParticleSystems();
            PlayLights();
        }
        public void StopAll()
        {
            StopSounds();
            StopParticleSystems();
            StopLights();
        }
        public void PlaySounds()
        {
            if (null == attackInfo.audioSource1) return;
            if (null == attackInfo.sound1) return;
            if (null == attackInfo.sound2)
            {
                attackInfo.audioSource1.PlayOneShot(attackInfo.sound1);
                return;
            }
            if (null == attackInfo.audioSource2)
            {
                Debug.LogError("Need two audio sources to play two audio clips.");
                return;
            }
            // https://docs.unity3d.com/ScriptReference/AudioSource.SetScheduledEndTime.html
            int len1 = attackInfo.sound1.samples;
            int len2 = attackInfo.sound2.samples;
            float overlap = 0.2F;
            double t0 = AudioSettings.dspTime + 3.0F;
            double clipTime1 = len1;
            clipTime1 /= attackInfo.sound1.frequency;
            attackInfo.audioSource1.PlayScheduled(t0);
            attackInfo.audioSource1.SetScheduledEndTime(t0 + clipTime1);
            Debug.Log("t0 = " + t0 + ", clipTime1 = " + clipTime1 + ", attackInfo.sound1.frequency = " + attackInfo.sound1.frequency);
            Debug.Log("attackInfo.sound2.frequency = " + attackInfo.sound2.frequency + ", samplerate = " + AudioSettings.outputSampleRate);
            attackInfo.audioSource2.PlayScheduled(t0 + clipTime1);
            attackInfo.audioSource2.time = overlap;
        }
        public void StopSounds()
        {
            if (null != attackInfo.audioSource1 && attackInfo.audioSource1.isPlaying) attackInfo.audioSource1.Stop();
            if (null != attackInfo.audioSource2 && attackInfo.audioSource2.isPlaying) attackInfo.audioSource2.Stop();
        }
        public void PlayAnimation()
        {
            int randomAniation = Random.Range(0, attackInfo.animationNames.Length);
            Anim.Play(attackInfo.animationNames[randomAniation]);
        }
        public void PlayParticleSystems()
        {
            for (int loop1 = 0; loop1 < attackInfo.particleSystems.Length; loop1++)
            {
                ParticleSystem particleSystem = attackInfo.particleSystems[loop1];
                particleSystem.Play();
            }
        }
        public void StopParticleSystems()
        {
            for (int loop1 = 0; loop1 < attackInfo.particleSystems.Length; loop1++)
            {
                ParticleSystem particleSystem = attackInfo.particleSystems[loop1];
                particleSystem.Stop();
            }
        }
        public void PlayLights()
        {
            for (int loop1 = 0; loop1 < attackInfo.lights.Length; loop1++)
            {
                Light light = attackInfo.lights[loop1];
                light.enabled = true;
            }
        }
        public void StopLights()
        {
            for (int loop1 = 0; loop1 < attackInfo.lights.Length; loop1++)
            {
                Light light = attackInfo.lights[loop1];
                light.enabled = false;
            }
        }
    }
}
