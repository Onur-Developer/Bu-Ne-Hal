using System.Collections;
using UnityEngine;

namespace Scipts.Boss
{
    public class BossFlyState : BossState
    {
        public BossFlyState(Boss boss, BossStateMachine stateMachine) : base(boss, stateMachine)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            Boss.SetFlyMode(true);
            Boss.CreateSaws();
            Boss.PlaySound(Boss.flySound);
            Boss.StartCoroutine(FlyCoolDown());
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Boss.Move(Boss.idleSpeed);
            Boss.LookWall();
            if(Boss.PlayerDied()) StateMachine.ChangeState(Boss.IdleStateState);
        }

        public override void ExitState()
        {
            base.ExitState();
            Boss.SetFlyMode(false);
            Boss.ResetVelocity();
            Boss.DestroySaws();
        }

        IEnumerator FlyCoolDown()
        {
            yield return new WaitForSeconds(Boss.flyTimer);
            if (StateMachine.CurrentState == this)
                StateMachine.ChangeState(Boss.IdleStateState);
        }
    }
}