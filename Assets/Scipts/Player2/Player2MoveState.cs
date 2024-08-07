using Scipts.AllPlayers;

namespace Scipts.Player2
{
    public class Player2MoveState : Player2State
    {
        public Player2MoveState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player2 player2) : base(mainPlayer, playerStateMachine, stateName, player2)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.axis==0)
                StateMachine.ChangeState(Player2.IdleState);
            else if (MainPlayer.isJump)
                StateMachine.ChangeState(Player2.JumpState);
            else if (!MainPlayer.GroundCheck() && Player2.WallCheck())
                StateMachine.ChangeState(Player2.WallSlideState);
            if (Player2.DieCheck())
                MainPlayer.StateMachine.ChangeState(MainPlayer.PlayerDieState);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
