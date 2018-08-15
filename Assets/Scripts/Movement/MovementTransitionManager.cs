using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement
{
    public class MovementTransitionManager : MonoBehaviour
    {
        public MovementModulesTransition Transition { get; set; }
        private List<AbstractMovementModule> modules;
        private Animator Anim;

        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();

            if (null == Anim) { Debug.LogError("ClimbManager.Start: Anim is null."); }

            if (null == Anim) { return; }

            modules = new List<AbstractMovementModule>();
        }
        void Update() {}
        public void Register(AbstractMovementModule movementModule) { modules.Add(movementModule); }
        public AbstractMovementModule GetModule(ModuleTypes moduleType)
        {
            for (int loop = 0; loop < modules.Count; loop++)
            {
                AbstractMovementModule module = modules[loop];
                if (module.ModuleType() == moduleType) return module;
            }
            return null;
        }
        public bool TransitionFromStarted()
        {
            AbstractMovementModule fromModule = GetModule(Transition.From);
            AbstractMovementModule toModule = GetModule(Transition.To);
            if (null == fromModule) return false;
            if (null == toModule) return false;
            Anim.SetBool(fromModule.StopAnimationVariable(), true);
            return true;
        }
        public bool TransitionFromComplete()
        {
            AbstractMovementModule fromModule = GetModule(Transition.From);
            AbstractMovementModule toModule = GetModule(Transition.To);
            if (null == fromModule) return false;
            if (null == toModule) return false;
            fromModule.StopModule();
            Anim.SetBool(fromModule.StopAnimationVariable(), false);
            Anim.SetBool(toModule.StartAnimationVariable(), true);
            return true;
        }
        public bool TransitionToComplete()
        {
            AbstractMovementModule fromModule = GetModule(Transition.From);
            AbstractMovementModule toModule = GetModule(Transition.To);
            if (null == fromModule) return false;
            if (null == toModule) return false;
            Anim.SetBool(toModule.StartAnimationVariable(), false);
            toModule.StartModule();
            return true;
        }
    }

    [System.Serializable]
    public class MovementModulesTransition
    {
        public ModuleTypes From { get; set; }
        public ModuleTypes To { get; set; }

        public MovementModulesTransition(ModuleTypes from, ModuleTypes to)
        {
            From = from;
            To = to;
        }
    }
}
