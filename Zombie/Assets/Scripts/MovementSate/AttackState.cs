using UnityEngine;

public class AttackState : MovementBaseSate
{
    public override void EnterState(MovementStateManager state)
    {
        state.SwitchCameraMode(CameraMode.Aiming);
        state.aimStateManager.SetAimRigActive(true);
        state.animator.SetBool("Attacking", true);
        state.currentSpeed = state.aimSpeed;
    }

    public override void UpdateState(MovementStateManager state)
    {
        state.attackDuration -= Time.deltaTime;
        if (state.attackDuration <= 0)
        {
            ExitState(state, state.Idle);
        }
        else if (state.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                ExitState(state, state.Run);
        }
    }

    public void ExitState(MovementStateManager state, MovementBaseSate baseSate)
    {
        state.SwitchCameraMode(CameraMode.FreeLook);
        state.aimStateManager.SetAimRigActive(false);
        state.animator.SetBool("Attacking", false);
        state.SwitchState(baseSate);
    }
}
