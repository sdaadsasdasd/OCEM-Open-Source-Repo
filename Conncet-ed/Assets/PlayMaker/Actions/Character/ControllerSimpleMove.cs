// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Moves a Game Object with a Character Controller. Velocity along the y-axis is ignored. Speed is in meters/s. Gravity is automatically applied.")]
	public class ControllerSimpleMove : ComponentAction<CharacterController>
    {
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
		[Tooltip("A Game Object with a Character Controller.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The movement vector.")]
		public FsmVector3 moveVector;
		
		[Tooltip("Multiply the Move Vector by a speed factor.")]
		public FsmFloat speed;

		[Tooltip("Move in local or world space.")]
		public Space space;

        [Tooltip("Event sent if the Character Controller starts falling.")]
        public FsmEvent fallingEvent;

        private CharacterController controller
        {
            get { return cachedComponent; }
        }

        public override void Reset()
		{
			gameObject = null;
			moveVector = new FsmVector3 {UseVariable = true};
            speed = new FsmFloat {Value = 1};
			space = Space.World;
		}

		public override void OnUpdate()
		{
            if (!UpdateCacheAndTransform(Fsm.GetOwnerDefaultTarget(gameObject)))
                return;
		
            var move = space == Space.World ? moveVector.Value : cachedTransform.TransformDirection(moveVector.Value);
            controller.SimpleMove(move * speed.Value);

            if (!controller.isGrounded)
            {
                Fsm.Event(fallingEvent);
            }
		}
	}
}
