using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scipts.FlameMachine
{
    public class FlameMachine : MonoBehaviour
    {
        [SerializeField] private float minAttackTime;
        [SerializeField] private float maxAttackTime;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject fireBulletInstance;
        [SerializeField] private float fireForce;
        [SerializeField] private bool isRotate;
        private Animator _animator;
        private SpriteRenderer _sr;
        private float _firePositionX;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (!isRotate)
                SetFirePoint();
            StartCoroutine(Attack());
        }

        private void Update()
        {
            if (isRotate) LookPlayer();
        }

        void LookPlayer()
        {
            GameObject target = GameObject.FindWithTag("Player");
            if (target == null) return;
            Vector3 lookDirection = target.transform.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        void SetFirePoint()
        {
            float posZ = _sr.flipX ? -.45f : .45f;
            firePoint.localPosition = new Vector2(posZ, 0);
            _firePositionX = _sr.flipX ? -1f : 1f;
        }

        public void Fire()
        {
            if (isRotate) GetComponent<AudioSource>().Play();
            GameObject fireBullet = Instantiate(fireBulletInstance, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
            if (!isRotate)
                rb.AddForce(Vector2.right * _firePositionX * fireForce, ForceMode2D.Impulse);
            else
                rb.AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
            Destroy(fireBullet, 3f);
        }


        IEnumerator Attack()
        {
            float attackWaitTime = Random.Range(minAttackTime, maxAttackTime);
            yield return new WaitForSeconds(attackWaitTime);
            if (_sr.enabled)
                _animator.SetTrigger("Attack");
            StartCoroutine(Attack());
        }
    }
}