using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement {
    public class MovementTransitionManager : MonoBehaviour {
        private MovementTransition _transition { get; set; }
        private List<AbstractMovementModule> _modules;
        private Animator _anim;

        void Awake() { _modules = new List<AbstractMovementModule>(); }
        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
            if (null != playerCharacter) _anim = playerCharacter.GetComponent<Animator>();
            if (null == _anim) { Debug.LogError("ClimbManager.Start: Anim is null."); }
            if (null == _anim) { return; }
        }
        void Update() {}
        public void Register(AbstractMovementModule movementModule) { _modules.Add(movementModule); }
        public AbstractMovementModule GetModule(ModuleTypes moduleType) {
            for (int loop = 0; loop < _modules.Count; loop++) {
                AbstractMovementModule module = _modules[loop];
                if (module.ModuleType() == moduleType) return module;
            }
            return null;
        }
        public bool StartTransitionFrom(ModuleTypes fromModuleType, ModuleTypes toModuleType) {
            AbstractMovementModule fromModule = GetModule(fromModuleType);
            AbstractMovementModule toModule = GetModule(toModuleType);
            if (null == fromModule) return false;
            if (null == toModule) return false;
            _transition = new MovementTransition(fromModule, toModule);
            _anim.Play(fromModule.GetTransitionFromAnimationName());
            return true;
        }
        public bool TransitionFromComplete() {
            _transition.From.StopModule();
            _anim.Play(_transition.To.GetTransitionToAnimationName());
            return true;
        }
        public bool TransitionToComplete() {
            _transition.To.StartModule();
            return true;
        }
        public bool IsCurrentTransition(ModuleTypes fromModule, ModuleTypes toModule) { return null != _transition && _transition.From.ModuleType() == fromModule && _transition.To.ModuleType() == toModule; }
        public void Clear() { _transition = null; }
    }
}
