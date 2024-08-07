using Scipts.AllPlayers;

namespace Scipts.Player3
{
    public class Player3State : PlayerState
    {
        protected Player3 Player3;
        public Player3State(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, Player3 player3) : base(mainPlayer, playerStateMachine, stateName)
        {
            Player3 = player3;
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
