using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Combat
{
    public abstract class AbstractBuff : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Attack Buff(Attack attack) { return attack; }

    }
}
