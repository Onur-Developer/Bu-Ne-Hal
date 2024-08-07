using System.Collections;
using UnityEngine;

namespace Scipts.Boss
{
    public class BossTakeDamageState : BossState
    {
        private Animator _anim;
        public BossTakeDamageState(Boss boss, BossStateMachine stateMachine,Animator animator) : base(boss, stateMachine)
        {
            _anim = animator;
        }


        public override void EnterState()
        {
            base.EnterState();
            _anim.Play("TakeDamage");
            Boss.ResetVelocity();
            Boss.PlaySound(Boss.takeDamageSound);
            Boss.StartCoroutine(Cooldown());
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
            _anim.Play("Default");
        }

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(Boss.shakingTime);
            StateMachine.ChangeState(Boss.IdleStateState);
        }
    }
}
