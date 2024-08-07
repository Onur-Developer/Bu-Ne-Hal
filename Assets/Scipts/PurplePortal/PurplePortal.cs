using System.Collections;
using DG.Tweening;
using Scipts.AllPlayers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Scipts.PurplePortal
{
    public class PurplePortal : MonoBehaviour
    {
        private Transform _player;
        private GameObject _players;
        [SerializeField] private float pullTime;
        [SerializeField] private float dropTime;
        [SerializeField] private float returnTime = 1f;
        [SerializeField] private float minimumXaxis = -2;
        [SerializeField] private float maximumXaxis = 2;
        [SerializeField] private float minimumYaxis = -2;
        [SerializeField] private float maximumYaxis = 2;
        private Animator _animator;
        private AudioSource _au;
        [SerializeField] private AudioClip teleport1;
        [SerializeField] private AudioClip teleport2;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _players = GameObject.FindWithTag("Players");
            _au = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            StartCoroutine(DelayTime());
            PlaySound(teleport1);
        }

        void LookPlayer()
        {
            Vector3 lookDirection = _player.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Vector2 CreateRandomPos()
        {
            float randomX = Random.Range(minimumXaxis, maximumXaxis);
            float randomY = Random.Range(minimumYaxis, maximumYaxis);
            float randomXsign = Random.Range(0, 2) == 0 ? 1 : -1;
            float randomYsign = Random.Range(0, 2) == 0 ? 1 : -1;
            float newXpos = _player.position.x + randomX * randomXsign;
            float newYpos = _player.position.y + randomY * randomYsign;
            Vector2 newPos = new Vector2(newXpos, newYpos);
            return newPos;
        }

        void PlaySound(AudioClip clip)
        {
            _au.clip = clip;
            _au.Play();
        }

        void PullPlayer()
        {
            Sequence pullq = DOTween.Sequence();
            pullq.Append(_player.DOMove(transform.position, pullTime)
                .SetEase(Ease.InCubic)
                .OnComplete(PullPlayerComplete));
            pullq.Join(_player.DOScale(new Vector3(.1f, .1f, 1), pullTime)
                .SetEase(Ease.InQuart));
            pullq.Join(_player.DORotate(_player.localRotation.eulerAngles + new Vector3(0, 0, 180), pullTime)
                .SetEase(Ease.InQuart));
        }

        void DropPlayer()
        {
            _player.localScale = new Vector3(.1f, .1f, 1);
            Sequence pullq = DOTween.Sequence();
            pullq.Append(_player.DOScale(new Vector3(1f, 1f, 1), dropTime)
                .SetEase(Ease.InQuart)
                .OnComplete(DropPlayerComplete));
            pullq.Join(_player.DORotate(new Vector3(0, 0, 0), dropTime)
                .SetEase(Ease.InQuart)
                .OnComplete(() => { PlaySound(teleport2); }));
            if (_player.GetComponentInChildren<SpriteRenderer>() != null)
            {
                SpriteRenderer sr = _player.GetComponentInChildren<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
                pullq.Join(sr.DOFade(1, dropTime)
                    .SetEase(Ease.InQuint));
            }
        }

        void DropPlayerComplete()
        {
            MainPlayer mainPlayer = _player.GetComponent<MainPlayer>();
            mainPlayer.Respawn();
        }

        void PullPlayerComplete()
        {
            PlaySound(teleport2);
            GameManager.instance.ChangeCameraTarget(transform);
            _players.SetActive(false);
            _animator.SetTrigger("Disappear");
            StartCoroutine(ReturnBase());
        }

        IEnumerator ReturnBase()
        {
            yield return new WaitForSeconds(returnTime - .25f);
            _animator.SetTrigger("Appear");
            transform.position = _players.transform.position;
            _players.SetActive(true);
            GameManager.instance.ReturnPlayer1();
            _player = GameObject.FindWithTag("Player").transform;
            _player.transform.position = transform.position;
            _player.GetComponent<Player1.Player1>().isDash = true;
            DropPlayer();
            GameManager.instance.ChangeCameraTarget(_player);
            yield return new WaitForSeconds(dropTime);
            _animator.SetTrigger("Disappear");
            yield return new WaitForSeconds(0.7f);
            _player.GetComponent<Player1.Player1>().isDash = false;
            gameObject.SetActive(false);
        }

        IEnumerator DelayTime()
        {
            _player = GameManager.instance.choosenPlayer;
            if (_player.CompareTag("Player2"))
                _player = _player.transform.GetChild(0).GetChild(0).transform;
            transform.position = CreateRandomPos();
            yield return new WaitForSeconds(.1f);
            LookPlayer();
            PullPlayer();
        }
    }
}