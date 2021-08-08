// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when a Key is released.")]
	public class GetKeyUp : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The key to detect.")]
        public KeyCode key;

        [Tooltip("The Event to send when the key is released.")]
        public FsmEvent sendEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a Bool Variable. True if released, otherwise False.")]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			sendEvent = null;
			key = KeyCode.None;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool keyUp = Input.GetKeyUp(key);
			
			if (keyUp)
				Fsm.Event(sendEvent);
			
			storeResult.Value = keyUp;
		}

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, key.ToString(), sendEvent != null ? sendEvent.Name : "");
        }
#endif
    }
}