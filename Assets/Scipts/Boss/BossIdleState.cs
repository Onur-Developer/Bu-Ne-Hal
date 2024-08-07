using UnityEngine;


namespace Scipts.Boss
{
    public class BossIdleState : BossState
    {
        private float _attackTimer;

        public BossIdleState(Boss boss, BossStateMachine stateMachine) : base(boss, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Boss.LookWall();
            _attackTimer = 10f;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Boss.Move(Boss.idleSpeed);
            Boss.LookWall();
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0) Attack();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        void Attack()
        {
            int value = Boss.RandomValue();
            switch (value)
            {
                case 0:
                    StateMachine.ChangeState(Boss.MoveStateState);
                    break;
                case 1:
                    StateMachine.ChangeState(Boss.FlyState);
                    break;
                case 2:
                    StateMachine.ChangeState(Boss.FlameAttackState);
                    break;
            }
        }
    }
}