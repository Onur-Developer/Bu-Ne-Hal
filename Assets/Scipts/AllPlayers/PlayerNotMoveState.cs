using UnityEngine;

namespace Scipts.AllPlayers
{
    public class PlayerNotMoveState : PlayerState
    {
        public PlayerNotMoveState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName) : base(mainPlayer, playerStateMachine, stateName)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            MainPlayer.rb.velocity=Vector2.zero;
            MainPlayer.rb.bodyType = RigidbodyType2D.Kinematic;
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
            MainPlayer.rb.bodyType = RigidbodyType2D.Dynamic;
            MainPlayer.axis = 0;
        }
    }
}
