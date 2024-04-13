using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = false;
		public bool cursorInputForLook = true;

		[Header("Spells")]
		public bool red = false;
		public bool blue = false;
		public bool green = false;
		public bool white = false;
		public int spellId = 0; // 0-nothing 1-red, 2-blue...


#if ENABLE_INPUT_SYSTEM
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
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnCursorLock(InputValue value)
		{
			cursorLocked = !cursorLocked;
			CursorInput();
		}

		public void OnAttack1(InputValue value)
		{
			AttackInput(1, value.isPressed);
		}

		public void OnAttack2(InputValue value)
		{
			AttackInput(2, value.isPressed);
		}

		public void OnAttack3(InputValue value)
		{
			AttackInput(3, value.isPressed);
		}

		public void OnAttack4(InputValue value)
		{
			AttackInput(4, value.isPressed);
		}
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

		public void CursorInput()
		{
			SetCursorState(cursorLocked);
		}

		// private void OnApplicationFocus(bool hasFocus)
		// {
		// 	SetCursorState(cursorLocked);
		// }

		public void AttackInput(int spell, bool isPressed){
			switch(spell){
				case 1:
					red = isPressed;
					spellId = isPressed? 1: 0;
					break;
				case 2:
					blue = isPressed;
					spellId = isPressed? 2: 0;
					break;
				case 3:
					green = isPressed;
					spellId = isPressed? 3: 0;
					break;
				case 4:
					white = isPressed;
					spellId = isPressed? 4: 0;
					break;
			}
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}