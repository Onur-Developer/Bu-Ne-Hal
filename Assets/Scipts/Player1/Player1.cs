using System;
using System.Collections;
using Scipts.AllPlayers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scipts.Player1
{
    public class Player1 : MainPlayer
    {
        [Header("Player 1 Properties")] [SerializeField]
        private float dashForceX = 25.0f;

        [SerializeField] private float dashForceY = 25.0f;

        [Header("Dash Properties")] [SerializeField]
        private float dashCoolDownTimer = 3.0f;

        public float dashTime = .5f;
        [HideInInspector] public bool isDash;
        private float _dashXaxis;
        private float _dashYaxis;
        private bool _isGhost = true;
        [SerializeField] private GameObject dashGhostInstance;
        [SerializeField] private Sprite defaultEye;
        [SerializeField] private Sprite tiredEye;

        [Header("Breakable Object Properties")] [SerializeField]
        private Transform boxcastPos;

        [SerializeField] private LayerMask breakableMask;
        [SerializeField] private GameObject fallParticleInstance;
        [Header("Sounds")] [SerializeField] private AudioClip fallingSound;
        public AudioClip dash;

        #region Player1 States

        public Player1DashState DashState { get; private set; }
        public Player1IdleState IdleState { get; private set; }
        public Player1MoveState MoveState { get; private set; }

        public Player1JumpState JumpState { get; private set; }

        public Player1FallState FallState { get; private set; }

        #endregion


        protected override void Awake()
        {
            base.Awake();
            DashState = new Player1DashState(this, StateMachine, "Dash State", this);
            IdleState = new Player1IdleState(this, StateMachine, "Idle State", this);
            MoveState = new Player1MoveState(this, StateMachine, "Move State", this);
            JumpState = new Player1JumpState(this, StateMachine, "Jump State", this);
            FallState = new Player1FallState(this, StateMachine, "Fall State", this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.ChangeState(IdleState);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Eye.transform.localScale = new Vector3(.3f, .3f, 1f);
            isDash = false;
        }

        private void OnDisable()
        {
            Eye.sprite = defaultEye;
            Eye.GetComponent<Animator>().enabled = true;
        }

        public override void BackIdle()
        {
            base.BackIdle();
            StateMachine.ChangeState(IdleState);
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.UpdateState();
        }

        public void Dash()
        {
            isDash = true;
            float dashX = _dashXaxis != 0 ? _dashXaxis : 0;
            float dashY = _dashYaxis != 0 ? _dashYaxis : 0;
            rb.AddForce(new Vector2(dashX * dashForceX, dashY * dashForceY), ForceMode2D.Impulse);
            StartCoroutine(DashCoolDown());
        }

        public void StartCreateGhost()
        {
            _isGhost = true;
            StartCoroutine(StartGhostSpawn());
        }

        public void StopCreateGhost() => _isGhost = false;

        public void CreateFallParticle()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y - 5f);
            GameObject fallParticle = Instantiate(fallParticleInstance, position, Quaternion.identity);
            var main1 = fallParticle.transform.GetChild(1).GetComponent<ParticleSystem>().main;
            var main2 = fallParticle.transform.GetChild(2).GetComponent<ParticleSystem>().main;
            float mainSize = Mathf.Abs(rb.velocity.y / 150);
            main1.startSize = mainSize;
            main2.startSize = mainSize;
            GameManager.instance.SetCameraShake(0.35f, 15, mainSize * 10);
            PlaySoundEffect(fallingSound);
            Destroy(fallParticle, 1);
        }

        public Collider2D BreakControl() =>
            Physics2D.OverlapBox(boxcastPos.position, Vector2.one * 2f, 0, breakableMask);

        void DashControl()
        {
            if (isDash || StateMachine.CurrentState == PlayerNotMoveState
                       || StateMachine.CurrentState == PlayerDieState) return;
            StateMachine.ChangeState(DashState);
        }

        public override void Die()
        {
            base.Die();
            DieEffect();
        }


        public void DieEffect()
        {
            if (StateMachine.CurrentState != PlayerDieState) StateMachine.ChangeState(PlayerDieState);
            rb.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        public override void Respawn()
        {
            base.Respawn();
            rb.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<CircleCollider2D>().isTrigger = false;
            StateMachine.ChangeState(IdleState);
        }


        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawCube(boxcastPos.position, Vector3.one * 2f);
        }


        IEnumerator DashCoolDown()
        {
            Eye.sprite = tiredEye;
            Eye.GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(dashCoolDownTimer);
            Eye.GetComponent<Animator>().enabled = true;
            Eye.sprite = defaultEye;
            isDash = false;
        }

        IEnumerator StartGhostSpawn()
        {
            GameObject dashGhost = Instantiate(dashGhostInstance, transform.position, transform.rotation);
            Sprite eyeSprite = GameObject.FindWithTag("Eye").GetComponent<SpriteRenderer>().sprite;
            dashGhost.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = eyeSprite;
            yield return new WaitForSeconds(0.035f);
            if (_isGhost)
                StartCoroutine(StartGhostSpawn());
        }

        #region Player1 Input Actions

        protected override void OnMove(InputValue value)
        {
            base.OnMove(value);
            _dashXaxis = axis;
        }

        void OnDash() => DashControl();

        void OnVerticalAxis(InputValue value)
        {
            float axisY = value.Get<float>();
            _dashYaxis = axisY;
        }

        #endregion
    }
}