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

        private CombatPad _combatPadScript;
        private bool _isSetup;

        public AttackBonus() { }
        public AttackBonus(AttackBonus from) {
            BonusChance = from.BonusChance;
            BAMModel = from.BAMModel;
            POWModel = from.POWModel;
            KABAMModel = from.KABAMModel;
            BonusPoints = from.BonusPoints;
            BAMBonusChance = from.BAMBonusChance;
            POWBonusChance = from.POWBonusChance;
            _combatPadScript = from._combatPadScript;
        }
        public void CalculateBonus(Attack attack, EntityController attacker, EntityController target) {
            if (!_isSetup) SetupAttackInfo();
            int bonus = Random.Range(1, 101);
            if (bonus > BonusChance) return;
            bonus = Random.Range(1, 101);
            if (bonus <= BAMBonusChance) {
                attacker.ClearAttackTimers();
                MessageManager.INSTANCE.ShowMessage(Messages.MSG_ATTACK_TIMERS_CLEARED, 2);
                DynamicObjectManager.INSTANCE.Clone(BAMModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
            }
            else if (bonus <= POWBonusChance) {
                MessageManager.INSTANCE.ShowMessage(Messages.MSG_ATTACK_DAMAGE_BONUS, 2);
                DynamicObjectManager.INSTANCE.Clone(POWModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
                DynamicObjectManager.INSTANCE.Clone(BonusPoints, target.transform.position + Vector3.up, 0.0f, 180.0f, 0.0f);
                attack.AttackInfo.Damage.DamagePoints += 2;
            } else {
                _combatPadScript.IncrementSuperBar(5);
                MessageManager.INSTANCE.ShowMessage(Messages.MSG_ATTACK_SUPER_BAR_BONUS, 2);
                DynamicObjectManager.INSTANCE.Clone(KABAMModel, target.transform.position + Vector3.up, 0.0f, 0.0f, 0.0f);
            }
        }
        private bool SetupAttackInfo() {
             GameObject combatPanel = GameObject.FindWithTag("CombatPanel") as GameObject;
             if (null != combatPanel) _combatPadScript = combatPanel.GetComponent<CombatPad>();

             if (null == _combatPadScript) { Debug.LogError("Attack.SetupAttack: CombatPadScript is null."); }

             if (null == _combatPadScript) { return false; }

             BAMBonusChance = 33;
             POWBonusChance = 66;
             _isSetup = true;
             return true;
        }
   }
}
