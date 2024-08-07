using Scipts.AllPlayers;

namespace Scipts.Player2
{
    public class Player2WallSlideState : Player2State
    {
        public Player2WallSlideState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player2 player2) : base(mainPlayer, playerStateMachine, stateName, player2)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            Player2.InSliding();
            Player2.SlideJumping += SlideJump;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.GroundCheck() && !Player2.WallCheck()) StateMachine.ChangeState(Player2.IdleState);
        }

        public override void ExitState()
        {
            base.ExitState();
            Player2.OutSliding();
            Player2.SlideJumping -= SlideJump;
        }

        void SlideJump()
        {
            StateMachine.ChangeState(Player2.SlideJump);
        }
    }
}
