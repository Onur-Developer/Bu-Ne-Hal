using JetBrains.Annotations;
using UnityEngine;
using DG.Tweening;

namespace Scipts.DirectionArrow
{
    public class DirectionArrow : MonoBehaviour
    {
        [CanBeNull] private Transform _player;
        private bool _isActive;
       [HideInInspector] public Transform target;
        private float _scale;

        private void Awake()
        {
            _scale = transform.localScale.x;
        }

        private void Start()
        {
            transform.localScale = new Vector2(0, 0);
            gameObject.SetActive(false);
        }


        public void ChangeActive()
        {
            _isActive = !_isActive;
            gameObject.SetActive(true);
            if (_isActive)
                transform.DOScale(_scale, 0.5f)
                    .SetEase(Ease.InOutBounce)
                    .OnComplete(CompleteScale);
            else
                transform.DOScale(0, 0.5f)
                    .SetEase(Ease.InOutBounce)
                    .OnComplete(CompleteScale);
        }
        
        void CompleteScale() =>gameObject.SetActive(_isActive);

        private void Update()
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject == null) return;
            _player = playerObject.transform;
            transform.position = new Vector2(_player.position.x, _player.position.y + 1.5f);
            Vector3 lookDirection = target.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}