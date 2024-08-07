using System;
using UnityEngine;

namespace Scipts.NitrogenBarrel
{
    public class NitrogenBarrel : MonoBehaviour
    {
        [SerializeField] private GameObject explosionParticleInstance;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundLayer;
        private bool _isExplosion;
        private BoxCollider2D _bx;

        private void Awake()
        {
            _bx = GetComponent<BoxCollider2D>();
        }


        private void Update()
        {
            if (_bx.enabled)
                CheckExplosion();
        }


        void CheckExplosion()
        {
            RaycastHit2D hit = GroundCheck();
            if (hit.collider != null && !_isExplosion) Explosion(hit);
        }

        void Explosion(RaycastHit2D hit)
        {
            _isExplosion = true;
            if(hit.collider.CompareTag("Boss"))
               hit.collider.GetComponent<Boss.Boss>().TakeDamage();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Instantiate(explosionParticleInstance, transform);
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 2f);
        }

        RaycastHit2D GroundCheck() => Physics2D.BoxCast(groundCheck.position, Vector2.one, 0, Vector2.down,
            groundDistance, groundLayer);


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheck.position, Vector3.one);
        }
    }
}