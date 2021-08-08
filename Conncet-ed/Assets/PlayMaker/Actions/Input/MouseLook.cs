// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	/// <summary>
	/// Action version of Unity's builtin MouseLook behaviour.
	/// TODO: Expose invert Y option.
	/// </summary>
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
	public class MouseLook : ComponentAction<Transform>
	{
		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The axes to rotate around.")]
		public RotationAxes axes = RotationAxes.MouseXAndY;

		[RequiredField]
		[Tooltip("Sensitivity of movement in X direction.")]
		public FsmFloat sensitivityX;

		[RequiredField]
		[Tooltip("Sensitivity of movement in Y direction.")]
		public FsmFloat sensitivityY;

		[HasFloatSlider(-360,360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
		public FsmFloat minimumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
        public FsmFloat maximumX;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat minimumY;

		[HasFloatSlider(-360, 360)]
        [Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
        public FsmFloat maximumY;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

        private float rotationX;
        private float rotationY;

        public override void Reset()
		{
			gameObject = null;
			axes = RotationAxes.MouseXAndY;
			sensitivityX = 15f;
			sensitivityY = 15f;
			minimumX = new FsmFloat {UseVariable = true};
            maximumX = new FsmFloat { UseVariable = true };
			minimumY = -60f;
			maximumY = 60f;
			everyFrame = true;
		}

		public override void OnEnter()
		{
            if (!UpdateCachedTransform(Fsm.GetOwnerDefaultTarget(gameObject)))
            {
				Finish();
				return;
			}

			// Make the rigid body not change rotation
			// TODO: Original Unity script had this. Expose as option?
		    var rigidbody = cachedGameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
			{
				rigidbody.freezeRotation = true;
			}

            // initialize rotation

            rotationX = cachedTransform.localRotation.eulerAngles.y;
            rotationY = cachedTransform.localRotation.eulerAngles.x;

            if (!everyFrame)
			{
                DoMouseLook();
                Finish();
			}
        }

		public override void OnUpdate()
		{
			DoMouseLook();
		}

        private void DoMouseLook()
		{
            if (!UpdateCachedTransform(Fsm.GetOwnerDefaultTarget(gameObject)))
            {
                Finish();
                return;
            }

            switch (axes)
			{
				case RotationAxes.MouseXAndY:
					
					cachedTransform.localEulerAngles = new Vector3(GetYRotation(), GetXRotation(), 0);
					break;
				
				case RotationAxes.MouseX:

                    cachedTransform.localEulerAngles = new Vector3(cachedTransform.localEulerAngles.x, GetXRotation(), 0);
					break;

				case RotationAxes.MouseY:

                    cachedTransform.localEulerAngles = new Vector3(GetYRotation(-1), cachedTransform.localEulerAngles.y, 0);
					break;
			}
        }

        private float GetXRotation()
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX.Value;
			rotationX = ClampAngle(rotationX, minimumX, maximumX) % 360;
			return rotationX;
		}

        private float GetYRotation(float invert = 1)
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY.Value * invert;
			rotationY = ClampAngle(rotationY, minimumY, maximumY) % 360;
			return rotationY;
		}

		// Clamp function that respects IsNone and 360 wrapping
        private static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
		{
            if (angle < 0f) angle = 360 + angle;

            var from = min.IsNone ? -720 : min.Value;
            var to = max.IsNone ? 720 : max.Value;

            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
		}
	}
}