using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Scipts.PlatformButton
{
    public class PlatformButton : MonoBehaviour
    {
        [SerializeField] private Transform platform;
        [SerializeField] private GameObject tilemap;
        [SerializeField] private GameObject walls;
        [SerializeField] private GameObject nitrogenBarrel;
        [SerializeField] private Transform nitrogenBarrelPos;
        private SpriteRenderer _sr;
        private BoxCollider2D _bx;
        private BoxCollider2D _platformCollider;

        private void Awake()
        {
            _bx = GetComponent<BoxCollider2D>();
            _sr = GetComponentInChildren<SpriteRenderer>();
            _platformCollider = platform.GetComponent<BoxCollider2D>();
        }


        void PressedButton(bool active)
        {
            _bx.enabled = active;
            _platformCollider.enabled = active;
            float zAxis= !active ? -180 :0f;
            platform.DORotate(new Vector3(0, 0, zAxis), 0.25f)
                .SetEase(Ease.Linear);
        }

        void SetActiveObjects(bool active)
        {
            _sr.enabled = active;
            tilemap.SetActive(active);
            walls.SetActive(active);
            if (active)
                platform.position = new Vector2(Random.Range(-4f, 6f), platform.position.y);
        }


        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                PressedButton(false);
                SetActiveObjects(false);
                StartCoroutine(SetCoolDown());
            }
        }

        IEnumerator SetCoolDown()
        {
            yield return new WaitForSeconds(10f);
            SetActiveObjects(true);
            PressedButton(true);
           GameObject barrel= Instantiate(nitrogenBarrel, nitrogenBarrelPos);
           barrel.transform.localPosition=Vector3.zero;
        }
    }
}
