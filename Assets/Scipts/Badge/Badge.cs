using TMPro;
using UnityEngine;

namespace Scipts.Badge
{
    public class Badge : MonoBehaviour
    {
        [SerializeField] private GameObject blackImage;
        [SerializeField] private GameObject padlock;
        [SerializeField] private TextMeshProUGUI unlockNumber;
        [SerializeField] private string dataName;
        [SerializeField] private int unlockValue;


        private void Start()
        {
            WriteUnlockNumber();
            CheckUnlock();
        }


        void CheckUnlock()
        {
            if (PlayerPrefs.GetInt(dataName) == unlockValue)
            {
                blackImage.SetActive(false);
                padlock.SetActive(false);
                unlockNumber.gameObject.SetActive(false);
            }
        }

        void WriteUnlockNumber() => unlockNumber.text = PlayerPrefs.GetInt(dataName) + "/" + unlockValue;
    }
}