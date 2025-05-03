using UnityEngine;

public class StarController : MonoBehaviour
{
    void OnMouseDown()
    {
        // коли натиснули на колайдер зірки
        Destroy(gameObject);
    }
}