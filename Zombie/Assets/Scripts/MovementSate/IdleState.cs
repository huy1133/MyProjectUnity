using UnityEngine;

public class IdleState : MovementBaseSate
{
    public override void EnterState(MovementStateManager state)
    {
        state.animator.SetBool("Idling", true);
    }

    public override void UpdateState(MovementStateManager state)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            state.attackDuration = state.attackTimer;
            ExitState(state, state.Attack);
        }
        if (state.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                ExitState(state, state.Run);
            else
                ExitState(state, state.Walk);
        }
    }
    public void ExitState(MovementStateManager state, MovementBaseSate baseSate)
    {
        state.animator.SetBool("Idling", false);
        state.SwitchState(baseSate);
    }
}
