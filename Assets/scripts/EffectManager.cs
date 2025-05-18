// File: Assets/Scripts/EffectManager.cs
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [Header("Swipe Effects (slice)")]
    public GameObject[] swipeEffects;
    [Tooltip("Множник масштабу для swipe-ефектів")]
    public float swipeScaleMultiplier = 10f;
    public int   equippedSwipe       = 0;

    [Header("Block Effects (shield)")]
    [Tooltip("Множник масштабу для block-ефектів")]
    public float blockScaleMultiplier = 5f;
    [Tooltip("Час життя block-ефекту (сек)")]
    public float blockEffectDuration  = 3f;

    [Header("Canvas (лиш для UI–префабів)")]
    public Canvas uiCanvas;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else                  Destroy(gameObject);
    }

    public void SpawnSwipeEffect(Vector3 worldPos, float baseScale)
    {
        if (equippedSwipe < 0 || equippedSwipe >= swipeEffects.Length) return;
        var prefab = swipeEffects[equippedSwipe];
        if (prefab == null) return;

        var fx = Instantiate(prefab, worldPos, Quaternion.identity);
        fx.transform.localScale = Vector3.one * baseScale * swipeScaleMultiplier;
        Destroy(fx, blockEffectDuration);
    }

    public void SpawnBlockEffect(Vector3 worldPos, float baseScale)
    {
        // 1) Дістаємо префаб із активного SkinItem
        var skin   = SkinManager.Instance.BlockSkins[SkinManager.Instance.activeBlockSkin];
        var prefab = skin.blockEffectPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("[EffectManager] У скіні немає blockEffectPrefab!");
            return;
        }

        GameObject fx;

        // 2) Якщо це UI–префаб (має RectTransform) — інстанціюємо під Canvas
        if (prefab.GetComponent<RectTransform>() != null && uiCanvas != null)
        {
            fx = Instantiate(prefab, uiCanvas.transform, false);

            // конвертуємо world→screen→canvas-local
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(worldPos),
                uiCanvas.worldCamera,
                out canvasPos
            );
            fx.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        }
        else
        {
            // 3) Інакше — звичайна світова інстанція
            fx = Instantiate(prefab, worldPos, Quaternion.identity);
        }

        // 4) Задаємо масштаб
        fx.transform.localScale = Vector3.one * baseScale * blockScaleMultiplier;

        // 5) Знищуємо через заданий час
        Destroy(fx, blockEffectDuration);
    }
}
