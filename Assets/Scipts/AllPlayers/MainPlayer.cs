using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scipts.AllPlayers
{
    public class MainPlayer : MonoBehaviour
    {
        #region Main Player Properties

        [Header("Main Player Properties")] [SerializeField]
        protected float speed = 3.5f;

        [SerializeField] protected float accel = 20.0f;
        [SerializeField] private float deccel = 20.0f;
        [SerializeField] private float jumpForce;

        [Header("Ground Check Properties")] [SerializeField]
        protected Transform groundcheck;

        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected float groundDistance;

        [Header("Camera Settings")] private CinemachineBrain _brain;
        [SerializeField] private UpdateType updateType;

        #endregion

        #region Protected Properties

        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public float axis;
        [HideInInspector] public bool isJump;
        [HideInInspector] public bool isDied;
        protected SpriteRenderer Eye;

        #endregion

        #region Player States

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerDieState PlayerDieState { get; private set; }

        public PlayerNotMoveState PlayerNotMoveState { get; private set; }

        #endregion

        protected GameObject PurplePortal;
        private AudioSource _au;

        private enum UpdateType
        {
            LateUpdate,
            FixedUpdate
        }

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            Eye = GameObject.FindWithTag("Eye").GetComponent<SpriteRenderer>();
            _au = GetComponent<AudioSource>();
            StateMachine = new PlayerStateMachine();
            PlayerDieState = new PlayerDieState(this, StateMachine, "Player Die");
            PlayerNotMoveState = new PlayerNotMoveState(this, StateMachine, "Player Not Move State");
        }

        protected virtual void Start()
        {
            PurplePortal = GameManager.instance.purplePortal;
            _brain = GameManager.instance.brain;
        }

        protected virtual void OnEnable()
        {
            StateMachine.Inıtıalize(PlayerNotMoveState);
            Eye.transform.parent = transform;
            Eye.transform.localPosition = Vector2.zero;
            var brainControl = GameManager.instance?.brain;
            if (brainControl == null) return;
            _brain = GameManager.instance.brain;
            if (updateType == UpdateType.FixedUpdate)
                _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
            else
                _brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
        }


        public virtual void Movement()
        {
            if (axis != 0)
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, axis * speed, accel * Time.fixedDeltaTime),
                    rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0.0f, deccel * Time.fixedDeltaTime), rb.velocity.y);
        }

        public virtual void Die()
        {
            GameManager.instance.players.canChangePlayer = false;
            PurplePortal.SetActive(true);
            isDied = true;
            GameManager.instance.player3.isDied = true;
        }

        public virtual void Respawn()
        {
            GameManager.instance.players.canChangePlayer = true;
        }

        public virtual void BackIdle()
        {
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            _au.clip = clip;
            _au.Play();
        }

        public void Win() => StateMachine.ChangeState(PlayerNotMoveState);


        public void Jumping() => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        public bool GroundCheck() => Physics2D.Raycast(groundcheck.position, Vector2.down, groundDistance, groundLayer);

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(groundcheck.position, new Vector2(groundcheck.position.x, groundcheck.position.y -
                groundDistance));
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("DieCollider"))
                StateMachine.ChangeState(PlayerDieState);
            else if (col.CompareTag("SecretPlace"))
                BadgeManager.BadgeManager.instance.DetectiveLiquid();
        }


        #region Input Actions

        protected virtual void OnMove(InputValue value) => axis = value.Get<float>();

        void OnJump()
        {
            if (GroundCheck())
                isJump = true;
        }

        void OnDirectionArrow()
        {
            if (GameManager.instance.greenPortal.currentLevel != 10)
                GameManager.instance.directionArrow.ChangeActive();
        }

        void OnPause()
        {
            GameManager.instance.PauseGame();
        }

        #endregion
    }
}