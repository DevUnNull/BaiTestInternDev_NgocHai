public abstract class PlayerBaseState
{
    protected PlayerMovement player;
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerMovement player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
