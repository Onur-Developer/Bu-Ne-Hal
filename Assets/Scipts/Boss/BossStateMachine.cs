
namespace Scipts.Boss
{
    public class BossStateMachine
    {
        public BossState CurrentState { get; private set; }

        public void Initialize(BossState firstState)
        {
            CurrentState = firstState;
            CurrentState.EnterState();
        }

        public void ChangeState(BossState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
