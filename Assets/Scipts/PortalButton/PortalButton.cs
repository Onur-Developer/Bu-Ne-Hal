using System;
using System.Collections;
using Scipts.AllPlayers;
using UnityEngine;

namespace Scipts.PortalButton
{
    public class PortalButton : MonoBehaviour
    {
        private Animator _animator;
        private CircleCollider2D _collider2D;
        private AudioSource _au;
        [SerializeField] private AudioClip portalOpenSound;
        [SerializeField] private AudioClip buttonClickSound;

        private void Awake()
        {
            _animator = GameObject.FindWithTag("GreenPortal").GetComponentInChildren<Animator>();
            _collider2D = GameObject.FindWithTag("GreenPortal").GetComponent<CircleCollider2D>();
            _au = GetComponent<AudioSource>();
        }


        private void Start()
        {
            GameManager.instance.directionArrow.target = transform;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponentInChildren<Animator>().Play("PortalOpen");
            _collider2D.enabled = true;
            _au.clip = buttonClickSound;
            _au.Play();
            StartCoroutine(PortalOpening());
        }

        IEnumerator PortalOpening()
        {
            GameManager.instance.ChangeCameraTarget(_animator.transform);
            MainPlayer mainPlayer = GameObject.FindWithTag("Player").GetComponent<MainPlayer>();
            mainPlayer.StateMachine.ChangeState(mainPlayer.PlayerNotMoveState);
            yield return new WaitForSeconds(1.5f);
            _animator.SetTrigger("Open");
            _au.clip = portalOpenSound;
            _au.Play();
            yield return new WaitForSeconds(1.5f);
            GameManager.instance.ChangeCameraTarget(mainPlayer.transform);
            mainPlayer.BackIdle();
            GameManager.instance.directionArrow.target = _animator.transform;
        }
    }
}
