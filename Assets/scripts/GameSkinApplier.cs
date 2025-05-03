// File: Assets/Scripts/GameSkinApplier.cs
using UnityEngine;

public class GameSkinApplier : MonoBehaviour
{
    // Синглтон для легкого доступу з інших скриптів
    public static GameSkinApplier Instance { get; private set; }

    [Header("LineRenderer для свайпу (можна призначити вручну)")]
    public LineRenderer swipeTrail;

    void Awake()
    {
        // Ініціалізуємо синглтон
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Якщо не призначили вручну в інспекторі — автоматично знаходимо перший LineRenderer
        if (swipeTrail == null)
        {
            var all = FindObjectsOfType<LineRenderer>();
            if (all.Length > 0)
            {
                swipeTrail = all[0];
                Debug.Log($"[GameSkinApplier] Використаний LineRenderer з GameObject “{swipeTrail.gameObject.name}”");
            }
            else
            {
                Debug.LogWarning("[GameSkinApplier] Не знайдено жодного LineRenderer у сцені!");
            }
        }

        // Зразу застосовуємо поточний скин
        UpdateSkin();
    }

    /// <summary>
    /// Зчитує активний скин з SkinManager і змінює матеріал свайпу.
    /// </summary>
    public void UpdateSkin()
    {
        if (swipeTrail == null)
            return;

        var mgr   = SkinManager.Instance;
        var swipe = mgr.SwipeSkins[mgr.activeSwipeSkin];

        if (swipe.swipeTrailMaterial != null)
            swipeTrail.material = swipe.swipeTrailMaterial;
        else
            Debug.LogWarning("[GameSkinApplier] Для цього скіна не задано swipeTrailMaterial!");
    }
}
