// File: Assets/Scripts/GameOverManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverUI;

    [Header("Result Texts")]
    public TextMeshProUGUI yourScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinText;          // показує монети, зароблені за сесію

    [Header("Buttons")]
    public Button restartButton;
    public Button menuButton;

    void Awake()
    {
        gameOverUI.SetActive(false);
        restartButton.onClick.AddListener(OnRestart);
        menuButton.onClick.AddListener(OnMainMenu);
    }

    public void ShowGameOver()
    {
        // зупиняємо гру
        Time.timeScale = 0f;

        // рахунок за сесію
        int finalScore = GameSceneManager.Instance.Score;
        yourScoreText.text = $"Рахунок: {finalScore}";

        // оновлюємо рекорд, якщо потрібно
        RecordManager.TryUpdate(finalScore);
        highScoreText.text    = $"Рекорд: {RecordManager.HighScore}";

        // скільки монет зароблено в цій сесії
        int sessionCoins    = GameSceneManager.Instance.SessionCoins;
        coinText.text       = $"Монети: {sessionCoins}";

        // Додаємо монети до загального балансу і зберігаємо
        CurrencyManager.Add(sessionCoins);
        

        gameOverUI.SetActive(true);
    }

    void OnRestart()
    {
        // перезапуск сцени
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnMainMenu()
    {
        // повернення в головне меню
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
