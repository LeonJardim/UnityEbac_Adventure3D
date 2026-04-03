using Animation;
using Leon.StateMachine;
using UnityEngine;

namespace Boss
{
    public class BossStateBase : StateBase
    {
        protected BossBase boss;

        public override void OnEnter(params object[] objs)
        {
            base.OnEnter(objs);
            boss = (BossBase)objs[0];
        }
    }



    public class BossStateInit : BossStateBase
    {
        public override void OnEnter(params object[] objs)
        {
            base.OnEnter(objs);
            boss.StartInitAnimation();
        }
    }


    public class BossStateWalk : BossStateBase
    {
        public override void OnEnter(params object[] objs)
        {
            base.OnEnter(objs);
            boss.PlayAnimationByTrigger(AnimationType.RUN);
            boss.GoToRandomPoint(OnArrive);
        }
        public override void OnExit()
        {
            base.OnExit();
            boss.StopAllCoroutines();
        }

        private void OnArrive()
        {
            boss.SwitchState(BossAction.ATTACK);
        }
    }


    public class BossStateAttack : BossStateBase
    {
        public override void OnEnter(params object[] objs)
        {
            base.OnEnter(objs);
            boss.PlayAnimationByTrigger(AnimationType.ATTACK);
            boss.StartAttack(OnAttackFinished);
        }
        public override void OnExit()
        {
            base.OnExit();
            boss.StopAllCoroutines();
        }

        private void OnAttackFinished()
        {
            boss.SwitchState(BossAction.WALK);
        }
    }


    public class BossStateDeath : BossStateBase
    {
        public override void OnEnter(params object[] objs)
        {
            base.OnEnter(objs);
            boss.PlayAnimationByTrigger(AnimationType.DEATH);
        }
    }
}
