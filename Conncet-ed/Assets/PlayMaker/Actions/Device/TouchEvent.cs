// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events based on Touch Phases. Optionally filter by a fingerID.")]
	public class TouchEvent : FsmStateAction
	{
        [Tooltip("An optional Finger Id to filter by. For example, if you detected a Touch Began and stored the FingerId, you could look for the Ended event for that Finger Id.")]
        public FsmInt fingerId;
        [Tooltip("The phase you're interested in detecting (Began, Moved, Stationary, Ended, Cancelled).")]
        public TouchPhase touchPhase;
        [Tooltip("The event to send when the Touch Phase is detected.")]
        public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the Finger Id associated with the touch event for later use.")]
        public FsmInt storeFingerId;
		
		public override void Reset()
		{
			fingerId = new FsmInt { UseVariable = true } ;
			storeFingerId = null;
		}

		public override void OnUpdate()
		{
			if (Input.touchCount > 0)
			{
				foreach (var touch in Input.touches)
				{

					if (fingerId.IsNone || touch.fingerId == fingerId.Value)
					{
						if (touch.phase == touchPhase)
						{
							storeFingerId.Value = touch.fingerId;
							Fsm.Event(sendEvent);
						}
					}
				}
			}
		}
	}
}