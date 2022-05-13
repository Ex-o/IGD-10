using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance ?? (instance = FindObjectOfType<UIManager>());

    [SerializeField] private Transform kidModelTransform, stairFinishTransform;
    [SerializeField] private Slider slider;
    private float totalDistance;

    [SerializeField] private TextMeshProUGUI diamondText, failText, levelCompletedText;
    [SerializeField] private RectTransform retryButtonRectTransform, nextButtonRectTransform;
    private int diamondNumber;

    [SerializeField] private GameObject failPanel, levelCompletedPanel;

    [SerializeField] GameObject animatedCoinPrefab;
    
    [Space]
    [Header ("Animation settings")]
    [SerializeField] [Range (0.5f, 10f)] float minAnimDuration;
    [SerializeField] [Range (0.9f, 200f)] float maxAnimDuration;

    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    [SerializeField] RectTransform targetPosition;
    
    private float t = 0.0f;
    public Text highScore;
    public float highScoreAnimationLength = 1.0f; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            totalDistance = stairFinishTransform.position.z - kidModelTransform.position.z;
            var coins = PlayerPrefs.GetInt("diamond-count");
            diamondText.text = coins.ToString();
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
            CheckProgressBar();

        var curScore = PlayerPrefs.GetInt("diamond-count", 0);
        if (SceneManager.GetActiveScene().buildIndex > 0 && levelCompletedPanel.activeInHierarchy)
        {
            if (Convert.ToInt32(diamondText.text) != curScore)
            {
                t = Mathf.MoveTowards(t, 1.0f, Time.deltaTime / highScoreAnimationLength);
                int displayedScore = (int)Mathf.Lerp(0, curScore, t);
                diamondText.text = displayedScore.ToString ();
            }
        }
    }

    public void CheckProgressBar()
    {
        // NextButton();
        if (GameManager.Instance.IsGameStart())
        {
            var kidModelCurrentPosition = kidModelTransform.position.z;
            var currentDistance = totalDistance - kidModelCurrentPosition;
            var newSliderValue = (totalDistance - currentDistance) / totalDistance;
            slider.value = newSliderValue;
        }
    }

    public void UpdateDiamond()
    {
        diamondNumber++;
        // diamondText.text = diamondNumber.ToString();
    }
    
    public void ActivateFailPanel()
    {
        failPanel.SetActive(true);

        failText.rectTransform.DOLocalMoveY(failText.rectTransform.localPosition.y - 1494f, 1.5f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutBack);

        retryButtonRectTransform.DOLocalMoveY(retryButtonRectTransform.localPosition.y + 978f, 1.5f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutBack);
    }

    public void AddBonus(int bonus)
    {
        diamondNumber += bonus;
    }
    public void UpdateDiamonds()
    {
        var curScore = PlayerPrefs.GetInt("diamond-count", 0);
        PlayerPrefs.SetInt("diamond-count", curScore + diamondNumber);
    }
    public void ActivateLevelCompletedPanel()
    {
        levelCompletedPanel.SetActive(true);
        
        levelCompletedText.rectTransform.DOLocalMoveY(levelCompletedText.rectTransform.localPosition.y - 1494f, 1.5f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutBack);

        nextButtonRectTransform.DOLocalMoveY(nextButtonRectTransform.localPosition.y + 978f, 1.5f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutBack);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        int highestLevel = PlayerPrefs.GetInt("highest-level", 0);
        int level = (highestLevel == 4 ? 3 : highestLevel + 1);
        SceneManager.LoadScene(level);
    }
    public void RetryButton()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        var nextLevel = PlayerPrefs.GetInt("highest-level", 0) + 1;

        if (nextLevel == 5)
        {
            nextLevel = 3;
            PlayerPrefs.SetInt("highest-level", nextLevel);
        }
        SceneManager.LoadScene(nextLevel);
    }

    public void AnimateCoins(Transform obj, Vector3 scale)
    {
        int amount = 10;
        
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            
            if (scale != Vector3.zero)
                coin.transform.localScale = scale;
            
            coin.SetActive(true);
            coin.transform.position = obj.position + new Vector3(Random.Range(-spread, spread), 0f, 0f);

            float duration = Random.Range(minAnimDuration, maxAnimDuration);
            coin.transform.DOMove(targetPosition.position, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    coin.SetActive(false);
                });
        }
    }
    public void NextButton()
    {
        UpdateDiamonds();
        AnimateCoins(nextButtonRectTransform, Vector3.zero);
        Invoke("LoadNextLevel", 1.5f);
    }
}
