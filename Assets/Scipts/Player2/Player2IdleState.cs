using Scipts.AllPlayers;

namespace Scipts.Player2
{
    public class Player2IdleState : Player2State
    {
        public Player2IdleState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player2 player2) : base(mainPlayer, playerStateMachine, stateName, player2)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            MainPlayer.axis = 0;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            if (MainPlayer.axis!=0)
                MainPlayer.StateMachine.ChangeState(Player2.MoveState);
            else if (MainPlayer.isJump)
                MainPlayer.StateMachine.ChangeState(Player2.JumpState);
            if (Player2.DieCheck())
                MainPlayer.StateMachine.ChangeState(MainPlayer.PlayerDieState);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
