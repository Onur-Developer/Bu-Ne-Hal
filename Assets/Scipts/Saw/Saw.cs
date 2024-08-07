using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Scipts.Saw
{
    public class Saw : MonoBehaviour
    {
        [Header("Moving Properties")] [SerializeField]
        private MovingDirection movingDirection;

        [SerializeField] private float movingWay;
        [SerializeField] private float movingTime;

        [Header("Rotation Properties")] [SerializeField]
        private RotationDirection rotationDirection;

        [SerializeField] private float spinningTime;

        private float _movingDir;
        private float _rotationDir;
        private CircleCollider2D _cr;
        private Tween _t, _t2;

        enum MovingDirection
        {
            UpDown,
            LeftRight
        }

        enum RotationDirection
        {
            Left,
            Right
        }

        private void Awake()
        {
            _cr = GetComponent<CircleCollider2D>();
            _movingDir = movingDirection == MovingDirection.LeftRight
                ? _movingDir = transform.position.x + movingWay
                : _movingDir = transform.position.y + movingWay;
            _rotationDir = rotationDirection == RotationDirection.Left ? 180 : -180;
        }

        private void Start()
        {
            if (movingDirection == MovingDirection.LeftRight)
                _t = transform.DOMoveX(_movingDir, movingTime)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
            else
                _t = transform.DOMoveY(_movingDir, movingTime)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
            _t2 = transform.DORotate(new Vector3(0, 0, _rotationDir), spinningTime)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        private void OnDestroy()
        {
            _t.Kill();
            _t2.Kill();
        }


        IEnumerator SetCollider()
        {
            _cr.enabled = false;
            yield return new WaitForSeconds(2f);
            _cr.enabled = true;
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player")) StartCoroutine(SetCollider());
        }
    }
}