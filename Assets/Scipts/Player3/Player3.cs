using System;
using DG.Tweening;
using Scipts.AllPlayers;
using UnityEngine;
using UnityEngine.UI;

namespace Scipts.Player3
{
    public class Player3 : MainPlayer
    {
        [Header("Player3 Properties")] [SerializeField]
        private float flyingAccel = .5f;

        [SerializeField] private float flyingSpeed = .75f;
        public float startInsideFlyTimer = 3f;
        public float outsideDuration = 2.5f;

        [Header("Object Check")] [SerializeField]
        private float detectRadius = 1f;

        [SerializeField] private LayerMask objectLayer;
        [Header("Particles")] [SerializeField] private ParticleSystem gasParticle;
        [SerializeField] private ParticleSystem insideParticle;
        private BoxCollider2D _bx;
        [HideInInspector] public GameObject collidingObject;
        private GameObject _beforeCollidingObject;
        public Action Interact;
        private Rigidbody2D _rigidbody;
        [HideInInspector] public Players players;
        [SerializeField] private SpriteRenderer sr;
        [HideInInspector] public BoxCollider2D eyeCollider;
        [SerializeField] private Image healthBar;
        [HideInInspector] public float health = 100f;
        private float _healthDecreaseAmount;
        public float ıdleDecreaseAmount = 2.5f;
        public float insideDecreaseAmount = 1.5f;
        [HideInInspector] public bool isTurning;

        #region Player3 States

        public Player3IdleState IdleState { get; private set; }
        public Player3InsideState InsideState { get; private set; }
        public Player3OutsideState OutsideState { get; private set; }

        #endregion

        [Header("Sounds")] public AudioClip gasInside;
        public AudioClip gasOutside;
        [HideInInspector] public bool isBeforeInside;


        protected override void Awake()
        {
            base.Awake();
            _bx = GetComponent<BoxCollider2D>();
            players = transform.parent.GetComponent<Players>();
            eyeCollider = Eye.GetComponent<BoxCollider2D>();
            IdleState = new Player3IdleState(this, StateMachine, "Idle State", this);
            InsideState = new Player3InsideState(this, StateMachine, "Inside State", this);
            OutsideState = new Player3OutsideState(this, StateMachine, "Outside State", this);
        }

        protected override void Start()
        {
            base.Start();
            insideParticle.gameObject.SetActive(false);
            isTurning = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;
            rb.bodyType = RigidbodyType2D.Dynamic;
            _bx.isTrigger = false;
            healthBar.transform.parent.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (collidingObject != null)
                collidingObject.transform.GetChild(0).gameObject.SetActive(false);
            healthBar.transform.parent.gameObject.SetActive(false);
            collidingObject = null;
        }

        public override void BackIdle()
        {
            base.BackIdle();
            if (collidingObject != null)
            {
                isBeforeInside = true;
               StateMachine.ChangeState(InsideState);
            }
            else StateMachine.ChangeState(IdleState);
        }

        public void DecreaseHealth()
        {
            health -= _healthDecreaseAmount * Time.deltaTime;
            healthBar.fillAmount = health / 100;
            if (health <= 0)
            {
                if (!gasParticle.isPlaying) Outside();
                StateMachine.ChangeState(PlayerDieState);
                health = 100;
            }
        }

        public void SetDecreaseAmount(float value) => _healthDecreaseAmount = value;

        private void FixedUpdate()
        {
            StateMachine.CurrentState.UpdateState();
        }

        public override void Die()
        {
            base.Die();
            rb.bodyType = RigidbodyType2D.Kinematic;
            _bx.isTrigger = true;
        }

        #region Player3 Input Actions

        void OnInteract()
        {
            Interact?.Invoke();
        }

        #endregion

        public override void Movement()
        {
            base.Movement();
            rb.velocity = new Vector2(rb.velocity.x,
                Mathf.Lerp(rb.velocity.y, flyingSpeed, flyingAccel * Time.fixedDeltaTime));
        }

        public void MovementObj()
        {
            _rigidbody.velocity = new Vector2(
                Mathf.Lerp(_rigidbody.velocity.x, axis * speed, accel * Time.fixedDeltaTime)
                , Mathf.Lerp(rb.velocity.y, flyingSpeed, flyingAccel * Time.fixedDeltaTime));
        }

        public void Inside()
        {
            gasParticle.transform.parent = null;
            gasParticle.Stop();
            //transform.position = collidingObject.transform.position;
            eyeCollider.isTrigger = true;
            _bx.isTrigger = true;
            transform.DOMove(collidingObject.transform.position, 1f)
                .OnComplete((() =>
                {
                    eyeCollider.isTrigger = false;
                    _bx.isTrigger = false;
                    insideParticle.gameObject.SetActive(true);
                    insideParticle.transform.parent = collidingObject.transform;
                    insideParticle.transform.localPosition = Vector2.zero;
                }));
            _rigidbody = collidingObject.GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            rb.velocity = Vector2.zero;
            collidingObject.transform.GetChild(0).gameObject.SetActive(false);
            Color color = collidingObject.GetComponentInChildren<SpriteRenderer>().color;
            color.a = .5f;
            collidingObject.GetComponentInChildren<SpriteRenderer>().color = color;
            insideParticle.transform.localScale = Vector3.one;
            collidingObject.GetComponent<BoxCollider2D>().enabled = false;
            collidingObject.GetComponent<EdgeCollider2D>().enabled = true;
            GameManager.instance.ChangeCameraTarget(collidingObject.transform);
        }

        public void Outside()
        {
            transform.position = collidingObject.transform.position;
            gasParticle.transform.parent = transform;
            insideParticle.transform.parent = transform;
            gasParticle.transform.position = transform.position;
            insideParticle.transform.position = transform.position;
            gasParticle.Play();
            insideParticle.gameObject.SetActive(false);
            _rigidbody.gravityScale = 1;
            rb.velocity = Vector2.zero;
            Color color = collidingObject.GetComponentInChildren<SpriteRenderer>().color;
            color.a = 1f;
            collidingObject.GetComponentInChildren<SpriteRenderer>().color = color;
            collidingObject.GetComponent<BoxCollider2D>().enabled = true;
            collidingObject.GetComponent<EdgeCollider2D>().enabled = false;
            collidingObject = null;
            GameManager.instance.ChangeCameraTarget(transform);
        }


        public bool ObjectCheck()
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, detectRadius, objectLayer);

            if (hit.Length != 0)
            {
                collidingObject = CalculateCloserHit(hit).gameObject;
                collidingObject.transform.GetChild(0).gameObject.SetActive(true);
                if (_beforeCollidingObject == null || _beforeCollidingObject != collidingObject)
                {
                    if (_beforeCollidingObject != null)
                        _beforeCollidingObject.transform.GetChild(0).gameObject.SetActive(false);
                    _beforeCollidingObject = collidingObject;
                }

                return true;
            }

            if (collidingObject != null)
            {
                collidingObject.transform.GetChild(0).gameObject.SetActive(false);
                collidingObject = null;
            }

            return false;
        }

        Collider2D CalculateCloserHit(Collider2D[] hit)
        {
            int hitObjectsCount = hit.Length;
            if (hitObjectsCount == 1) return hit[0];

            float distance = Vector2.Distance(transform.position, hit[0].transform.position);
            Collider2D returnCollider = hit[0];

            for (int i = 1; i < hit.Length; i++)
            {
                float newDistance = Vector2.Distance(transform.position, hit[i].transform.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    returnCollider = hit[i];
                }
            }

            return returnCollider;
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (StateMachine.CurrentState != InsideState)
                base.OnTriggerEnter2D(col);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, detectRadius);
        }
    }
}