
namespace Scipts.AllPlayers
{
    public class Player1IdleState : PlayerState
    {
        private global::Scipts.Player1.Player1 _player1;
        public Player1IdleState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, global::Scipts.Player1.Player1 player1) : base(mainPlayer, playerStateMachine, stateName)
        {
            _player1 = player1;
        }


        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.axis!=0)
                MainPlayer.StateMachine.ChangeState(_player1.MoveState);
            else if (MainPlayer.isJump)
                MainPlayer.StateMachine.ChangeState(_player1.JumpState);
            else if (!MainPlayer.GroundCheck())
                MainPlayer.StateMachine.ChangeState(_player1.FallState);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
