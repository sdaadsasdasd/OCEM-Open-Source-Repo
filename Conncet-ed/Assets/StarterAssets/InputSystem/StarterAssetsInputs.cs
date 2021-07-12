using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
        public energyBar eRef;
        public healthBar hRef;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
        private float maxEnergy = 10; //Editable
        private float maxHealth = 100; //Editable
        public float currentEnergy ; //Editable
        public float currentHealth ; //Editable
		public float timerEnergy = 360;


        [Header("Movement Settings")]
		public bool analogMovement;

		[SerializeField] private UIController uiControl;

        private void Start()
        {
            currentEnergy = maxEnergy;
            currentHealth = maxHealth;
            eRef.setMaxEnergy(currentEnergy);
            hRef.setMaxHealth(currentHealth);
        }
        private void Update()
        {
			if (timerEnergy > 0)
				timerEnergy -= Time.deltaTime;
			else
			{
				currentEnergy--;
				timerEnergy = 360;
			}
            hRef.setHealth(currentHealth);
			eRef.setEnergy(currentEnergy);

        }

#if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED


        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
            currentHealth--;
            
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
        }

		public void OnMenu(InputValue value)
		{
            if (value.isPressed)
            {
				if (uiControl.IsMenuOpen())
				{
					uiControl.CloseMenu();
				}
				else
				{
					uiControl.OpenMenu();
				}

			}
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


            public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}