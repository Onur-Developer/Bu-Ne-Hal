using System.Collections;
using UnityEngine;

namespace Scipts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _xAxis=1.0f;
        private bool _isMove=true;
        [SerializeField] private float speed;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private float wallDistance;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private float backNormalTime;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            ControlAxis();
            if (_isMove) Movement();
            else StopEnemy();
        }


        void ControlAxis() =>_xAxis=WallCheck() ? _xAxis*-1:_xAxis;



        void Movement() =>_rb.velocity = new Vector2(speed*_xAxis*Time.fixedDeltaTime,_rb.velocity.y);

        void StopEnemy() => _rb.velocity = Vector2.zero;


        bool WallCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right*_xAxis,
            wallDistance, wallMask);


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _isMove = false;
                StartCoroutine(BackNormal());
            }
        }

        IEnumerator BackNormal()
        {
            yield return new WaitForSeconds(backNormalTime);
            _isMove = true;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color=Color.red;
            Gizmos.DrawLine(wallCheck.position,new Vector2(wallCheck.position.x+wallDistance*_xAxis,
                wallCheck.position.y));
        }
    }
}
