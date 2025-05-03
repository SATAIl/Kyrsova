using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText; 
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Settings")]
    public int maxHP = 3;

    private int score = 0;
    private int hp;
    
    // нове поле для монет, зароблених в цій сесії:
    private int sessionCoins = 0;

    public int Score => score;
    public int SessionCoins => sessionCoins;   // щойно додана властивість

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        hp = maxHP;
        UpdateAllUI();
    }

    public void AddScore(int points)
    {
        // стара логіка без виклику CurrencyManager
        int oldThresh = score / 10;
        score += points;
        UpdateScoreUI();

        int newThresh = score / 10;
        int coinsToAdd = newThresh - oldThresh;
        if (coinsToAdd > 0)
        {
            // рахуємо тільки в сесії
            sessionCoins += coinsToAdd;
            // (CurrencyManager лишається без змін — ми його поки не зачіпаємо)
        }
    }

    public void ResetScore()
    {
        score = 0;
        sessionCoins = 0;   // не забуваємо скинути монети сесії при рестарті
        UpdateScoreUI();
    }

    public void LoseHP(int amount = 1)
    {
        hp -= amount;
        UpdateHeartsUI();
        if (hp <= 0)
            FindObjectOfType<GameOverManager>().ShowGameOver();
    }

    public void AddHP(int amount = 1)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHP);
        UpdateHeartsUI();
    }

    public void ResetHP()
    {
        hp = maxHP;
        UpdateHeartsUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText) scoreText.text = score.ToString();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = (i < hp) ? fullHeart : emptyHeart;
    }

    void UpdateAllUI()
    {
        UpdateScoreUI();
        UpdateHeartsUI();
    }
}
