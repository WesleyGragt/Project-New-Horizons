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
    Control touch1 = Control.None;
    Control touch2 = Control.None;
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

        if (touch1 == Control.None && touch2 != Control.None)
        {
            touch1 = touch2;
            touch2 = Control.None;
        }

        if (touch1 == Control.Drag && touch2 == Control.Joystick) invertTourch = true;

        if (joystick.touchEnd)
        {
            if (touch1 == Control.Joystick) touch1 = Control.None;
            else if (touch2 == Control.Joystick) touch2 = Control.None;
        }

        if (touchCount > 0)
        {
            if (!joystick.gameObject.activeSelf)
            {
                TouchDrag(Input.GetTouch(0));
                return;
            }

            if (touch1 != Control.Joystick && touch2 != Control.Drag && !joystick.OnStick(Input.GetTouch(0).position))
            {
                touch1 = Control.Drag;

                TouchDrag(Input.GetTouch(0));
            }
            else if (touch1 != Control.Drag && touch2 != Control.Joystick && (joystick.touched || joystick.OnStick(Input.GetTouch(0).position)))
            {
                touch1 = Control.Joystick;
                
                joystick.touched = true;
                
                joystick.touchInput = Input.GetTouch(0);
            }

            if (touch1 != Control.Drag && touch2 != Control.Drag)
            {
                transform.forward = Vector3.Lerp(transform.forward, player.forwards, Time.deltaTime);
            }

            if (touchCount < 2) return;
            if (touch1 != Control.Joystick && touch2 != Control.Drag && (joystick.touched || joystick.OnStick(Input.GetTouch(1).position)))
            {
                touch2 = Control.Joystick;
                
                joystick.touched = true;
                joystick.touchInput = Input.GetTouch(1);
            }
            else if (touch1 != Control.Drag && touch2 != Control.Joystick && !joystick.OnStick(Input.GetTouch(1).position))
            {
                touch2 = Control.Drag;

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
            touch1 = Control.None;
            touch2 = Control.None;
            invertTourch = false;
        }
    }

    private void TouchDrag(Touch touchDrag)
    {
        if (touchDrag.phase == TouchPhase.Ended || touchDrag.phase == TouchPhase.Canceled)
        {
            if (touch1 == Control.Drag) touch1 = Control.None;
            else if (touch2 == Control.Drag) touch2 = Control.None;
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

enum Control
{
    None,
    Drag,
    Joystick
}
