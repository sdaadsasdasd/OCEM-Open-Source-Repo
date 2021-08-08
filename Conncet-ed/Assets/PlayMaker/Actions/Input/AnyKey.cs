// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
	public class AnyKey : FsmStateAction
    {
        [Tooltip("Where to send the event")]
        public FsmEventTarget eventTarget;

        [RequiredField]
		[Tooltip("Event to send when any Key or Mouse Button is pressed.")]
		public FsmEvent sendEvent;

		public override void Reset()
        {
            eventTarget = null;
			sendEvent = null;
		}

		public override void OnUpdate()
		{
			if (Input.anyKeyDown)
			{
                Fsm.Event(eventTarget, sendEvent);
            }
		}

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, sendEvent);
        }
#endif
    }
}