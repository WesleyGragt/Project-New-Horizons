using TMPro;
using UnityEngine;

public class MovementControlCam : MonoBehaviour
{
    //Automatic camControl
    [SerializeField] private Player player;
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
    bool invertTourch = false;


    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        transform.position = player.transform.position; //Vector3.Lerp(transform.position, player.position, Time.deltaTime * 5);

        //FollowPlayer();
        ScreenInput();
    }

    private void FollowPlayer()
    {
        Vector3 offset = player.transform.position - cam.position;

        float distance = offset.magnitude;

        if (distance < minDistance) transform.position = Vector3.Lerp(transform.position, transform.position - offset.normalized * (minDistance - distance), Time.deltaTime);
        cam.LookAt(player.transform.position);
    }

    private void ScreenInput()
    {
        int touchCount = Input.touchCount;

        if (touch1 == control.None && touch2 != control.None)
        {
            touch1 = touch2;
            touch2 = control.None;
        }

        if (touch1 == control.Drag && touch2 == control.Joystick) invertTourch = true;

        if (joystick.touchEnd)
        {
            if (touch1 == control.Joystick) touch1 = control.None;
            else if (touch2 == control.Joystick) touch2 = control.None;
        }

        if (touchCount > 0)
        {
            if (!joystick.gameObject.activeSelf)
            {
                TouchDrag(Input.GetTouch(0));
                return;
            }

            if (touch1 != control.Joystick && touch2 != control.Drag && !joystick.OnStick(Input.GetTouch(0).position))
            {
                touch1 = control.Drag;

                TouchDrag(Input.GetTouch(0));
            }
            else if (touch1 != control.Drag && touch2 != control.Joystick && (joystick.touched || joystick.OnStick(Input.GetTouch(0).position)))
            {
                touch1 = control.Joystick;
                
                joystick.touched = true;
                
                joystick.touchInput = Input.GetTouch(0);
            }

            if (touch1 != control.Drag && touch2 != control.Drag)
            {
                transform.forward = Vector3.Lerp(transform.forward, player.forwards, Time.deltaTime);
            }

            if (touchCount < 2) return;
            if (touch1 != control.Joystick && touch2 != control.Drag && (joystick.touched || joystick.OnStick(Input.GetTouch(1).position)))
            {
                touch2 = control.Joystick;
                
                joystick.touched = true;
                joystick.touchInput = Input.GetTouch(1);
            }
            else if (touch1 != control.Drag && touch2 != control.Joystick && !joystick.OnStick(Input.GetTouch(1).position))
            {
                touch2 = control.Drag;

                if (invertTourch)
                {
                    joystick.touchInput = Input.GetTouch(1);
                    TouchDrag(Input.GetTouch(0));
                } else
                {
                    joystick.touchInput = Input.GetTouch(0);
                    TouchDrag(Input.GetTouch(1));
                }
            }
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, player.forwards, Time.deltaTime);
            touch1 = control.None;
            touch2 = control.None;
            invertTourch = false;
        }
    }

    private void TouchDrag(Touch touchDrag)
    {
        if (touchDrag.phase == TouchPhase.Ended || touchDrag.phase == TouchPhase.Canceled)
        {
            if (touch1 == control.Drag) touch1 = control.None;
            else if (touch2 == control.Drag) touch2 = control.None;
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
    }
}

enum control
{
    None,
    Drag,
    Joystick
}
