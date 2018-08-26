using UnityEngine;
using USComics_Debug;
using USComics_Entity;
using USComics_Dynamic;
using USComics_Message_Manager;

namespace USComics_FX {
	public class FXStateBassClass {
		protected EntityController PlayerEntityControllerScript;
		protected Animator PlayerAnimator;
		protected MessageManager MessageManagerScript;
		protected DynamicObjectManager DynamicObjectManagerScript;
		protected DebugConsole DebugConsoleScript;
	
		public virtual bool Setup() {
			GameObject playerCharacter = GameObject.FindWithTag("PlayerCharacter") as GameObject;
			if (null != playerCharacter) PlayerEntityControllerScript = playerCharacter.GetComponent<EntityController>();
			if (null != playerCharacter) PlayerAnimator = playerCharacter.GetComponent<Animator>();
			GameObject messageCanvas = GameObject.FindWithTag("MessageCanvas") as GameObject;
			if (null != messageCanvas) MessageManagerScript = messageCanvas.GetComponent<MessageManager>();
			GameObject dynamicObjects = GameObject.FindWithTag("DynamicObjects") as GameObject;
			if (null != dynamicObjects) DynamicObjectManagerScript = dynamicObjects.GetComponent<DynamicObjectManager>();
			GameObject debugConsole = GameObject.FindWithTag("DebugConsole") as GameObject;
			if (null != debugConsole) DebugConsoleScript = debugConsole.GetComponent<DebugConsole>();
	
			if (null == PlayerEntityControllerScript) { Debug.LogError("FXStateBassClass.Setup: PlayerEntityControllerScript is null."); }
			if (null == PlayerAnimator) { Debug.LogError("FXStateBassClass.Setup: PlayerAnimator is null."); }
			if (null == MessageManagerScript) { Debug.LogError("FXStateBassClass.Setup: MessageManagerScript is null."); }
			if (null == DynamicObjectManagerScript) { Debug.LogError("FXStateBassClass.Setup: DynamicObjectManagerScript is null."); }
			if (null == DebugConsoleScript) { Debug.LogError("FXStateBassClass.Setup: DebugConsoleScript is null."); }
	
			if (null == PlayerEntityControllerScript) { return false; }
			if (null == PlayerAnimator) { return false; }
			if (null == MessageManagerScript) { return false; }
			if (null == DynamicObjectManagerScript) { return false; }
			if (null == DebugConsoleScript) { return false; }
	
			return true;
		}
	}
}
