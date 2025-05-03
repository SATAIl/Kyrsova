using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class SwipeTrail : MonoBehaviour
{
    [Header("Вигляд сліду")]
    public Material trailMaterial;       // Матеріал із прозорим шейдером (sprites default або particles/fade)
    public Color    trailColor    = Color.white;
    public float    trailWidth    = 0.1f;   // Товщина лінії

    [Header("Параметри побудови")]
    public float minDistance   = 0.05f;     // Мінімальний крок між точками
    public float fadeDuration  = 0.5f;      // Час затухання після закінчення свайпу

    private LineRenderer line;
    private bool         isSwiping;
    private Vector3      lastPoint;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        // Базові налаштування LineRenderer
        line.positionCount     = 0;
        line.useWorldSpace     = true;
        line.material          = trailMaterial != null
                                  ? trailMaterial
                                  : new Material(Shader.Find("Sprites/Default"));
        line.startColor        = trailColor;
        line.endColor          = trailColor;
        line.startWidth        = trailWidth;
        line.endWidth          = trailWidth;
        line.numCapVertices    = 8;
        line.numCornerVertices = 6;
    }

    void Update()
    {
        // Отримуємо світову позицію без Z-offset
        Vector3 worldPos;
        bool down=false, held=false, up=false;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector3 sp = new Vector3(t.position.x, t.position.y, 0f);
            worldPos = Camera.main.ScreenToWorldPoint(sp);
            worldPos.z = 0f;

            down = t.phase == TouchPhase.Began;
            held = t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary;
            up   = t.phase == TouchPhase.Ended   || t.phase == TouchPhase.Canceled;
        }
        else
        {
            Vector3 sp = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                0f);
            worldPos = Camera.main.ScreenToWorldPoint(sp);
            worldPos.z = 0f;

            down = Input.GetMouseButtonDown(0);
            held = Input.GetMouseButton(0);
            up   = Input.GetMouseButtonUp(0);
        }

        // ПОЧАТОК свайпу: очищаємо старий слід та додаємо початкову точку
        if (down)
        {
            isSwiping = true;
            StopAllCoroutines();
            line.positionCount = 0;
            line.positionCount = 1;
            line.SetPosition(0, worldPos);
            lastPoint = worldPos;
        }

        // ПРОДОВЖЕННЯ свайпу: додаємо/оновлюємо точки
        if (held && isSwiping)
        {
            float dist = Vector3.Distance(lastPoint, worldPos);
            if (dist >= minDistance)
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, worldPos);
                lastPoint = worldPos;
            }
            else
            {
                // оновлюємо останню точку, щоб слід не відставав
                line.SetPosition(line.positionCount - 1, worldPos);
                lastPoint = worldPos;
            }
        }

        // КІНЕЦЬ свайпу: запускаємо плавне затухання
        if (up && isSwiping)
        {
            isSwiping = false;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startC = line.startColor, endC = line.endColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            float alpha = Mathf.Lerp(startC.a, 0f, t);

            Color c1 = startC; c1.a = alpha;
            Color c2 = endC;   c2.a = alpha;
            line.startColor = c1;
            line.endColor   = c2;

            yield return null;
        }
        // Після затухання повністю очищаємо слід
        line.positionCount = 0;
    }
}
