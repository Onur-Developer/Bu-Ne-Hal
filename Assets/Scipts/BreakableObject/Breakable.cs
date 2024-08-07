using System.Collections;
using UnityEngine;

namespace Scipts.BreakableObject
{
    public class Breakable : MonoBehaviour
    {
        private BoxCollider2D _bx;
        [SerializeField] private float destroyTime=.5f;
        [SerializeField] private GameObject breakableParticleInstance;
        private SpriteRenderer _sr;
        [SerializeField] private AudioSource au;
        
        private void Awake()
        {
            _bx = GetComponent<BoxCollider2D>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        }


       public void Break()
        {
            _bx.enabled = false;
            _sr.enabled = false;
            Instantiate(breakableParticleInstance, transform);
            au.Play();
            StartCoroutine(Destroy());
        }

        IEnumerator Destroy()
        {
            BadgeManager.BadgeManager.instance.SolidMaster();
            yield return new WaitForSeconds(destroyTime);
            Destroy(gameObject);
        }
    }
}
