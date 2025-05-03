using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("Tap vs Swipe")]
    public float tapThresholdTime = 0.2f;
    public float moveThreshold    = 0.1f;

    private Camera cam;
    private GameSceneManager gm;
    private Canvas uiCanvas;

    private Vector2 pressPos;
    private float   pressTime;
    private bool    dragging;
    private Vector2 lastPos;

    void Awake()
    {
        cam      = Camera.main;
        gm       = FindObjectOfType<GameSceneManager>();
        uiCanvas = FindObjectOfType<Canvas>();
    }

    void Update()
    {
        Vector2 mp = Input.mousePosition;
        if (mp.x < 0 || mp.y < 0 || mp.x > Screen.width || mp.y > Screen.height)
        {
            dragging = false;
            return;
        }
        Vector2 wp = cam.ScreenToWorldPoint(mp);

        // Початок натискання
        if (Input.GetMouseButtonDown(0))
        {
            pressPos  = wp;
            lastPos   = wp;
            pressTime = Time.time;
            dragging  = false;
        }

        // Тримаємо — swipe?
        if (Input.GetMouseButton(0))
        {
            if (!dragging && Vector2.Distance(pressPos, wp) > moveThreshold)
                dragging = true;

            if (dragging)
            {
                var hits = Physics2D.LinecastAll(lastPos, wp);
                foreach (var hit in hits)
                {
                    var proj = hit.collider.GetComponent<Projectile>();
                    if (proj != null && !proj.reacted)
                    {
                        proj.reacted = true;
                        if (proj.type == Projectile.Type.Good)    gm.AddScore(10);
                        else if (proj.type == Projectile.Type.Star) gm.LoseHP();
                        else { gm.AddHP(1); gm.AddScore(5); }

                        EffectManager.Instance.SpawnSwipeEffect(
                            hit.point,
                            proj.transform.localScale.x
                        );
                        Destroy(proj.gameObject);
                    }
                }
                lastPos = wp;
            }
        }

        // Відпустили — tap-block?
        if (Input.GetMouseButtonUp(0))
        {
            if (!dragging && (Time.time - pressTime) <= tapThresholdTime)
                DoBlock(wp);
            dragging = false;
        }
    }

    private void DoBlock(Vector2 wp)
    {
        // Shield UI full-screen (якщо потрібен окремий prefab, робіть тут)

        // Блокуємо тільки Star-проектайли
        foreach (var proj in FindObjectsOfType<Projectile>())
        {
            if (!proj.reacted && proj.type == Projectile.Type.Star)
            {
                proj.reacted = true;
                gm.AddScore(5);

                EffectManager.Instance.SpawnBlockEffect(
                    proj.transform.position,
                    proj.transform.localScale.x
                );

                Destroy(proj.gameObject);
            }
        }
    }
}
