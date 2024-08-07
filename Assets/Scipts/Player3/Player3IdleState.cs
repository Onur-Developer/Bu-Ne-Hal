using Scipts.AllPlayers;
using UnityEngine;

namespace Scipts.Player3
{
    public class Player3IdleState : Player3State
    {
        public Player3IdleState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName,
            Player3 player3) : base(mainPlayer, playerStateMachine, stateName, player3)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            Player3.Interact += CheckInsıde;
            Player3.SetDecreaseAmount(Player3.ıdleDecreaseAmount);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            MainPlayer.Movement();
            Player3.ObjectCheck();
            Player3.DecreaseHealth();
        }

        public override void ExitState()
        {
            base.ExitState();
            Player3.Interact -= CheckInsıde;
        }

        void CheckInsıde()
        {
            if (Player3.ObjectCheck())
                StateMachine.ChangeState(Player3.InsideState);
        }
    }
}