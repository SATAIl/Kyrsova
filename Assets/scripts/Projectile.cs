using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Type { Good, Star, Heart }
    public Type type;

    [Header("Scale & Timing")]
    public float initialScale      = 0.05f;
    public float maxScale          = 0.25f;
    public float baseApproachSpeed = 2.0f;
    public float lifeDuration      = 0.5f;

    [HideInInspector] public bool reacted = false;
    private float timer;
    private GameSceneManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameSceneManager>();
        timer = 0f;
        transform.localScale = Vector3.one * initialScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Зростання масштабу
        float t = Mathf.Clamp01((float)gm.Score / 240f);
        float speedFactor = Mathf.Lerp(0.18f, 0.6f, t);
        float speed = baseApproachSpeed * speedFactor;
        float next = Mathf.Min(transform.localScale.x + speed * Time.deltaTime, maxScale);
        transform.localScale = Vector3.one * next;

        // Автознищення
        if (timer >= lifeDuration)
        {
            if (!reacted)
            {
                if (type == Type.Good || type == Type.Star)    gm.LoseHP();
                else if (type == Type.Heart)                    gm.AddHP();

                // **Тут ми більше не маємо автодеспавн-ефекту в EffectManager**
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reacted || !other.CompareTag("Sword")) return;  // перевіряємо тег Sword
        reacted = true;

        if (type == Type.Good)      gm.AddScore(10);
        else if (type == Type.Star) gm.LoseHP();
        else                        { gm.AddHP(1); gm.AddScore(5); }

        // **Swipe-ефект**
        EffectManager.Instance.SpawnSwipeEffect(
            transform.position,
            transform.localScale.x
        );

        Destroy(gameObject);
    }
}
