using Scipts.AllPlayers;

namespace Scipts.Player2
{
    public class Player2State : PlayerState
    {
        protected Player2 Player2;
        public Player2State(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player2 player2) : base(mainPlayer, playerStateMachine, stateName)
        {
            Player2 = player2;
        }


        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
