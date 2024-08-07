using System;
using System.Collections;
using Scipts.AllPlayers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scipts.Player2
{
    public class Player2 : MainPlayer
    {
        [Header("Wall Check Properties")] [SerializeField]
        private Transform wallCheck;

        [SerializeField] private float wallDistance;
        [SerializeField] private LayerMask wallLayer;
        [Header("Player 2 Properties")]
        [SerializeField] private float slideJump;
        [SerializeField] private float baseGravity = 1f;
        [SerializeField] private float slideGravity = .25f;
        private float _rotAxis = 1.0f;
      [SerializeField]  private Rigidbody2D[] _bodies = new Rigidbody2D[13];
      [SerializeField] private CircleCollider2D[] _colliders = new CircleCollider2D[13];
        public Action SlideJumping;
        private Players _players;
        [SerializeField] private LayerMask dieCollider;

        #region Player2 States

        public Player2IdleState IdleState { get; private set; }
        public Player2MoveState MoveState { get; private set; }

        public Player2JumpState JumpState { get; private set; }

        public Player2WallSlideState WallSlideState { get; private set; }

        public Player2SlideJump SlideJump { get; private set; }

        #endregion

        [Header("Sounds")] public AudioClip jump;


        protected override void Awake()
        {
            base.Awake();
            _players = GameObject.FindWithTag("Players").GetComponent<Players>();
            IdleState = new Player2IdleState(this, StateMachine, "Idle State", this);
            MoveState = new Player2MoveState(this, StateMachine, "Move State", this);
            JumpState = new Player2JumpState(this, StateMachine, "Jump State", this);
            WallSlideState = new Player2WallSlideState(this, StateMachine, "Wall Slide State", this);
            SlideJump = new Player2SlideJump(this, StateMachine, "Slide Jump", this);
        }

        protected override void Start()
        {
            base.Start();
            SetGravity(baseGravity);
            StartCoroutine(BackdDynamic(_players.opacityDuration));
        }

        private void OnDisable()
        {
            if ( _colliders[0].isTrigger)
                _players.BackPlayer1();
            Destroy(transform.parent.parent.gameObject,1f);
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


        public bool WallCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right
                                                                         * _rotAxis, wallDistance, wallLayer);
        
        public bool DieCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right
                                                                        * _rotAxis, wallDistance, dieCollider);

        public void InSliding()
        {
            rb.velocity = Vector2.zero;
            SetGravity(slideGravity);
        }

        public void OutSliding() => SetGravity(baseGravity);

        void SetGravity(float gravity)
        {
            for (int i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].gravityScale = gravity;
            }
        }

        void SetBodyType(RigidbodyType2D rigidbodyType2D,bool colliderSituation)
        {
            for (int i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].bodyType = rigidbodyType2D;
                _colliders[i].isTrigger = colliderSituation;
            }
        }

        public void MakeSlideJump()
        {
            axis *= -1;
            _rotAxis = axis;
            rb.velocity = new Vector2(rb.velocity.x, slideJump);
        }

        public override void Die()
        {
            base.Die();
            SetGravity(0);
            SetBodyType(RigidbodyType2D.Dynamic,true);
        }


        #region Player2 Input Actions

        protected override void OnMove(InputValue value)
        {
            if (WallCheck() && !GroundCheck())
            {
                axis = _rotAxis;
                return;
            }

            base.OnMove(value);
            _rotAxis = axis != 0 ? axis : _rotAxis;
        }

        void OnSlideJump()
        {
            SlideJumping?.Invoke();
        }

        #endregion

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheck.position,
                new Vector2(wallCheck.position.x + wallDistance * _rotAxis, wallCheck.position.y));
        }

        IEnumerator BackdDynamic(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            for (int i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}