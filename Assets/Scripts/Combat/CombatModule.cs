using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Message_Manager;

namespace USComics_Combat
{
    public class CombatModule
        : AbstractMovementModule
    {

        public AttackType CurrentAttack { get; set; }
        public AttackType PreviousAttack { get; set; }
        public GameObject PunchPointsObject;
        public GameObject KickPointsObject;
        public GameObject JumpkickPointsObject;
        public GameObject SuperPointsObject;
        public GameObject BonusPointsObject;
        public AudioClip PunchSound;
        public AudioClip KickSound;
        public AudioClip BlockSound;
        public AudioClip BlockBreakSound;
        public AudioClip BlockHitReactSound;
        public AudioClip JumpkickSound;
        public AudioClip SuperFXSound;
        public AudioClip SuperSound;
        public AudioClip KnockdownSound;
        public AudioClip Emote1Sound;
        public AudioClip Emote2Sound;
        public AudioClip Emote3Sound;
        public AudioClip Emote4Sound;
        public AudioClip Emote5Sound;
        public GameObject bamModel;
        public GameObject powModel;
        public GameObject kabamModel;
        public ParticleSystem[] superEffects;
        public Light[] superLights;
        public float damageBonus = 0.0f;

        private GameObject playerCharacter;
        private GameObject healthPanel;
        private Animator Anim;
        private AudioSource audioSource;
        private MessageManager messageManagerScript;
        private GameObject combatPanel;
        private GameObject superBar;
        private CombatPad CombatPadScript;
        private Keyboard KeyboardScript;
        private DynamicObjectManager DynamicObjectManagerScript;
        private DebugConsole debugConsoleScript;
        private bool moduleActive;
        private int bonusChance = 3;
        private int bamBonusChance = 33;
        private int powBonusChance = 66;

        // Use this for initialization
        void Start()
        {
            playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null != playerCharacter) audioSource = playerCharacter.GetComponent<AudioSource>();
            if (null != playerCharacter) messageManagerScript = playerCharacter.GetComponent<MessageManager>();
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

            if (null == playerCharacter) { Debug.LogError("CombatModule.Start: playerCharacter is null."); }
            if (null == Anim) { Debug.LogError("CombatModule.Start: Anim is null."); }
            if (null == audioSource) { Debug.LogError("CombatModule.Start: audioSource is null."); }
            if (null == messageManagerScript) { Debug.LogError("CombatModule.Start: messageManagerScript is null."); }
            if (null == healthPanel) { Debug.LogError("CombatModule.Start: healthPanel is null."); }
            if (null == debugConsole) { Debug.LogError("CombatModule.Start: debugConsole is null."); }
            if (null == DynamicObjectManagerScript) { Debug.LogError("CombatModule.Start: DynamicObjectManagerScript is null."); }
            if (null == combatPanel) { Debug.LogError("CombatModule.Start: combatPanel is null."); }
            if (null == superBar) { Debug.LogError("CombatModule.Start: superBar is null."); }
            if (null == CombatPadScript) { Debug.LogError("CombatModule.Start: CombatPadScript is null."); }
            if (null == KeyboardScript) { Debug.LogError("CombatModule.Start: KeyboardScript is null."); }

            if (null == playerCharacter) { return; }
            if (null == Anim) { return; }
            if (null == audioSource) { return; }
            if (null == messageManagerScript) { return; }
            if (null == healthPanel) { return; }
            if (null == debugConsole) { return; }
            if (null == DynamicObjectManagerScript) { return; }
            if (null == combatPanel) { return; }
            if (null == superBar) { return; }
            if (null == CombatPadScript) { return; }
            if (null == KeyboardScript) { return; }

            moduleActive = false;
        }

        // Update is called once per frame
        void Update()
        {
            damageBonus = 0.0f;
            if (!moduleActive) return;
            AttackType attack = GetAttack();
            UpdateAnimation(attack);
            SetAttack(attack);
            if (!moduleActive)
            {
                combatPanel.SetActive(false);
                superBar.SetActive(false);
            }
        }

        public override ModuleTypes ModuleType() { return ModuleTypes.Combat; }
        public override string GetTransitionToAnimationName() { return "AttackStart"; }
        public override string GetTransitionFromAnimationName() { return "AttackStop"; }

        public override void StartModule()
        {
            moduleActive = true;
            if (!combatPanel.activeSelf) combatPanel.SetActive(true);
            if (!superBar.activeSelf) superBar.SetActive(true);
        }

        public override bool IsRunning()
        {
            return moduleActive;
        }

        public override void StopModule()
        {
            moduleActive = false;
            if (combatPanel.activeSelf) combatPanel.SetActive(false);
            if (!superBar.activeSelf) superBar.SetActive(false);
        }

        public void ForceStop()
        {
            BufferedDirection stop = new BufferedDirection(DirectionType.Stop);
            KeyboardScript.directionBuffer.queue.Enqueue(stop);
        }

        private AttackType GetAttack()
        {
            AttackType attack = CombatPadScript.CurrentAttack;
            return attack;
        }

        private void SetAttack(AttackType attack)
        {
            PreviousAttack = CurrentAttack;
            CurrentAttack = attack;
            UpdateAnimation(attack);
        }

        private void UpdateAnimation(AttackType attack)
        {
            if (AttackType.Punch == attack) Punching();
            else if (AttackType.Kick == attack) Kicking();
            else if (AttackType.Jumpkick == attack) Jumpkicking();
            else if (AttackType.Block == attack) Blocking();
            else if (AttackType.BlockBreak == attack) BlockBreak();
            else if (AttackType.BlockHitReact == attack) BlockHitReact();
            else if (AttackType.Knockdown == attack) Knockdown();
            else if (AttackType.Super == attack) Super();
        }

        private void Punching()
        {
            PlayAttackSound(PunchSound);
            int punch = Random.Range(1, 5);
            if (1 == punch) Anim.Play("PunchRightUppercut");
            else if (2 == punch) Anim.Play("PunchLeftCross");
            else if (3 == punch) Anim.Play("PunchRightDownward");
            else if (4 == punch) Anim.Play("PunchLeftUppercut");
            CalculateBonus();
            DynamicObjectManagerScript.Clone(PunchPointsObject, healthPanel.transform.position, 0.0f, 180.0f, 0.0f);
    }

    private void Kicking()
        {
            PlayAttackSound(KickSound);
            Anim.Play("KickForwardRoundhouse");
            CalculateBonus();
            DynamicObjectManagerScript.Clone(KickPointsObject, healthPanel.transform.position, 0.0f, 180.0f, 0.0f);
    }

    private void Jumpkicking()
        {
            PlayAttackSound(JumpkickSound);
            Anim.Play("KickRoundhouse");
            CalculateBonus();
            DynamicObjectManagerScript.Clone(JumpkickPointsObject, healthPanel.transform.position, 0.0f, 180.0f, 0.0f);
    }

    private void Blocking()
        {
            PlayAttackSound(BlockSound);
            Anim.Play("Block");
            audioSource.PlayOneShot(BlockSound);
        }

        private void BlockBreak()
        {
            PlayAttackSound(BlockBreakSound);
        }

        private void BlockHitReact()
        {
            PlayAttackSound(BlockHitReactSound);
        }

        private void Knockdown()
        {
            PlayAttackSound(KnockdownSound);
        }

        private void Super()
        {
            audioSource.PlayOneShot(SuperFXSound);
            for (int loop1 = 0; loop1 < superEffects.Length; loop1++)
            {
                ParticleSystem particleSystem = superEffects[loop1];
                particleSystem.Play();
            }
            for (int loop2 = 0; loop2 < superLights.Length; loop2++)
            {
                Light light = superLights[loop2];
                light.enabled = true;
            }
            PlayAttackSound(SuperSound);
            Anim.Play("LeapingPunch");
            DynamicObjectManagerScript.Clone(SuperPointsObject, healthPanel.transform.position, 0.0f, 180.0f, 0.0f);
            CombatPadScript.ResetSuperBar();
    }

    private void PlayAttackSound(AudioClip attackSound)
        {
            int emote = Random.Range(1, 11);
            if (1 == emote) {
                int emoteChoice = Random.Range(1, 6);
                if (1 == emoteChoice) audioSource.PlayOneShot(Emote1Sound);
                else if (2 == emoteChoice) audioSource.PlayOneShot(Emote2Sound);
                else if (3 == emoteChoice) audioSource.PlayOneShot(Emote3Sound);
                else if (4 == emoteChoice) audioSource.PlayOneShot(Emote4Sound);
                else if (5 == emoteChoice) audioSource.PlayOneShot(Emote5Sound);
            }
            audioSource.PlayOneShot(attackSound);
        }

        private void CalculateBonus()
        {
            int bonus = Random.Range(1, 101);
            if (bonus > bonusChance) return;
            GameObject effect;
            bonus = Random.Range(1, 101);
            if (bonus <= bamBonusChance)
            {
                effect = bamModel;
                CombatPadScript.ClearKickTimer();
                CombatPadScript.ClearJumpkickTimer();
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_TIMERS_CLEARED);
            }
            else if (bonus <= powBonusChance)
            {
                effect = powModel;
                damageBonus = 2.0f;
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_DAMAGE_BONUS);
                DynamicObjectManagerScript.Clone(BonusPointsObject, healthPanel.transform.position, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                effect = kabamModel;
                CombatPadScript.IncrementSuperBar(5);
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_SUPER_BAR_BONUS);
            }
            effect = DynamicObjectManagerScript.Clone(effect, healthPanel.transform.position, 0.0f, 0.0f, 0.0f);
        }
    }

    [System.Serializable]
    public enum AttackType { Punch, Kick, Jumpkick, Block, BlockBreak, BlockHitReact, Knockdown, Super, None };

    [System.Serializable]
    public class AttackDamage
    {
        public const int PUNCH_DAMAGE = 1;
        public const int KICK_DAMAGE = 3;
        public const int JUMPKICK_DAMAGE = 10;
        public const int SUPER_DAMAGE = 30;
        public static int GetDamage(AttackType attackType)
        {
            if (AttackType.Punch == attackType) return AttackDamage.PUNCH_DAMAGE;
            if (AttackType.Kick == attackType) return AttackDamage.KICK_DAMAGE;
            if (AttackType.Jumpkick == attackType) return AttackDamage.JUMPKICK_DAMAGE;
            if (AttackType.Super == attackType) return AttackDamage.SUPER_DAMAGE;
            return 0;
        }
    }
}
