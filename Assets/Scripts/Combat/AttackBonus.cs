using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Debug;
using USComics_Entity;
using USComics_Dynamic;
using USComics_Message_Manager;

namespace USComics_Combat {
    [System.Serializable]
    public class AttackBonus {
        public int BonusChance;
        public GameObject BAMModel;
        public GameObject POWModel;
        public GameObject KABAMModel;
        public GameObject BonusPoints;
        public int BAMBonusChance;
        public int POWBonusChance;

        private MessageManager messageManagerScript;
        private DynamicObjectManager DynamicObjectManagerScript;
        private CombatPad CombatPadScript;
        private DebugConsole DebugConsoleScript;
        private bool isSetup;

        public AttackBonus() { }
        public AttackBonus(AttackBonus from) {
            BonusChance = from.BonusChance;
            BAMModel = from.BAMModel;
            POWModel = from.POWModel;
            KABAMModel = from.KABAMModel;
            BonusPoints = from.BonusPoints;
            BAMBonusChance = from.BAMBonusChance;
            POWBonusChance = from.POWBonusChance;
            messageManagerScript = from.messageManagerScript;
            DynamicObjectManagerScript = from.DynamicObjectManagerScript;
            CombatPadScript = from.CombatPadScript;
            DebugConsoleScript = from.DebugConsoleScript;
        }
        public void CalculateBonus(Attack attack, EntityController attacker, EntityController target) {
            if (!isSetup) SetupAttackInfo();
            int bonus = Random.Range(1, 101);
            if (bonus > BonusChance) return;
            bonus = Random.Range(1, 101);
            if (bonus <= BAMBonusChance) {
                attacker.ClearAttackTimers();
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_TIMERS_CLEARED, 2);
                DynamicObjectManagerScript.Clone(BAMModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
            }
            else if (bonus <= POWBonusChance) {
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_DAMAGE_BONUS, 2);
                DynamicObjectManagerScript.Clone(POWModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
                DynamicObjectManagerScript.Clone(BonusPoints, target.transform.position + Vector3.up, 0.0f, 180.0f, 0.0f);
                attack.AttackInfo.Damage.DamagePoints += 2;
            } else {
                CombatPadScript.IncrementSuperBar(5);
                messageManagerScript.ShowMessage(Messages.MSG_ATTACK_SUPER_BAR_BONUS, 2);
                DynamicObjectManagerScript.Clone(KABAMModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
            }
        }
        private bool SetupAttackInfo() {
             GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
             if (null != messageCanvas) messageManagerScript = messageCanvas.GetComponent<MessageManager>();
             GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
             if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
             GameObject combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
             if (null != combatPanel) CombatPadScript = combatPanel.GetComponent<CombatPad>();
             GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
             if (null != debugConsole) DebugConsoleScript = debugConsole.GetComponent<DebugConsole>();

             if (null == messageManagerScript) { Debug.LogError("Attack.SetupAttack: messageManagerScript is null."); }
             if (null == DynamicObjectManagerScript) { Debug.LogError("Attack.SetupAttack: DynamicObjectManagerScript is null."); }
             if (null == CombatPadScript) { Debug.LogError("Attack.SetupAttack: CombatPadScript is null."); }
             if (null == DebugConsoleScript) { Debug.LogError("Attack.SetupAttack: DebugConsoleScript is null."); }

             if (null == messageManagerScript) { return false; }
             if (null == DynamicObjectManagerScript) { return false; }
             if (null == CombatPadScript) { return false; }
             if (null == DebugConsoleScript) { return false; }

             BAMBonusChance = 33;
             POWBonusChance = 66;
             isSetup = true;
             return true;
        }
   }
}
