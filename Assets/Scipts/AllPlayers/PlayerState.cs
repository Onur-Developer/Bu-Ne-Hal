namespace Scipts.AllPlayers
{
    public class PlayerState
    {
        protected MainPlayer MainPlayer;
        protected PlayerStateMachine StateMachine;
        private string _stateName;
        public PlayerState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName)
        {
            MainPlayer = mainPlayer;
            StateMachine = playerStateMachine;
            _stateName = stateName;
        }
        public virtual void EnterState()
        {
            //MainPlayer.stateText.text = _stateName;
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState()
        {
            
        }
    }
}
