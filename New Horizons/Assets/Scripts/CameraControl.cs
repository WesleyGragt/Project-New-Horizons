using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    //Automatic camControl
    public TextMeshProUGUI text;
    public Transform cam;
    public float minDistance;
    
    //Drag Control
    [Space]
    public Joystick joystick;
    [Range(0f, 1f)] public float dragStart;

    private Vector3 curPos;
    private Vector3 prevPos;
    control touch1 = control.None;
    control touch2 = control.None;

    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        text.text = $"1: {touch1}, 2: {touch2}";

        FollowPlayer();
        ScreenInput();
    }

    private void FollowPlayer()
    {
        transform.position = player.position;

        Vector3 offset = player.position - cam.position;

        float distance = offset.magnitude;

        if (distance < minDistance) transform.position = Vector3.Lerp(transform.position, transform.position - offset.normalized * (minDistance - distance), Time.deltaTime);
        cam.LookAt(player.position);
    }

    private void ScreenInput()
    {
        int touchCount = Input.touchCount;

        if (touch1 == control.None && touch2 != control.None)
        {
            touch1 = touch2;
            touch2 = control.None;
        }

        if (joystick.touchEnd)
        {
            if (touch1 == control.Joystick) touch1 = control.None;
            else if (touch2 == control.Joystick) touch2 = control.None;
        }

        if (touchCount > 0)
        {
            if (!joystick.gameObject.activeSelf)
            {
                TouchDrag(Input.GetTouch(0), true);
                return;
            }
            if (touch1 != control.Drag && (joystick.touched || joystick.OnStick(Input.GetTouch(0).position)))
            {
                touch1 = control.Joystick;

                joystick.touched = true;
                joystick.touchInput = Input.GetTouch(0);
            }
            else if (touch1 != control.Joystick && !joystick.OnStick(Input.GetTouch(0).position))
            {
                touch1 = control.Drag;

                TouchDrag(Input.GetTouch(0), true);
            }
            
            if (touch1 != control.Joystick && touch2 != control.Drag && (joystick.touched || joystick.OnStick(Input.GetTouch(1).position)))
            {
                touch2 = control.Joystick;

                joystick.touched = true;
                joystick.touchInput = Input.GetTouch(1);
            } else if (touch1 != control.Drag && touch2 != control.Joystick && !joystick.OnStick(Input.GetTouch(1).position))
            {
                touch2 = control.Drag;

                TouchDrag(Input.GetTouch(1), true);
            }

            if (touch1 != control.Drag && touch2 != control.Drag)
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
            else if (touch2 == control.Drag) touch2 = control.None;
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
            if (rotation.magnitude > 20) rotation = rotation.normalized * 20;
            transform.eulerAngles += rotation;

            prevPos = touchDrag.position;

        }
        transform.position = player.position;
    }
}

enum control
{
    None,
    Drag,
    Joystick
}
