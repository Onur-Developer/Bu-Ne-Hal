using System.Collections;
using Scipts.AllPlayers;
using UnityEngine;

namespace Scipts.Player3
{
    public class Player3InsideState : Player3State
    {
        private bool _isInsideFinish;

        public Player3InsideState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName,
            Player3 player3) : base(mainPlayer, playerStateMachine, stateName, player3)
        {
        }


        public override void EnterState()
        {
            base.EnterState();
            Player3.Inside();
            _isInsideFinish = false;
            Player3.Interact += Outside;
            if (!Player3.isBeforeInside)
                MainPlayer.StartCoroutine(StartFly());
            else
                _isInsideFinish = true;
            Player3.players.canChangePlayer = false;
            Player3.SetDecreaseAmount(Player3.insideDecreaseAmount);
            MainPlayer.PlaySoundEffect(Player3.gasInside);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (!_isInsideFinish) return;
            MainPlayer.Movement();
            Player3.MovementObj();
            Player3.DecreaseHealth();
        }

        public override void ExitState()
        {
            base.ExitState();
            Player3.Interact -= Outside;
            Player3.isBeforeInside = false;
        }

        void Outside()
        {
            if(!_isInsideFinish) return;
            StateMachine.ChangeState(Player3.OutsideState);
        }

        IEnumerator StartFly()
        {
            yield return new WaitForSeconds(Player3.startInsideFlyTimer);
            _isInsideFinish = true;
        }
    }
}