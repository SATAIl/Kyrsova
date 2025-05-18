// File: Assets/Scripts/SkinManager.cs
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [Header("Колекція скінів")]
    public List<SkinItem> SwipeSkins; // Список свайп-скінів
    public List<SkinItem> BlockSkins; // Список блок-скінів

    [Header("Вибрані скіни")]
    public int activeSwipeSkin;       // Індекс активного свайп-скіну
    public int activeBlockSkin;       // Індекс активного блоку

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else Destroy(gameObject);
    }

    // Завантажує стани з PlayerPrefs
    void LoadData()
    {
        activeSwipeSkin = PlayerPrefs.GetInt("ActiveSwipe", 0);
        activeBlockSkin = PlayerPrefs.GetInt("ActiveBlock", 0);

        for (int i = 0; i < SwipeSkins.Count; i++)
            SwipeSkins[i].isUnlocked = PlayerPrefs.GetInt($"SwipeUnlocked_{i}", i == 0 ? 1 : 0) == 1;

        for (int i = 0; i < BlockSkins.Count; i++)
            BlockSkins[i].isUnlocked = PlayerPrefs.GetInt($"BlockUnlocked_{i}", i == 0 ? 1 : 0) == 1;
    }

    // Зберігає стани в PlayerPrefs
    public void SaveData()
    {
        PlayerPrefs.SetInt("ActiveSwipe", activeSwipeSkin);
        PlayerPrefs.SetInt("ActiveBlock", activeBlockSkin);

        for (int i = 0; i < SwipeSkins.Count; i++)
            PlayerPrefs.SetInt($"SwipeUnlocked_{i}", SwipeSkins[i].isUnlocked ? 1 : 0);

        for (int i = 0; i < BlockSkins.Count; i++)
            PlayerPrefs.SetInt($"BlockUnlocked_{i}", BlockSkins[i].isUnlocked ? 1 : 0);

        PlayerPrefs.Save();
    }
}