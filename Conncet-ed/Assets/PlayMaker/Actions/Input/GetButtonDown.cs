// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when a Button is pressed.")]
	public class GetButtonDown : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The name of the button. Defined in the Unity Input Manager.")]
		public FsmString buttonName;

        [Tooltip("Event to send if the button is pressed.")]
		public FsmEvent sendEvent;

        [Tooltip("Set to True if the button is pressed, otherwise False.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		public override void Reset()
		{
			buttonName = "Fire1";
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			var buttonDown = Input.GetButtonDown(buttonName.Value);

            storeResult.Value = buttonDown;

            if (buttonDown)
			{
			    Fsm.Event(sendEvent);
			}
        }

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, buttonName) + " " + (sendEvent != null ? sendEvent.Name : "");
        }
#endif
    }
}