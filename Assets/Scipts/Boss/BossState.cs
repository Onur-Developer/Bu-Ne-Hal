
namespace Scipts.Boss
{
    public class BossState
    {
        protected Boss Boss;
        protected BossStateMachine StateMachine;


        public BossState(Boss boss,BossStateMachine stateMachine)
        {
            Boss = boss;
            StateMachine = stateMachine;
        }
        
        
        public virtual void EnterState()
        {
            
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState()
        {
            
        }
    }
}
