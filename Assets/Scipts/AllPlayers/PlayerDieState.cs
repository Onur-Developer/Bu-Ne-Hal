
namespace Scipts.AllPlayers
{
    public class PlayerDieState : PlayerState
    {
        public PlayerDieState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName) : base(mainPlayer, playerStateMachine, stateName)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            MainPlayer.Die();
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
