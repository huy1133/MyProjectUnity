using UnityEngine;

public class RunState : MovementBaseSate
{
    public override void EnterState(MovementStateManager state)
    {
        state.animator.SetBool("Running", true);
        state.currentSpeed = state.runSpeed;
    }

    public override void UpdateState(MovementStateManager state)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            state.attackDuration = state.attackTimer;
            ExitState(state, state.Attack);
        }

        if (Input.GetKey(KeyCode.LeftShift)) ExitState(state, state.Walk);
        else if(state.dir.magnitude < 0.1f) ExitState(state, state.Idle);
    }

    public void ExitState(MovementStateManager state, MovementBaseSate baseSate)
    {
        state.animator.SetBool("Running", false);
        state.SwitchState(baseSate);
    }
}
