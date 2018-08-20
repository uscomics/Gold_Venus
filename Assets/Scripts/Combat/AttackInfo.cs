using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USComics;
using USComics_Debug;
using USComics_Dynamic;
using USComics_Movement;
using USComics_Message_Manager;

namespace USComics_Combat
{
    [System.Serializable]
    public class AttackInfo
    {
        public string name;
        public float damage;
        public float duration;
        public float range;
        public float recharge;
        public float lastUsed;
        public string[] animationNames;
        public GameObject pointsObject;
        public AudioSource audioSource1;
        public AudioSource audioSource2;
        public AudioClip sound1;
        public AudioClip sound2;
        public ParticleSystem[] particleSystems;
        public Light[] lights;
        public AbstractBuff[] buffs;
        public AbstractDebuff[] debuffs;
        public KeyCode[] keyCodes;
        public Image uiImage;
        public AttackInfo() { }
        public AttackInfo(AttackInfo attackInfo)
        {
            name = attackInfo.name;
            damage = attackInfo.damage;
            duration = attackInfo.duration;
            range = attackInfo.range;
            recharge = attackInfo.recharge;
            lastUsed = attackInfo.lastUsed;
            animationNames = attackInfo.animationNames;
            pointsObject = attackInfo.pointsObject;
            sound1 = attackInfo.sound1;
            sound2 = attackInfo.sound2;
            particleSystems = attackInfo.particleSystems;
            lights = attackInfo.lights;
            buffs = attackInfo.buffs;
            debuffs = attackInfo.debuffs;
            audioSource1 = attackInfo.audioSource1;
            audioSource2 = attackInfo.audioSource2;
            keyCodes = attackInfo.keyCodes;
            uiImage = attackInfo.uiImage;
        }
    }
}
