
namespace Scipts.AllPlayers
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState { get; private set; }
        public void Inıtıalize(PlayerState firstState)
        {
            CurrentState = firstState;
            CurrentState.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
