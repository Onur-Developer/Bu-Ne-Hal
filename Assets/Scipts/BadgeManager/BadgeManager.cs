using UnityEngine;

namespace Scipts.BadgeManager
{
    public class BadgeManager : MonoBehaviour
    {
        private int _solidMaster;
        private int _collecthalfStars;
        private int _clampLevel;
        private int _collectStars;
        private int _withoutPortal;
        private int _detectiveliquid;
        private int _currentStars;
        private int _defeatBoss;
        private int _pumped;
        private int _lastAirBender;
        private int _noob;
        private int _withoutError;

        public bool[] isWinThisLevel = new bool[10];

        public static BadgeManager instance;


        private void Awake()
        {
            instance = this;
            TakeValues();
        }

        private void Start()
        {
            _currentStars = PlayerPrefs.GetInt("LevelStar" + GameManager.instance.greenPortal.currentLevel);
        }

        void TakeValues()
        {
            _solidMaster = PlayerPrefs.GetInt("SolidMaster");
            _collecthalfStars = PlayerPrefs.GetInt("Collect25Stars");
            _clampLevel = PlayerPrefs.GetInt("Noob");
            _collectStars = PlayerPrefs.GetInt("CollectAllStars");
            _withoutPortal = PlayerPrefs.GetInt("WithoutPortal");
            _detectiveliquid = PlayerPrefs.GetInt("DetectiveLiquid");
            _defeatBoss = PlayerPrefs.GetInt("DefeatBoss");
            _pumped = PlayerPrefs.GetInt("Pumped");
            _lastAirBender = PlayerPrefs.GetInt("LastAirbender");
            _noob = PlayerPrefs.GetInt("Noob");
            _withoutError = PlayerPrefs.GetInt("WithoutError");
        }


        public void SolidMaster()
        {
            if (GameManager.instance.greenPortal.currentLevel <= 3) return;
            if (_solidMaster == 5) return;
            _solidMaster++;
            _solidMaster = Mathf.Min(_solidMaster, 5);
            PlayerPrefs.SetInt("SolidMaster", _solidMaster);
            if (_solidMaster == 5)
                isWinThisLevel[0] = true;
        }

        public void CollectHalfStars()
        {
            if (_currentStars != 0) return;
            if (_collecthalfStars == 25) return;
            _collecthalfStars++;
            _collecthalfStars = Mathf.Min(_collecthalfStars, _collecthalfStars);
            PlayerPrefs.SetInt("Collect25Stars", _collecthalfStars);
            if (_collecthalfStars == 25)
                isWinThisLevel[1] = true;
        }

        public void LastAirbender(float health)
        {
            if (GameManager.instance.greenPortal.currentLevel <= 3) return;
            if (health < 60) return;
            if (_lastAirBender == 1) return;
            _lastAirBender++;
            PlayerPrefs.SetInt("LastAirbender", _lastAirBender);
            if (_lastAirBender == 1)
                isWinThisLevel[2] = true;
        }

        public void Pumped(bool isTurning)
        {
            if (GameManager.instance.greenPortal.currentLevel <= 3) return;
            if (isTurning) return;
            if (_pumped == 1) return;
            _pumped++;
            PlayerPrefs.SetInt("Pumped", _pumped);
            if (_pumped == 1)
                isWinThisLevel[4] = true;
        }

        public void Noob(int level)
        {
            if (_noob == 3) return;
            int clamp = Mathf.Min(3, level);
            _clampLevel = clamp;
            PlayerPrefs.SetInt("Noob", _clampLevel);
            if (_clampLevel == 3)
                isWinThisLevel[5] = true;
        }

        public void WithoutError(bool isDied)
        {
            if (GameManager.instance.greenPortal.currentLevel <= 3) return;
            if (isDied) return;
            if (_withoutError == 1) return;
            _withoutError++;
            PlayerPrefs.SetInt("WithoutError", _withoutError);
            if (_withoutError == 1)
                isWinThisLevel[6] = true;
        }

        public void CollectAllStars()
        {
            if (_currentStars != 0)
            {
                _currentStars--;
                return;
            }

            if (_collectStars == 30) return;
            _collectStars++;
            _collectStars = Mathf.Min(_collectStars, _collectStars);
            PlayerPrefs.SetInt("CollectAllStars", _collectStars);
            if (_collectStars == 30)
                isWinThisLevel[7] = true;
        }

        public void WithoutPortal(bool isDied)
        {
            if (GameManager.instance.greenPortal.currentLevel <= 3) return;
            if (isDied) return;
            if (_withoutPortal == 5) return;
            _withoutPortal++;
            _withoutPortal = Mathf.Min(_withoutPortal, _withoutPortal);
            PlayerPrefs.SetInt("WithoutPortal", _withoutPortal);
            if (_withoutPortal == 5)
                isWinThisLevel[8] = true;
        }

        public void DetectiveLiquid()
        {
            if (_detectiveliquid == 1) return;
            _detectiveliquid++;
            PlayerPrefs.SetInt("DetectiveLiquid", _detectiveliquid);
            if (_detectiveliquid == 1)
                isWinThisLevel[3] = true;
        }

        public void DefeatBoss()
        {
            if (_defeatBoss == 1) return;
            _defeatBoss++;
            PlayerPrefs.SetInt("DefeatBoss", _defeatBoss);
            if (_defeatBoss == 1)
                isWinThisLevel[9] = true;
        }
    }
}