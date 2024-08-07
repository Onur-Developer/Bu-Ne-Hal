using UnityEngine;
using DG.Tweening;

namespace Scipts.Player1
{
    public class Player1Ghost : MonoBehaviour
    {
        private void Start()
        {
            Sequence q = DOTween.Sequence();
            q.Append(transform.DOScale(0, 1f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    Destroy(gameObject, 1f);
                }));
            q.Join(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 180),1));
        }
    }
}
