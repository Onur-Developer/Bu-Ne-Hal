using Scipts.AllPlayers;

namespace Scipts.Player1
{
    public class Player1FallState : PlayerState
    {
        private Player1 _player1;
        public Player1FallState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName,Player1 player1) : base(mainPlayer, playerStateMachine, stateName)
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
            if(MainPlayer.GroundCheck())
                MainPlayer.StateMachine.ChangeState(_player1.IdleState);
        }

        public override void ExitState()
        {
            base.ExitState();
            _player1.CreateFallParticle();
        }
    }
}
