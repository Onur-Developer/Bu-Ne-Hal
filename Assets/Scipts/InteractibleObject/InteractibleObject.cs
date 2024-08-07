using UnityEngine;

namespace Scipts
{
    public class InteractibleObject : MonoBehaviour
    {
        private Vector2 _originalPos;

        private void Awake()
        {
            _originalPos = transform.position;
        }

        private void Start()
        {
            GameManager.instance.PlayerDie += ReturnOriginalPos;
        }


        void ReturnOriginalPos() => transform.position = _originalPos;
    }
}
