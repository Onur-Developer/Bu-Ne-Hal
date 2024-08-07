
namespace Scipts.Boss
{
    public class BossMoveState : BossState
    {
        public BossMoveState(Boss boss, BossStateMachine stateMachine) : base(boss, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Boss.LookWall();
            Boss.PlaySound(Boss.moveSound);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if(Boss.WallCheck())
                StateMachine.ChangeState(Boss.IdleStateState);
            Boss.Move(Boss.movementSpeed);
        }

        public override void ExitState()
        {
            base.ExitState();
            Boss.ResetVelocity();
        }
    }
}
