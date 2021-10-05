using UnityEngine;

public class WallDrawer : MonoBehaviour
{
    public float maxLength;
    private GameObject currentWallObject;
    private LineRenderer currentWall;
    public GameObject wall;
    private EdgeCollider2D edgeCollider;
    public float cancelLenghtUnderThreshold;
    private WallStack wallStack;

    private void Start()
    {
        wallStack = gameObject.GetComponent<WallStack>();
    }

    private void Update()
    {
        Touch? tempTouch = getTouch();
        Touch touch;
        if (tempTouch.HasValue)
        {
            touch = tempTouch.Value;
        }
        else
        {
            return;
        }

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        touchPosition.z = 0;

        if (wallStack.HasWallAvailable)
        {
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    MovePhase(touchPosition);
                    break;

                case TouchPhase.Ended:
                    EndPhase(touchPosition);
                    break;

                case TouchPhase.Began:
                    BeganPhase(touchPosition);
                    break;
            }
        }
    }

    private void BeganPhase(Vector3 touchPosition)
    {
        InitializeWall(touchPosition);
    }

    private void InitializeWall(Vector3 touchPosition)
    {
        Debug.Log("Starting wall");
        currentWallObject = Instantiate(wall);
        currentWall = currentWallObject.GetComponent<LineRenderer>();
        currentWall.alignment = LineAlignment.View;
        currentWall.positionCount = 2;
        //Ghost wall material
        currentWall.material = (Material)Resources.Load("Materials/GhostWall", typeof(Material));
        currentWall.widthMultiplier = 0.2f;
        currentWall.SetPosition(0, new Vector3(touchPosition.x, touchPosition.y, 0));
        currentWall.SetPosition(1, new Vector3(touchPosition.x, touchPosition.y, 0));
    }

    private void MovePhase(Vector3 touchPosition)
    {
        Vector3 startPosition = currentWall.GetPosition(0);
        Vector3 dir = touchPosition - startPosition;
        float dist = Vector3.Distance(startPosition, touchPosition);

        if (dist > maxLength)
        {
            touchPosition = startPosition + (dir.normalized * maxLength);
        }

        currentWall.SetPosition(1, new Vector3(touchPosition.x, touchPosition.y, 0));
    }

    private void EndPhase(Vector3 touchPosition)
    {
        Vector3 startPosition = currentWall.GetPosition(0);
        float dist = Vector3.Distance(startPosition, touchPosition);

        if (dist < cancelLenghtUnderThreshold)
        {
            Destroy(currentWallObject);
        }
        else
        {
            AddCollider();
            //Final wall material
            currentWall.material = new Material(Shader.Find("Sprites/Default"));
            wallStack.RemoveWall();
        }
        currentWall = null;
        currentWallObject = null;
    }

    private bool simulateTouchWithMouseIfNotSupported(ref Touch touch)
    {
        touch = new Touch();
        touch.phase = TouchPhase.Canceled;
        touch.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            touch.phase = TouchPhase.Began;
        }
        else if (Input.GetMouseButton(0))
        {
            touch.phase = TouchPhase.Moved;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touch.phase = TouchPhase.Ended;
        }
        else
        {
            return false;
        }
        return true;
    }

    private Touch? getTouch()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(Input.touchCount - 1);
        }
        else
        {
            touch = new Touch();
            if (!simulateTouchWithMouseIfNotSupported(ref touch))
            {
                return null;
            }
        }
        return touch;
    }

    private void AddCollider()
    {
        edgeCollider = currentWallObject.AddComponent<EdgeCollider2D>();
        Vector2[] edge = { currentWall.GetPosition(0), currentWall.GetPosition(1) };
        edgeCollider.points = edge;
        edgeCollider.tag = "wall";
    }
}