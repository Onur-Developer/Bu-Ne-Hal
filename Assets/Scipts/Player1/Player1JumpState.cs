
namespace Scipts.AllPlayers
{
    public class Player1JumpState : PlayerState
    {
        private global::Scipts.Player1.Player1 _player1;
        public Player1JumpState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, global::Scipts.Player1.Player1 player1) : base(mainPlayer, playerStateMachine, stateName)
        {
            _player1 = player1;
        }

        public override void EnterState()
        {
            base.EnterState();
            MainPlayer.isJump = false;
            MainPlayer.Jumping();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.GroundCheck() && MainPlayer.rb.velocity.y < 0)
            {
                _player1.CreateFallParticle();
                MainPlayer.StateMachine.ChangeState(_player1.IdleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
