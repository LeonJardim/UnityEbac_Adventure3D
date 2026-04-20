using UnityEngine;
using Leon.StateMachine;
using Animation;

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
        _player.animationBase.SetAnimationBool(AnimationType.RUN, true);
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
        _player.animationBase.SetAnimationBool(AnimationType.RUN, false);
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
        _player.isDead = true;
        _player.animationBase.PlayAnimationByTrigger(AnimationType.DEATH);
        _player.capCollider.enabled = false;
        if (_player.IsOnFloor())
            _player.characterController.enabled = false;
    }
    public override void OnStay()
    {
        if (_player.characterController.enabled)
        {
            if (_player.IsOnFloor() )
                _player.characterController.enabled = false;
        }
    }
    public override void OnExit()
    {
        _player.capCollider.enabled = true;
        _player.characterController.enabled = true;
        _player.isDead = false;
    }
}