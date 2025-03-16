using UnityEngine;

public interface ICharacterState
{
    void EnterState(MoveV3 character);
    void UpdateState(MoveV3 character);
    void ExitState(MoveV3 character);
}

public class AirState : ICharacterState
{
    public void EnterState(MoveV3 character)
    {
        character.animator.SetBool("IsFall", true);
        character.animator.SetBool("GetLand", false);
        character.rb.gravityScale = 2.5f;
        character.canMove = false;
    }

    public void ExitState(MoveV3 character)
    {
        character.animator.SetBool("IsFall", false);
        character.canJump = false;
    }

    public void UpdateState(MoveV3 character)
    {
        
    }
}

public class GroundState : ICharacterState
{
    public void EnterState(MoveV3 character)
    {
        character.animator.SetBool("IsMove",true);
        character.animator.SetBool("GetLand", true);
        character.rb.gravityScale = 2.5f;
        character.canMove = false;
        character.Invoke(nameof(character.ResetMove),0.3f);
        character.Invoke(nameof(character.ResetJump), 0.2f);

    }

    public void ExitState(MoveV3 character)
    {
        character.animator.SetBool("IsMove", false);
        character.animator.SetBool("IsJump", false);
        character.animator.SetFloat("Moving", 0);
    }

    public void UpdateState(MoveV3 character)
    {
        character.inputHorizontalDirectly();
        if (Input.GetKey(KeyCode.R))
        {
            if (character.CheckCanGrap() && !character.isGrap)
            {
                character.isGrap = true;
                character.canMove = false;
                character.canJump = false;
                character.SetTransfromBeforeGrap();
                character.animator.SetBool("Grap", character.isGrap);
            }
        }
        if (!character.isGrap)
        {
            if (character.canMove)
            {
                character.animator.SetFloat("Moving", Mathf.Abs(character.moveInput));
                character.MoveOnGround();
            }
            if (Input.GetKey(KeyCode.E) && character.canJump)
            {
                character.canJump = false;
                character.canMove = false;
                character.jumpDirection = character.moveInput;
                character.Invoke(nameof(character.Jump), 0.2f);
                character.animator.SetBool("IsJump", true);
            }
        }
    }
}

public class SnoutState : ICharacterState
{
    public void EnterState(MoveV3 character)
    {
        character.rb.velocity = Vector3.zero;
        character.rb.gravityScale = 0;
        character.animator.SetBool("IsSwing", true);
    }

    public void ExitState(MoveV3 character)
    {
        character.animator.SetBool("IsSwing", false);
    }

    public void UpdateState(MoveV3 character)
    {
        if (Input.GetKey(KeyCode.E))
        {
            character.animator.SetBool("Grap",true);
        }
    }
}

public class RopeState : ICharacterState
{
    public void EnterState(MoveV3 character)
    {
        character.CatchRope();
        character.animator.SetBool("IsSwing", true);
        character.canMove = true;
        character.canJumpOnRope = true;
        character.canSwing = false;
    }

    public void ExitState(MoveV3 character)
    {
        character.animator.SetBool("IsSwing", false);
        character.animator.SetBool("IsJumpOnRope", false);
        character.canMove = false;
    }
    public void UpdateState(MoveV3 character)
    {
        character.inputVerticalDirectly();
        character.inputHorizontalDirectly();
        character.animator.SetFloat("Climbing", character.climbInput);
        if (character.climbInput != 0)
        {
            character.MoveOnRope();
            Debug.Log(character.climbInput);
        }
        if (character.moveInput != 0)
        {
            character.SwingOnRope();
        }
        if (Input.GetKey(KeyCode.E) && character.canJumpOnRope)
        {
            character.animator.SetBool("IsJumpOnRope", true);
            character.jumpDirection = character.moveInput;
            character.Invoke(nameof(character.JumpOnRope), 0.3f);
            character.canJumpOnRope = false;
        }
    }
}