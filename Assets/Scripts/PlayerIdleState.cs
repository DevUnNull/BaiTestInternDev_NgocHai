using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.UpdateAnimationBlend(0f);

        if (player.MovementInput.magnitude >= 0.1f)
        {
            stateMachine.ChangeState(player.RunState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
