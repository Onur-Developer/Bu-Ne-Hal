using UnityEngine;
using DG.Tweening;

namespace Scipts.Star
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private float upDirection=2f;
        private float _movingUpDirection;

        private void Awake()
        {
            _movingUpDirection = transform.position.y + upDirection;
        }

        void CollectAnimation()
        {
            GetComponent<AudioSource>().Play();
            GameManager.instance.IncreaseStar();
            transform.DOMoveY(_movingUpDirection, 1f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    transform.DOScale(new Vector2(2, 2), 1f)
                        .SetEase(Ease.InCubic)
                        .OnComplete(() =>
                        {
                            transform.DOScale(new Vector2(0, 0), 1f)
                                .SetEase(Ease.InCubic)
                                .OnComplete(() =>
                                {
                                    Destroy(gameObject,0.1f);
                                });
                        });
                });
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                CollectAnimation();
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
}
