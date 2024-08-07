using System.Collections;
using Scipts.AllPlayers;
using UnityEngine;

namespace Scipts.Player3
{
    public class Player3OutsideState : Player3State
    {
        private float _outsideDuration;

        public Player3OutsideState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName,
            Player3 player3) : base(mainPlayer, playerStateMachine, stateName, player3)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            Player3.Outside();
            Player3.eyeCollider.enabled = false;
            MainPlayer.PlaySoundEffect(Player3.gasOutside);
            _outsideDuration = Player3.outsideDuration;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (_outsideDuration <= 0) BackIdle();
            _outsideDuration -= Time.deltaTime;
        }

        public override void ExitState()
        {
            base.ExitState();
            Player3.players.canChangePlayer = true;
        }

        void BackIdle()
        {
            Player3.eyeCollider.enabled = true;
            StateMachine.ChangeState(Player3.IdleState);
        }
    }
}