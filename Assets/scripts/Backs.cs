using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // 1) висота світу в одиницях = orthoSize * 2
        float worldHeight = Camera.main.orthographicSize * 2f;
        // 2) ширина світу за aspect:
        float worldWidth  = worldHeight * Screen.width / Screen.height;
        // 3) розмір спрайта (в одиницях) за 1:1 scale
        Vector2 spriteSize = sr.sprite.bounds.size;
        // 4) новий scale
        Vector3 newScale = transform.localScale;
        newScale.x = worldWidth  / spriteSize.x;
        newScale.y = worldHeight / spriteSize.y;
        transform.localScale = newScale;
    }
}
