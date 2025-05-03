// File: Assets/Scripts/BlockEffectController.cs
using UnityEngine;

public class BlockEffectController : MonoBehaviour
{
    // Точка, звідки спавниться щит (наприклад, під рукою персонажа)
    [Tooltip("Куди інстанціювати щит")]
    public Transform blockEffectSpawnPoint;
    [Tooltip("Який масштаб давати щиту")]
    public Vector3    blockEffectScale = Vector3.one;

    private GameObject _currentBlockEffect;

    /// <summary>
    /// Інстанціює новий префаб щита з активного SkinItem
    /// </summary>
    public void ActivateBlockEffect()
    {
        var skin = SkinManager.Instance.BlockSkins[SkinManager.Instance.activeBlockSkin];

        // Якщо старий є — видаляємо
        if (_currentBlockEffect != null)
            Destroy(_currentBlockEffect);

        // Інстанціюємо новий
        if (skin.blockEffectPrefab != null)
        {
            _currentBlockEffect = Instantiate(
                skin.blockEffectPrefab,
                blockEffectSpawnPoint.position,
                Quaternion.identity,
                blockEffectSpawnPoint  // робимо його дочірнім
            );

            // Встановлюємо правильний масштаб
            _currentBlockEffect.transform.localScale = blockEffectScale;
        }
        else
        {
            Debug.LogWarning("[BlockEffectController] prefab для щита не заданий у SkinItem");
        }
    }

    /// <summary>
    /// Видаляє поточний щит
    /// </summary>
    public void DeactivateBlockEffect()
    {
        if (_currentBlockEffect != null)
        {
            Destroy(_currentBlockEffect);
            _currentBlockEffect = null;
        }
    }
}