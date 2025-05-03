// File: Assets/Scripts/SkinItem.cs
using UnityEngine;

// Тип скіна: або матеріал свайпу, або префаб ефекту блоку
public enum SkinType { SwipeTrail, BlockEffect }

[CreateAssetMenu(fileName = "NewSkin", menuName = "Skin/Item")]
public class SkinItem : ScriptableObject
{
    [Header("Ідентифікація")]
    public string skinId;        // Унікальний ключ (наприклад: "swipe_red")
    public string displayName;   // Назва в UI

    [Header("UI")]
    public Sprite icon;          // Іконка в списку
    public int    price;         // Ціна в монетах

    [Header("Тип скіна")]
    public SkinType type;        // SwipeTrail або BlockEffect

    [Header("Дані для гри")]
    public Material   swipeTrailMaterial; // Матеріал для TrailRenderer (якщо тип=SwipeTrail)
    public GameObject blockEffectPrefab;  // Префаб ефекту (якщо тип=BlockEffect)

    [HideInInspector]
    public bool isUnlocked;      // Стан розблокування (зберігається через SkinManager)
}
