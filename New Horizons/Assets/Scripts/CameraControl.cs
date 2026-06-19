using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Automatic camControl
    public Transform camera;
    public float minDistance;
    public float maxDistance;
    
    //Drag Control
    [Space]
    public Joystick joystick;
    [Range(0f, 1f)] public float dragStart;

    private Vector3 curPos;
    private Vector3 prevPos;
    control touch1 = control.None;
    control touch2 = control.None;

    private Player player;
    private GameManager gameManager;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        ScreenInput();
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = player.transform.position;

        Vector3 offset = player.position - camera.position;

        float distance = offset.magnitude;

        if (distance < minDistance) transform.position = Vector3.Lerp(transform.position, transform.position - offset.normalized * (minDistance - distance), Time.deltaTime);
        camera.LookAt(player.position);
    }

    private void ScreenInput()
    {
        int touchCount = Input.touchCount;

        if (touch1 == control.None && touch2 != control.None)
        {
            touch1 = touch2;
            touch2 = control.None;
        }

        if (touchCount > 0)
        {
            //joystick.touched = true;
            //joystick.touchInput = Input.GetTouch(int);
            //TouchDrag(Input.GetTouch(0), true);

            if (touch1 != control.Drag && (joystick.touched || joystick.OnStick(Input.GetTouch(0).position)))
            {
                touch1 = control.Joystick;

                joystick.touched = true;
                joystick.touchInput = Input.GetTouch(0);
            } else if (touch1 != control.Joystick && !joystick.OnStick(Input.GetTouch(0).position))
            {
                touch1 = control.Drag;

                TouchDrag(Input.GetTouch(0), true);
            }

            if (touchCount == 1 && touch1 != control.Drag)
            {
                transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
            }
        }
        else transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
    }

    private void TouchDrag(Touch touchDrag, bool draggable)
    {
        if (touchDrag.phase == TouchPhase.Ended || touchDrag.phase == TouchPhase.Canceled)
        {
            if (touch1 == control.Drag) touch1 = control.None;
            else if (touch2 == control.Drag) touch1 = control.None;
            return;
        }

        if (!draggable)
        {
            transform.forward = Vector3.Lerp(transform.forward, player.forward, Time.deltaTime);
            return;
        }

        if (touchDrag.phase == TouchPhase.Began)
        {
            curPos = touchDrag.position;
            prevPos = touchDrag.position;
        }
        else if (touchDrag.phase == TouchPhase.Moved)
        {
            curPos = touchDrag.position;

            Vector3 rotation = -new Vector3(0, (prevPos.x - curPos.x), 0);
            transform.eulerAngles += rotation;

            prevPos = touchDrag.position;
        }
    }
}

enum control
{
    None,
    Drag,
    Joystick
}
