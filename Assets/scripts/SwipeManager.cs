using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public GameObject swipeTrailPrefab;
    public float minMoveThreshold = 0.1f;
    public float trailLifetime = 0f;
    public float trailWidth = 0.1f;
    public float zOffset = -1f; // важливе зміщення ближче до камери

    private GameObject _trailObj;
    private LineRenderer _trail;
    private Vector3 _lastPos;
    private bool _isDragging;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastPos = ScreenToWorld(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 curr = ScreenToWorld(Input.mousePosition);
            if (_trail == null && Vector3.Distance(curr, _lastPos) > minMoveThreshold)
                BeginSwipe(curr);
            else if (_trail != null)
                ContinueSwipe(curr);

            _lastPos = curr;
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            EndSwipe();
            _isDragging = false;
        }
    }

    Vector3 ScreenToWorld(Vector3 screenPos)
    {
        var wp = Camera.main.ScreenToWorldPoint(screenPos);
        wp.z = zOffset;  // ← важливе зміщення!
        return wp;
    }

    void BeginSwipe(Vector3 startPos)
    {
        _trailObj = Instantiate(swipeTrailPrefab, startPos, Quaternion.identity);
        _trail = _trailObj.GetComponent<LineRenderer>();
        _trail.positionCount = 0;
        _trail.startWidth = _trail.endWidth = trailWidth;

        var skin = SkinManager.Instance.SwipeSkins[SkinManager.Instance.activeSwipeSkin];
        if (skin.swipeTrailMaterial != null)
            _trail.material = skin.swipeTrailMaterial;

        AddPoint(startPos);
    }

    void ContinueSwipe(Vector3 pos) => AddPoint(pos);

    void AddPoint(Vector3 pos)
    {
        pos.z = zOffset;  // ← важливо, щоб точки були на правильній глибині!
        _trail.positionCount++;
        _trail.SetPosition(_trail.positionCount - 1, pos);
    }

    void EndSwipe()
    {
        Destroy(_trailObj, trailLifetime);
        _trailObj = null;
        _trail = null;
    }
}
