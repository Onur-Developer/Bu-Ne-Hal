using UnityEngine;
using Random = UnityEngine.Random;

namespace Scipts.Tramboline
{
    public class Tramboline : MonoBehaviour
    {
        [SerializeField] private float bounce;
        [SerializeField] private float jumpDistance;
       [SerializeField] private Transform jumpObj;
       [SerializeField] private Animator anim;
       private AudioSource _au;


       private void Awake()
       {
           _au = GetComponent<AudioSource>();
       }


       bool JumpCheck() =>Physics2D.Raycast(jumpObj.position, Vector2.up, jumpDistance);

        
        

        private void OnCollisionExit2D(Collision2D col)
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null && JumpCheck())
            {
                anim.SetTrigger("Jump");
                rb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
                _au.pitch = Random.Range(0.9f, 1.1f);
                _au.Play();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color=Color.cyan;
            Gizmos.DrawLine(jumpObj.position,new Vector2(jumpObj.position.x,jumpObj.position.y+jumpDistance));
        }
    }
}