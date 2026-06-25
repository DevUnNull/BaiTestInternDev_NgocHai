using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.UpdateAnimationBlend(0.6f);

        Vector3 direction = player.MovementInput;

        if (direction.magnitude < 0.1f)
        {
            stateMachine.ChangeState(player.IdleState);
            return; 
        }

        player.MoveCharacter(direction);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
