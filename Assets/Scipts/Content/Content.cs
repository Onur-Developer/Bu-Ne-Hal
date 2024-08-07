using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Scipts.Content
{
    public class Content : MonoBehaviour
    {
        private const float DefaultWeight = 250.0f;
        private const float DefaultHeight = 350.0f;
        private const float CenterWeight = 350.0f;
        private const float CenterHeight = 400.0f;
        private int _centerIndex = 1;
        private int _formerCenterIndex;
        private RectTransform _contentRect;
        [SerializeField] private Button[] buttons;
        private AudioSource _au;

        private void Awake()
        {
            _contentRect = GetComponent<RectTransform>();
            _au = GetComponent<AudioSource>();
        }


         void PlaySound() => _au.Play();

        public void LeftArrow()
        {
            _formerCenterIndex = _centerIndex;
            _centerIndex -= 1;
            if (_centerIndex == -1)
            {
                _centerIndex = 0;
                return;
            }

            PlaySound();
            ChangeBadge(0);
        }

        public void RightArrow()
        {
            _formerCenterIndex = _centerIndex;
            _centerIndex += 1;
            if (_centerIndex == 10)
            {
                _centerIndex = 9;
                return;
            }

            PlaySound();
            ChangeBadge(1);
        }

        void ChangeBadge(int index)
        {
            SetButtonInteractible(false);
            float directionX = index == 1 ? -377.75f : +377.75f;
            float newDirectionX = _contentRect.offsetMin.x + directionX;
            RectTransform formerBadgeRect = transform.GetChild(_formerCenterIndex).GetComponent<RectTransform>();
            RectTransform newBadgeRect = transform.GetChild(_centerIndex).GetComponent<RectTransform>();
            Sequence q = DOTween.Sequence();
            q.Append(_contentRect.DOAnchorPos(new Vector2(newDirectionX, 0), 0.5f)
                .SetEase(Ease.OutBack));
            q.Join(formerBadgeRect.DOSizeDelta(new Vector2(DefaultWeight, DefaultHeight), 0.5f)
                .SetEase(Ease.OutBack));
            q.Join(newBadgeRect.DOSizeDelta(new Vector2(CenterWeight, CenterHeight), 0.5f)
                .SetEase(Ease.OutBack));
            q.AppendCallback(() => { SetButtonInteractible(true); });
        }

        void SetButtonInteractible(bool interactible)
        {
            foreach (Button b in buttons)
            {
                b.interactable = interactible;
            }
        }
    }
}