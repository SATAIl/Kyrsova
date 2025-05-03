// File: Assets/Scripts/CurrencyManager.cs
using UnityEngine;

/// <summary>
/// Баланс монет:
///  - Balance       — поточний баланс (витрати/залишок)
///  - TotalEarned   — сукупно зароблені монети (для колекції)
/// </summary>
public static class CurrencyManager
{
    const string BAL_KEY = "CurrencyBalance";
    const string TOT_KEY = "TotalCoinsEarned";

    public static int Balance
    {
        get => PlayerPrefs.GetInt(BAL_KEY, 0);
        set => PlayerPrefs.SetInt(BAL_KEY, value);
    }

    public static int TotalEarned
    {
        get => PlayerPrefs.GetInt(TOT_KEY, 0);
        private set => PlayerPrefs.SetInt(TOT_KEY, value);
    }

    /// <summary>
    /// Додає amount до балансу та до загального заробітку.
    /// </summary>
    public static void Add(int amount)
    {
        if (amount <= 0) return;
        Balance += amount;
        TotalEarned += amount;
    }

    /// <summary>
    /// Спробувати витратити amount. Повертає true, якщо вистачило.
    /// </summary>
    public static bool TrySpend(int amount)
    {
        if (amount <= 0) return false;
        if (Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Скинути поточний баланс (для тестів).
    /// </summary>
    public static void ResetBalance()
    {
        Balance = 0;
    }

    /// <summary>
    /// Скинути і баланс і загальний заробіток.
    /// </summary>
    public static void ResetAll()
    {
        Balance = 0;
        TotalEarned = 0;
    }
}
