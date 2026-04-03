using UnityEngine;
using Leon.StateMachine;

public class PState_Idle : StateBase
{
    private Player _player;

    public PState_Idle(Player player)
    {
        _player = player;
    }

    public override void OnEnter(params object[] objs)
    {
        
    }
    public override void OnStay()
    {
        _player.HandleJump();

        if (_player.moveInput.magnitude > 0f)
        {
            _player.stateMachine.SwitchState(Player.PlayerStates.RUN);
        }
    }
    public override void OnExit()
    {

    }
}



public class PState_Run : StateBase
{
    private Player _player;

    public PState_Run(Player player)
    {
        _player = player;
    }

    public override void OnEnter(params object[] objs)
    {
        _player.animator.SetBool("Run", true);
    }
    public override void OnStay()
    {
        _player.HandleJump();
        _player.HandleGroundMovement();
        if (_player.moveInput.magnitude == 0f)
        {
            _player.stateMachine.SwitchState(Player.PlayerStates.IDLE);
        }

    }
    public override void OnExit()
    {
        _player.animator.SetBool("Run", false);
    }
}



public class PState_Dead : StateBase
{
    private Player _player;

    public PState_Dead(Player player)
    {
        _player = player;
    }

    public override void OnEnter(params object[] objs)
    {

    }
    public override void OnStay()
    {

    }
    public override void OnExit()
    {

    }
}