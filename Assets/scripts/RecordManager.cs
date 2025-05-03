using UnityEngine;

public static class RecordManager
{
    const string KEY = "HighScore";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(KEY, 0);
        private set => PlayerPrefs.SetInt(KEY, value);
    }

    public static void TryUpdate(int score)
    {
        if (score > HighScore)
            HighScore = score;
    }
}
