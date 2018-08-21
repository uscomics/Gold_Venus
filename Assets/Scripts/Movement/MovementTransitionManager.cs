using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement
{
    public class MovementTransitionManager : MonoBehaviour
    {
        private MovementModulesTransition Transition { get; set; }
        private List<AbstractMovementModule> modules;
        private Animator Anim;

        void Awake()
        {
            modules = new List<AbstractMovementModule>();
        }
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) Anim = playerCharacter.GetComponent<Animator>();
            if (null == Anim) { Debug.LogError("ClimbManager.Start: Anim is null."); }
            if (null == Anim) { return; }
        }
        void Update() {}
        public void Register(AbstractMovementModule movementModule) {
            modules.Add(movementModule);
        }
        public AbstractMovementModule GetModule(ModuleTypes moduleType)
        {
            for (int loop = 0; loop < modules.Count; loop++)
            {
                AbstractMovementModule module = modules[loop];
                if (module.ModuleType() == moduleType) return module;
            }
            return null;
        }
        public bool StartTransitionFrom(ModuleTypes fromModuleType, ModuleTypes toModuleType)
        {
            AbstractMovementModule fromModule = GetModule(fromModuleType);
            AbstractMovementModule toModule = GetModule(toModuleType);
            if (null == fromModule) return false;
            if (null == toModule) return false;
            Transition = new MovementModulesTransition(fromModule, toModule);
            Anim.Play(fromModule.GetTransitionFromAnimationName());
            return true;
        }
        public bool TransitionFromComplete()
        {
            Transition.From.StopModule();
            Anim.Play(Transition.To.GetTransitionToAnimationName());
            return true;
        }
        public bool TransitionToComplete()
        {
            Transition.To.StartModule();
            return true;
        }
    }

    [System.Serializable]
    class MovementModulesTransition
    {
        public AbstractMovementModule From { get; set; }
        public AbstractMovementModule To { get; set; }

        public MovementModulesTransition(AbstractMovementModule from, AbstractMovementModule to)
        {
            From = from;
            To = to;
        }
    }
}
