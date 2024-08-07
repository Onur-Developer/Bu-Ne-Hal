using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scipts.MenuManager
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;
        [SerializeField] private Button[] iconButtons;
        [SerializeField] private string noiseVideoPath;
        [SerializeField] private string catVideoPath;
        [SerializeField] private VideoPlayer noiseVideoPlayer;
        [SerializeField] private VideoPlayer catVideoPlayer;
        [SerializeField] private Settings.Settings settings;


        private void Start()
        {
            settings.SetValues();
            AttachVideos();
        }

        void AttachVideos()
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, noiseVideoPath);
            noiseVideoPlayer.url = videoPath;
            string videoPath2 = System.IO.Path.Combine(Application.streamingAssetsPath, catVideoPath);
            catVideoPlayer.url = videoPath2;
        }

        public void IconButon(int index)
        {
            if (panels[index].activeSelf) return;

            foreach (GameObject obj in panels)
            {
                obj.SetActive(false);
            }

            StartCoroutine(ChangePanel(index));
        }

        public void LevelButton(int index) => SceneManager.LoadScene("Level" + index);

        public void ExitButton() => Application.Quit();

        void SetInteractible(bool value)
        {
            foreach (Button button in iconButtons)
            {
                button.interactable = value;
            }
        }

        void PlayNoiseVideo()
        {
            panels[7].SetActive(true);
            noiseVideoPlayer.Play();
        }


        IEnumerator ChangePanel(int index)
        {
            PlayNoiseVideo();
            SetInteractible(false);
            yield return new WaitForSeconds(1f);
            SetInteractible(true);
            panels[index].SetActive(true);
            panels[7].SetActive(false);
            if (index == 0)
                panels[6].SetActive(true);
            if (index == 5)
                StartCoroutine(CatPanel());
        }

        IEnumerator CatPanel()
        {
            catVideoPlayer.Play();
            SetInteractible(false);
            yield return new WaitForSeconds(6.5f);
            if (panels[5].activeSelf)
                IconButon(0);
            SetInteractible(true);
        }
    }
}