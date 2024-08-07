using UnityEngine;

namespace Scipts.FlameMachine
{
    public class FlameMachineFire : MonoBehaviour
    {
        private FlameMachine _flameMachine;

        private void Awake()
        {
            _flameMachine = GetComponentInParent<FlameMachine>();
        }


        public void Fire() => _flameMachine.Fire();
    }
}
