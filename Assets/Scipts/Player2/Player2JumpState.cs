using Scipts.AllPlayers;

namespace Scipts.Player2
{
    public class Player2JumpState : Player2State
    {
        public Player2JumpState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player2 player2) : base(mainPlayer, playerStateMachine, stateName, player2)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            MainPlayer.isJump = false;
            MainPlayer.Jumping();
            MainPlayer.PlaySoundEffect(Player2.jump);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.GroundCheck() && MainPlayer.rb.velocity.y < 0)
                MainPlayer.StateMachine.ChangeState(Player2.IdleState);
            else if (!MainPlayer.GroundCheck() && Player2.WallCheck())
                StateMachine.ChangeState(Player2.WallSlideState);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}