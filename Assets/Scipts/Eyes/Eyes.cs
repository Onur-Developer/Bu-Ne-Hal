using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scipts.Eyes
{
    public class Eyes : MonoBehaviour
    {
        [SerializeField] private float minBlinkTime;
        [SerializeField] private float maxBlinkTime;
        [SerializeField] private Sprite defaultEye;
        private SpriteRenderer _sr;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _sr.sprite = defaultEye;
        }

        private void Start()
        {
            StartCoroutine(BlinkAnimation());
        }

        public void CloseEyes() => _animator.Play("CloseEyes");


        public void OpenEyes() => _animator.Play("OpenEyes");

        private IEnumerator BlinkAnimation()
        {
            float waitTime = Random.Range(minBlinkTime, maxBlinkTime);
            yield return new WaitForSeconds(waitTime);
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
                _animator.SetTrigger("blink");
            StartCoroutine(BlinkAnimation());
        }
    }
}