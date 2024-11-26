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
                character.Move();
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

public class SnoutSatate : ICharacterState
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