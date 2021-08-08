// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the value of the specified Input Axis and stores it in a Float Variable. " +
             "See {{Unity Input Manager}} docs.")]
    [SeeAlso("Unity Input Manager")]
    public class GetAxis : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The name of the axis. Set in the Unity Input Manager.")]
        public FsmString axisName;

        [Tooltip("Normally axis values are in the range -1 to 1. Use the multiplier to make this range bigger. " +
                 "E.g., A multiplier of 100 returns values from -100 to 100.")]
		public FsmFloat multiplier;

        [Tooltip("Invert the value of for the axis. E.g., -1 becomes 1, and 1 becomes -1.")]
        public FsmBool invert;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a float variable.")]
        public FsmFloat store;

        [Tooltip("Get the axis value every frame. This should be true most of the time, but there might be times when you only want to get the value once.")]
		public bool everyFrame;

		public override void Reset()
		{
			axisName = "";
			multiplier = 1.0f;
            invert = null;
			store = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetAxis();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetAxis();
		}

		void DoGetAxis()
		{
		    if (FsmString.IsNullOrEmpty(axisName)) return;

			var axisValue = Input.GetAxis(axisName.Value);

			// if variable set to none, assume multiplier of 1
			if (!multiplier.IsNone)
			{
				axisValue *= multiplier.Value;
			}

            if (invert.Value)
            {
                axisValue *= -1;
            }

			store.Value = axisValue;
		}


#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, axisName, store);
        }
#endif
    }
}

