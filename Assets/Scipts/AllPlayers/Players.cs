using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scipts.AllPlayers
{
    public class Players : MonoBehaviour
    {
        private Transform[] _players = new Transform[3];
        [SerializeField] private GameObject player2Instance;
        private Transform _player2Centre;
        private GameObject _player2;
        [HideInInspector] public bool canChangePlayer = true;
        private Vector2 _oldpos;
        [HideInInspector] public int activePlayer;
        private Material _player2Material;
        private SpriteRenderer _player1Sr;
        public float opacityDuration;
        [SerializeField] private TextMeshProUGUI transitionTx;
        private GameObject _stove;
        private GameObject _nitrogenTube;
        private bool _isStove;
        private AudioSource _au;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip nitrogenSound;

        private void Start()
        {
            _players[0] = transform.GetChild(0).transform;
            _players[2] = transform.GetChild(1).transform;
            activePlayer = 0;
            _player1Sr = _players[0].GetChild(0).GetComponent<SpriteRenderer>();
            _stove = GameManager.instance.stove;
            _nitrogenTube = GameManager.instance.nitrogenTube;
            _au = GetComponent<AudioSource>();
        }

        void PlaySound(AudioClip clip)
        {
            _au.clip = clip;
            _au.Play();
        }


        public void ChangePlayer(int newActivedPlayer)
        {
            StopBeforePlayer();
            TakeBeforePosition();
            _players[newActivedPlayer].gameObject.SetActive(true);
            GameManager.instance.choosenPlayer = _players[newActivedPlayer];
            _players[newActivedPlayer].position = _oldpos;
            ChangeTxPos();
            _isStove = activePlayer < newActivedPlayer;
            GameManager.instance.eyes.CloseEyes();
            TweenAnimation(newActivedPlayer, activePlayer);
            ChangeTxText(WriteTx(activePlayer, newActivedPlayer));
            activePlayer = newActivedPlayer;
            if (newActivedPlayer != 1)
                GameManager.instance.ChangeCameraTarget(_players[newActivedPlayer]);
            else
                GameManager.instance.ChangeCameraTarget(_player2Centre);
        }

        public void BackPlayer1()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            TakeBeforePosition();
            _players[0].gameObject.SetActive(true);
            _players[0].position = _oldpos;
            activePlayer = 0;
            GameManager.instance.ChangeCameraTarget(_players[0]);
        }

        void StopBeforePlayer()
        {
            MainPlayer mainPlayer;
            if (activePlayer != 1)
                mainPlayer = _players[activePlayer].GetComponent<MainPlayer>();
            else
                mainPlayer = _player2Centre.GetComponent<MainPlayer>();
            mainPlayer.StateMachine.ChangeState(mainPlayer.PlayerNotMoveState);
        }

        void TweenAnimation(int newPlayer, int oldPlayer)
        {
            canChangePlayer = false;
            CheckOpenStoveOrNitro();
            Sequence q = DOTween.Sequence();
            float a = 1;
            switch (oldPlayer)
            {
                case 0:
                    q.Append(_player1Sr.DOFade(0, opacityDuration)
                        .SetEase(Ease.InBounce)
                        .OnComplete(() => FinishOpacityActivePlayer(_players[0].gameObject)));
                    break;
                case 1:
                    q.Append(DOTween.To(() => a, x => SetOpacityPlayer2(x), 0, opacityDuration)
                        .SetEase(Ease.InOutCubic)
                        .OnComplete(() => FinishOpacityActivePlayer(_players[1].gameObject)));
                    break;
                case 2:
                    q.Append(DOTween.To(() => a, x => SetOpacityPlayer3(x), 0, opacityDuration)
                        .SetEase(Ease.OutElastic)
                        .OnComplete(() => FinishOpacityActivePlayer(_players[2].gameObject)));
                    break;
            }

            float b = 0;
            switch (newPlayer)
            {
                case 0:
                    q.Join(_player1Sr.DOFade(1, opacityDuration)
                        .SetEase(Ease.InOutCubic)
                        .OnComplete(() => FinishOpacityNewActivePlayer(_players[0].gameObject)));
                    break;
                case 1:
                    q.Join(DOTween.To(() => b, x => SetOpacityPlayer2(x), 1, opacityDuration)
                        .SetEase(Ease.InOutQuint)
                        .OnComplete(() => FinishOpacityNewActivePlayer(_players[1].gameObject)));
                    break;
                case 2:
                    q.Join(DOTween.To(() => b, x => SetOpacityPlayer3(x), 1, opacityDuration)
                        .SetEase(Ease.InCirc)
                        .OnComplete(() => FinishOpacityNewActivePlayer(_players[2].gameObject)));
                    break;
            }

            q.Join(transitionTx.DOFade(1, opacityDuration)
                .SetEase(Ease.OutQuart)
                .OnComplete(ResetTxFade));
        }

        void TakeBeforePosition()
        {
            if (activePlayer == 1)
                _oldpos = _player2Centre.position;
            else
                _oldpos = _players[activePlayer].position;
        }

        private void SetOpacityPlayer2(float opacity) => _player2Material.SetFloat("_Opacity", opacity);

        private void SetOpacityPlayer3(float opacity)
        {
            var main = GameObject.Find("GasParticle").GetComponent<ParticleSystem>().main;

            var startColor1 = main.startColor.colorMin;
            startColor1.a = opacity;
            main.startColor = new ParticleSystem.MinMaxGradient(startColor1, main.startColor.colorMin);

            var startColor2 = main.startColor.colorMax;
            startColor2.a = opacity;
            main.startColor = new ParticleSystem.MinMaxGradient(startColor2, main.startColor.colorMax);
        }

        void FinishOpacityActivePlayer(GameObject player) => player.SetActive(false);

        void FinishOpacityNewActivePlayer(GameObject player)
        {
            if (player.GetComponent<MainPlayer>() != null)
                player.GetComponent<MainPlayer>().BackIdle();
            else
                player.transform.GetChild(0).GetChild(0).GetComponent<MainPlayer>().BackIdle();
            canChangePlayer = true;
            GameManager.instance.eyes.OpenEyes();
            CheckCloseStoveOrNitro();
        }

        void ChangeTxPos() => transitionTx.transform.position = new Vector2(_oldpos.x, _oldpos.y + 1.5f);

        void ResetTxFade()
        {
            Color color = transitionTx.color;
            color.a = 0;
            transitionTx.color = color;
        }

        void ChangeTxText(string value) => transitionTx.text = value;

        string WriteTx(int oldPlayer, int newPlayer)
            => (oldPlayer, newPlayer) switch
            {
                (0, 1) => "Erime",
                (0, 2) => "Süblimleşme",
                (1, 0) => "Donma",
                (1, 2) => "Buharlaşma",
                (2, 0) => "Kırağılaşma",
                (2, 1) => "Yoğuşma",
                _ => "None"
            };

        void CheckOpenStoveOrNitro()
        {
            if (_isStove)
                OpenStove();
            else
                OpenNitrogenTube();
        }

        void CheckCloseStoveOrNitro()
        {
            if (_isStove)
                CloseStove();
            else
                CloseNitrogenTube();
        }

        void OpenNitrogenTube()
        {
            _nitrogenTube.transform.position = new Vector2(_oldpos.x + .55f, _oldpos.y - 2);
            _nitrogenTube.GetComponent<Animator>().Play("Appear");
            StartCoroutine(PlayDelay(nitrogenSound));
        }

        void OpenStove()
        {
            _stove.transform.position = new Vector2(_oldpos.x, _oldpos.y - 1);
            _stove.GetComponent<Animator>().Play("Appear");
            StartCoroutine(PlayDelay(fireSound));
        }

        void CloseStove() => _stove.GetComponent<Animator>().Play("Disappear");

        void CloseNitrogenTube() => _nitrogenTube.GetComponent<Animator>().Play("Disappear");

        IEnumerator PlayDelay(AudioClip clip)
        {
            yield return new WaitForSeconds(0.25f);
            PlaySound(clip);
        }


        #region Input Actions

        void OnPlayer1()
        {
            if (!canChangePlayer || activePlayer == 0) return;
            ChangePlayer(0);
            GameManager.instance.SetScreenAxis(0.25f, 0.5f);
        }

        void OnPlayer2()
        {
            if(GameManager.instance.greenPortal.currentLevel < 2) return;
            if (!canChangePlayer || activePlayer == 1) return;
            _player2 = Instantiate(player2Instance, transform);
            _player2Centre = _player2.transform.GetChild(0).GetChild(0).transform;
            _players[1] = _player2.transform;
            _player2Material = _player2.transform.GetChild(0).GetComponent<SpriteRenderer>().material;
            ChangePlayer(1);
            GameManager.instance.SetScreenAxis(0.25f, 0.5f);
        }

        void OnPlayer3()
        {
            if(GameManager.instance.greenPortal.currentLevel < 3) return;
            if (!canChangePlayer || activePlayer == 2) return;
            ChangePlayer(2);
            GameManager.instance.SetScreenAxis(0.5f, 0.75f);
        }

        #endregion
    }
}