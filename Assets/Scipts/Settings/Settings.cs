using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Scipts.Settings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private AudioMixer defaultMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundEffect;


        private void Start()
        {
           SetValues();
        }

        public void SetValues()
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicValue",0.5f);
            soundEffect.value =  PlayerPrefs.GetFloat("SFXValue",0.5f);
            SetMusicMixer();
            SetSoundEffectMixer();
        }

        public void SetMusicMixer()
        {
            float sliderValue = musicSlider.value;
            defaultMixer.SetFloat("Music", Mathf.Log10(sliderValue)*20);
            PlayerPrefs.SetFloat("MusicValue",sliderValue);
        }

        public void SetSoundEffectMixer()
        {
            float sliderValue = soundEffect.value;
            defaultMixer.SetFloat("SFX", Mathf.Log10(sliderValue)*20);
            PlayerPrefs.SetFloat("SFXValue",sliderValue);
        }
    }
}
