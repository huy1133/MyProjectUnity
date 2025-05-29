using UnityEngine;

public class WalkState : MovementBaseSate
{
    public override void EnterState(MovementStateManager state)
    {
        state.animator.SetBool("Walking", true);
        state.currentSpeed = state.walkSpeed;
    }

    public override void UpdateState(MovementStateManager state)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            state.attackDuration = state.attackTimer;
            ExitState(state, state.Attack);
        }

        if (Input.GetKey(KeyCode.LeftShift)) ExitState(state, state.Run);
        else if(state.dir.magnitude < 0.1f) ExitState(state, state.Idle);
    }

    public void ExitState(MovementStateManager state, MovementBaseSate baseSate)
    {
        state.animator.SetBool("Walking", false);
        state.SwitchState(baseSate);
    }
}
