using System;
using Cinemachine;
using Scipts.AllPlayers;
using Scipts.CameraManager;
using Scipts.DirectionArrow;
using Scipts.Eyes;
using Scipts.GreenPortal;
using Scipts.Player3;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Scipts.BadgeManager;
using Scipts.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject purplePortal;
    private Players _players;
    public CinemachineVirtualCamera cinemachine;
    public GameObject stove;
    public GameObject nitrogenTube;
    public Players players;
    public Eyes eyes;
    public CinemachineBrain brain;
    [SerializeField] private CameraManager cameraManager;
    public DirectionArrow directionArrow;
    public Player3 player3;
    public GreenPortal greenPortal;
    public Transform choosenPlayer;

    [Header("Pause Panel UI")] [SerializeField]
    private GameObject pausePanel;

    [SerializeField] private Image pauseBackgound;
    [SerializeField] private RectTransform pauseImage;
    [SerializeField] private RectTransform buttonArea;
    [SerializeField] private RectTransform musicArea;
    [SerializeField] private RectTransform soundArea;
    private bool _isPause;

    [Header("Win Panel UI")] [SerializeField]
    private Sprite[] lastWords;

    [SerializeField] private Sprite[] startsImages;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Image lastWord;
    [SerializeField] private Image startsImage;
    [SerializeField] private GameObject[] badges;

    [Header("Sounds")] private AudioSource _au;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip continueSound;
    [SerializeField] private Settings settings;

    public Action PlayerDie;


    private void Awake()
    {
        instance = this;
        _players = GameObject.FindWithTag("Players").GetComponent<Players>();
        _au = GetComponent<AudioSource>();
    }

    private void Start()
    {
        purplePortal.SetActive(false);
        settings.SetValues();
    }

    public void ReturnPlayer1()
    {
        _players.BackPlayer1();
        cameraManager.ResetScreenAxis();
        PlayerDie?.Invoke();
        player3.health = 100;
        choosenPlayer = _players.transform.GetChild(0);
    }


    public void ChangeCameraTarget(Transform newTarget) => cinemachine.Follow = newTarget;

    public void SetCameraShake(float duration, float frequency, float amplitude) =>
        cameraManager.SetCameraShake(duration, frequency, amplitude);

    public void SetScreenAxis(float xAxis, float yAxis) => cameraManager.SetScreenAxis(xAxis, yAxis);

    public void SetCollide(bool collide) => cameraManager.SetCollide(collide);

    public void IncreaseStar() => greenPortal.currentStars++;


    public void PauseGame()
    {
        if (_isPause) return;
        _isPause = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        _au.clip = pauseSound;
        _au.Play();
        Sequence q = DOTween.Sequence();
        q.SetUpdate(true);
        q.Append(pauseBackgound.DOFade(0, 1f)
            .SetEase(Ease.OutCubic)
            .From());
        q.Join(pauseImage.DOAnchorPosY(pauseImage.anchoredPosition.y + 250f, 1f)
            .SetEase(Ease.OutBack)
            .From());
        q.Join(buttonArea.DOAnchorPosY(buttonArea.anchoredPosition.y - 250f, 1f)
            .SetEase(Ease.OutBack)
            .From());
        q.Join(musicArea.DOAnchorPosX(musicArea.anchoredPosition.x - 250f, 1f)
            .SetEase(Ease.OutBack)
            .From());
        q.Join(soundArea.DOAnchorPosX(soundArea.anchoredPosition.x + 250f, 1f)
            .SetEase(Ease.OutBack)
            .From());
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        _isPause = false;
        _au.clip = continueSound;
        _au.Play();
    }

    public void BackMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenWinPanel()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
        startsImage.sprite = startsImages[greenPortal.currentStars];
        lastWord.sprite = lastWords[greenPortal.currentStars];
        OpenBadges();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level" + greenPortal.currentLevel);
    }

    void OpenBadges()
    {
        bool[] isActive = BadgeManager.instance.isWinThisLevel;
        for (int i = 0; i < badges.Length; i++)
        {
            badges[i].SetActive(isActive[i]);
        }
    }
}