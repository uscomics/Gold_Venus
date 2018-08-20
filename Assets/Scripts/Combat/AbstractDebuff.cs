using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Combat
{
    public abstract class AbstractDebuff : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public abstract AttackInfo Debuff(AttackInfo attack);
    }
}
