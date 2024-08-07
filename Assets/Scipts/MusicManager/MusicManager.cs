using System;
using UnityEngine;

namespace Scipts.MusicManager
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;
        [SerializeField] private AudioClip[] musics;
        private AudioSource _au;
        private int _currentMusic;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance!=this)
                Destroy(gameObject);

            _au = GetComponent<AudioSource>();
        }

        private void Start()
        {
            PlayMusic();
        }

        private void Update()
        {
            if (!_au.isPlaying)  PlayMusic();
        }


        void PlayMusic()
        {
            _currentMusic++;
            _currentMusic = _currentMusic >= musics.Length ? 0 : _currentMusic;
            _au.clip = musics[_currentMusic];
            _au.Play();
        }
    }
}
