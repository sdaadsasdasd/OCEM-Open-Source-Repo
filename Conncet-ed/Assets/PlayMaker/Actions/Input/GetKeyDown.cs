// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when a Key is pressed.")]
	public class GetKeyDown : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The key to detect.")]
		public KeyCode key;

        [Tooltip("The Event to send when the key is pressed.")]
		public FsmEvent sendEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a Bool Variable. True if pressed, otherwise False.")]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			sendEvent = null;
			key = KeyCode.None;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool keyDown = Input.GetKeyDown(key);
			
			if (keyDown)
				Fsm.Event(sendEvent);
			
			storeResult.Value = keyDown;
		}

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, key.ToString(), sendEvent != null ? sendEvent.Name : "");
        }
#endif
    }
}