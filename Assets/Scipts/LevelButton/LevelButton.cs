using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scipts.LevelButton
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private string dataName;
        [SerializeField] private string dataStarName;
        [SerializeField] private GameObject zeroStar;
        [SerializeField] private GameObject oneStar;
        [SerializeField] private GameObject twoStars;
        [SerializeField] private GameObject threeStars;
        [SerializeField] private Button nextButton;


        private void Start()
        {
            UnlockNextLevel();
            ShowStars();
        }


        void UnlockNextLevel()
        {
            string check = PlayerPrefs.GetString(dataName);
            if (check == "Yes")
                nextButton.interactable = true;
        }


        void ShowStars()
        {
            int stars = PlayerPrefs.GetInt(dataStarName);
            switch (stars)
            {
                case 0:
                    zeroStar.SetActive(true);
                    return;
                case 1:
                    oneStar.SetActive(true);
                    return;
                case 2:
                    twoStars.SetActive(true);
                    return;
                case 3:
                    threeStars.SetActive(true);
                    return;
            }
        }
    }
}
