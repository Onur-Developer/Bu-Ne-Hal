using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scipts.Boss
{
    public class Boss : MonoBehaviour
    {
        #region States

        public BossStateMachine StateMachine { get; private set; }
        public BossIdleState IdleStateState { get; private set; }
        public BossMoveState MoveStateState { get; private set; }

        public BossTakeDamageState TakeDamageState { get; private set; }

        public BossFlameAttackState FlameAttackState { get; private set; }

        public BossFlyState FlyState { get; private set; }

        public BossDieState DieState { get; private set; }

        #endregion

        #region Editor Values

        [Header("Default Properties")] public float movementSpeed;
        public float idleSpeed;
        public float shakingTime;
        [Header("Flames")] public float flameAttackTime;
        [SerializeField] private GameObject[] flames;
        [SerializeField] private GameObject purplePortal;

        [Header("UI")] [SerializeField] private Image bossFill;

        [Header("Fly Mode")] public Sprite defaultSprite;
        public Sprite flySprite;
        public float flyTimer;
        [SerializeField] private GameObject saw;
        [SerializeField] private Transform saw1Pos;
        [SerializeField] private Transform saw2Pos;
        private GameObject[] saws = new GameObject[2];
        private int _attackStateCount = 1;
        public int playerDieCount;
        private bool _isPlayerDie;
        [SerializeField] private GameObject portalButton;

        #endregion

        #region Wall Check

        [Header("Wall Properties")] [SerializeField]
        private Transform wallcheck;

        [SerializeField] private float wallDistance;
        [SerializeField] private LayerMask wallMask;

        #endregion

        #region Private Variables

        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        private float _movementX = 1.0f;
        private int _health = 50;

        #endregion

        [Header("Sound")] private AudioSource _au;
        public AudioClip moveSound;
        public AudioClip takeDamageSound;
        public AudioClip flySound;
        public AudioClip drawGunSound;

        private void Awake()
        {
            StateMachine = new BossStateMachine();
            IdleStateState = new BossIdleState(this, StateMachine);
            MoveStateState = new BossMoveState(this, StateMachine);
            TakeDamageState = new BossTakeDamageState(this, StateMachine, GetComponentInChildren<Animator>());
            FlameAttackState = new BossFlameAttackState(this, StateMachine);
            FlyState = new BossFlyState(this, StateMachine);
            DieState = new BossDieState(this, StateMachine);
            _sr = GetComponentInChildren<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _au = GetComponent<AudioSource>();
        }

        private void Start()
        {
            StateMachine.Initialize(IdleStateState);
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.UpdateState();
            if (PlayerDied() && !_isPlayerDie) PlayerDieListener();
        }

        public void PlaySound(AudioClip clip)
        {
            _au.clip = clip;
            _au.Play();
        }

        void PlayerDieListener()
        {
            _isPlayerDie = true;
            playerDieCount++;
            StartCoroutine(WaitListener());
        }

        public void LookWall()
        {
            if (WallCheck())
            {
                _movementX *= -1;
                _sr.flipX = !_sr.flipX;
            }
        }

        public void LookPlayer()
        {
            GameObject target = GameObject.FindWithTag("Player");
            if (target == null) return;
            float Xaxis = target.transform.position.x > transform.position.x ? 1 : -1;
            _movementX = Xaxis;
            _sr.flipX = Xaxis < 0;
        }

        public void Move(float comingSpeed)
        {
            _rb.velocity = new Vector2(_movementX * comingSpeed, _rb.velocity.y);
        }

        public void SetFlyMode(bool flyMode)
        {
            if (!flyMode)
            {
                _sr.sprite = defaultSprite;
                _rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                _sr.sprite = flySprite;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                transform.DOMove(new Vector2(0, -1), 2f)
                    .SetEase(Ease.OutQuart);
            }
        }

        public void CreateSaws()
        {
            GameObject saww = Instantiate(saw, saw1Pos);
            saws[0] = saww;
            GameObject saww2 = Instantiate(saw, saw2Pos);
            saws[1] = saww2;
        }

        public void DestroySaws()
        {
            foreach (GameObject s in saws)
            {
                Destroy(s);
            }
        }

        public void ResetVelocity() => _rb.velocity = Vector2.zero;

        public void TakeDamage()
        {
            _health -= 10;
            bossFill.fillAmount -= 0.2f;
            StateMachine.ChangeState(TakeDamageState);
            if (_health == 20 || _health == 30)
                _attackStateCount++;
            if (_health <= 0)
                Die();
        }

        void Die()
        {
            StateMachine.ChangeState(DieState);
            int dieCount = Mathf.Max(0, 3 - playerDieCount);
            if (dieCount != 0)
            {
                for (int i = 0; i < dieCount; i++)
                {
                    BadgeManager.BadgeManager.instance.CollectHalfStars();
                    BadgeManager.BadgeManager.instance.CollectAllStars();
                    GameManager.instance.IncreaseStar();
                }
            }

            portalButton.SetActive(true);
            BadgeManager.BadgeManager.instance.DefeatBoss();
            Destroy(gameObject, 1f);
        }

        public bool PlayerDied() => purplePortal.activeSelf;

        public int RandomValue() => Random.Range(0, _attackStateCount);

        public void SetFlames(bool active)
        {
            if (active)
                foreach (GameObject a in flames)
                {
                    a.SetActive(active);
                }


            Sequence q = DOTween.Sequence();
            if (active)
            {
                q.Append(flames[0].transform.DOScale(0, 1f)
                    .SetEase(Ease.OutBounce)
                    .From());
                q.Join(flames[1].transform.DOScale(0, 1f)
                    .SetEase(Ease.OutBounce)
                    .From());
            }
            else
            {
                q.Append(flames[0].transform.DOScale(0, 0.1f)
                    .SetEase(Ease.InBounce));
                q.Join(flames[1].transform.DOScale(0, 0.1f)
                        .SetEase(Ease.InBounce))
                    .OnComplete(() =>
                    {
                        flames[0].transform.localScale = Vector3.one;
                        flames[0].transform.GetChild(0).transform.localScale = new Vector2(0.25f, 0.25f);
                        flames[0].SetActive(false);
                        flames[1].transform.localScale = Vector3.one;
                        flames[1].transform.GetChild(0).transform.localScale = new Vector2(0.25f, 0.25f);
                        flames[1].SetActive(false);
                    });
            }
        }

        public bool WallCheck() => Physics2D.Raycast(wallcheck.position, Vector2.right * _movementX,
            wallDistance, wallMask);


        IEnumerator WaitListener()
        {
            yield return new WaitForSeconds(7f);
            _isPlayerDie = false;
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(wallcheck.position,
                new Vector2(wallcheck.position.x + wallDistance * _movementX, wallcheck.position.y));
        }
    }
}