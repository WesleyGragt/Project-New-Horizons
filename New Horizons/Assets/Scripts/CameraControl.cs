using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Automatic camControl
    public Transform camera;
    public float minDistance;

    //Drag Control
    [Range(0f, 1f)] public float dragStart;
    [Range(0f, 1f)] float dragMinLength;

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 prevPos;
    private float dragDistance;
    bool dragUse = false;

    private Player player;
    private GameManager gameManager;
    private Vector2 screenSize;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        screenSize = new Vector2(Screen.width, Screen.height);
        dragDistance = screenSize.y * dragMinLength;
    }

    private void Update()
    {
        ScreenInput();
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime);
        camera.LookAt(player.position);

        Vector3 offset = player.position - camera.position;

        float distance = offset.magnitude;

        if (distance < minDistance) transform.position = Vector3.Lerp(transform.position, transform.position - offset.normalized * (minDistance - distance), Time.deltaTime);
    }

    private void ScreenInput()
    {
        int touchCount = Input.touchCount;

        if (touchCount > 0)
        {
            Vector2 touchInputPos = Input.GetTouch(0).position;

            bool inDeadZone = touchInputPos.x < screenSize.x / 4 && touchInputPos.y < screenSize.y / 3;

            if (inDeadZone)
            {
                dragUse = false;

                if (touchCount > 1 && gameManager.GetControls() == GameControl.Joystick)
                {
                    dragUse = true;
                    TouchDrag(Input.GetTouch(1), dragUse);
                }
                else transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
            }
            else
            {
                TouchDrag(Input.GetTouch(0), dragUse);
            }
        }
        else transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
    }

    private void TouchDrag(Touch touchDrag, bool draggable)
    {
        if (touchDrag.phase == TouchPhase.Ended || touchDrag.phase == TouchPhase.Canceled)
        {
            dragUse = true;
            return;
        }

        if (!draggable)
        {
            transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
            return;
        }

        if (touchDrag.phase == TouchPhase.Began)
        {
            pos1 = touchDrag.position;
            pos2 = touchDrag.position;
            prevPos = touchDrag.position;
        }
        else if (touchDrag.phase == TouchPhase.Moved)
        {
            pos2 = touchDrag.position;

            Vector3 rotation = -new Vector3(0, (prevPos.x - pos2.x), 0);
            transform.eulerAngles += rotation;

            prevPos = touchDrag.position;
        }
    }
}
