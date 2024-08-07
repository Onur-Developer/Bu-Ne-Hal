using System.Collections;
using DG.Tweening;
using Scipts.AllPlayers;
using UnityEngine;

namespace Scipts.GreenPortal
{
    public class GreenPortal : MonoBehaviour
    {
        public int currentLevel;
        public int currentStars;

        void PullPlayer(Transform player)
        {
            Sequence q = DOTween.Sequence();
            q.Append(player.DOMove(transform.position, 0.5f)
                .SetEase(Ease.Linear));
            q.Join(player.DOScale(0, 0.5f)
                .SetEase(Ease.Linear));
            q.OnComplete((() => GameManager.instance.OpenWinPanel()));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                float health = GameManager.instance.player3.health;
                bool isTurning = GameManager.instance.player3.isTurning;
                for (int i = 0; i < currentStars; i++)
                {
                    BadgeManager.BadgeManager.instance.CollectHalfStars();
                    BadgeManager.BadgeManager.instance.CollectAllStars();
                }

                BadgeManager.BadgeManager.instance.LastAirbender(health);
                BadgeManager.BadgeManager.instance.Pumped(isTurning);
                BadgeManager.BadgeManager.instance.Noob(currentLevel);
                BadgeManager.BadgeManager.instance.WithoutError(GameManager.instance.player3.isDied);
                BadgeManager.BadgeManager.instance.WithoutPortal(GameManager.instance.player3.isDied);
                PlayerPrefs.SetString("Level" + currentLevel, "Yes");
                if (currentStars > PlayerPrefs.GetInt("LevelStar" + currentLevel))
                    PlayerPrefs.SetInt("LevelStar" + currentLevel, currentStars);
                col.GetComponent<MainPlayer>().Win();
                GetComponent<AudioSource>().Play();
                PullPlayer(col.transform);
            }
        }
    }
}