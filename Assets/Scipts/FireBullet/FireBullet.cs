using UnityEngine;

namespace Scipts.FireBullet
{
    public class FireBullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem flashParticle;
        [SerializeField] private ParticleSystem smokeParticle;
        [SerializeField] private ParticleSystem sparkParticle;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }


        void Explosion()
        {
            flashParticle.Play();
            smokeParticle.Stop();
            sparkParticle.Stop();
            rb.simulated = false;
            GetComponent<AudioSource>().Play();
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player")) Explosion();
        }
    }
}
