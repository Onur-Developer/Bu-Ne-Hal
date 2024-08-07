using System.Collections;
using UnityEngine;

namespace Scipts.Boss
{
    public class BossFlameAttackState : BossState
    {
        public BossFlameAttackState(Boss boss, BossStateMachine stateMachine) : base(boss, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Boss.ResetVelocity();
            Boss.SetFlames(true);
            Boss.PlaySound(Boss.drawGunSound);
            Boss.StartCoroutine(FireCooldown());
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Boss.LookPlayer();
            if (Boss.PlayerDied()) StateMachine.ChangeState(Boss.IdleStateState);
        }

        public override void ExitState()
        {
            base.ExitState();
            Boss.SetFlames(false);
        }

        IEnumerator FireCooldown()
        {
            yield return new WaitForSeconds(Boss.flameAttackTime);
            if (StateMachine.CurrentState==this)
                StateMachine.ChangeState(Boss.IdleStateState);
        }
    }
}