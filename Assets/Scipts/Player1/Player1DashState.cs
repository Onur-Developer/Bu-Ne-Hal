using System.Collections;
using Scipts.AllPlayers;
using Scipts.BreakableObject;
using UnityEngine;

namespace Scipts.Player1
{
    public class Player1DashState : PlayerState
    {
        private global::Scipts.Player1.Player1 _player1;
        public Player1DashState(MainPlayer mainPlayer, PlayerStateMachine playerStateMachine, string stateName, global::Scipts.Player1.Player1 player1) : base(mainPlayer, playerStateMachine, stateName)
        {
            _player1 = player1;
        }


        public override void EnterState()
        {
            base.EnterState();
            _player1.Dash();
            _player1.StartCreateGhost();
            GameManager.instance.SetCameraShake(0.35f,20,2.5f);
            MainPlayer.StartCoroutine(DashTime());
            MainPlayer.PlaySoundEffect(_player1.dash);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            ControlBreakObject();
        }

        public override void ExitState()
        {
            base.ExitState();
            _player1.StopCreateGhost();
        }


        void ControlBreakObject()
        {

            if (_player1.BreakControl()==null) return;
            Breakable breakable = _player1.BreakControl().GetComponent<Breakable>();
            breakable.Break();
        }

        IEnumerator DashTime()
        {
            yield return new WaitForSeconds(_player1.dashTime);
            MainPlayer.StateMachine.ChangeState(_player1.IdleState);
        }
    }
}
