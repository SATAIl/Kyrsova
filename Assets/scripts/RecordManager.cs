using UnityEngine;

public static class RecordManager
{
    private const string KEY = "HighScore";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(KEY, 0);
        private set
        {
            PlayerPrefs.SetInt(KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static bool TryUpdate(int score)
    {
        if (score > HighScore)
        {
            HighScore = score;
            return true;
        }
        return false;
    }

    public static void Reset()
    {
        HighScore = 0;
    }
}