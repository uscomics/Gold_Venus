using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Entity;

namespace USComics_Combat {
    [System.Serializable]
    public abstract class AbstractBuff {
        public float StartTime { get; set;  }
        public bool Expired { get; set;  }
        public EntityController Target { get; set;  }
        public EntityController Attacker { get; set;  }
        
        public AbstractBuff() { SetupBuff(); }
        public AbstractBuff(AbstractBuff buff) {
            SetupBuff();
            StartTime = buff.StartTime;
            Expired = buff.Expired;
            Target = buff.Target;
            Attacker = buff.Attacker;
        }
        public abstract AbstractBuff Clone();
        public abstract Attack Buff(Attack attack);
        public abstract EntityController Buff(EntityController entity);

        protected virtual bool SetupBuff() {
            StartTime = 0.0f;
            Expired = false;
            Target = null;
            Attacker = null;
            return true;
        }
    }
}
