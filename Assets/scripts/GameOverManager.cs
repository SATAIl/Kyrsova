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
    public TextMeshProUGUI coinText;          // тут тепер монети сесії

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
        Time.timeScale = 0f;

        int finalScore = GameSceneManager.Instance.Score;
        yourScoreText.text = $"Рахунок: {finalScore}";

        // якщо RecordManager ще не реалізований,
        // можна тимчасово виводити finalScore як HighScore
        highScoreText.text = $"Рекорд: {finalScore}";

        // ** ось тут монети сесії **
        int sessionCoins = GameSceneManager.Instance.SessionCoins;
        coinText.text = $"Монети: {sessionCoins}";

        gameOverUI.SetActive(true);
    }

    void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
