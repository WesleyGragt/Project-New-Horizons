using Unity.VisualScripting;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public GameObject pivot;
    public float maxAngle = 22.5f;
    public Joystick joystick;
    [Space]
    public float updateSpeed = 2f;
    public float resetTransformSpeed = 3f;

    private Player player;
    
    private Quaternion prevRot;

    private bool resetRot = false;

    private void OnEnable()
    {
        joystick.gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        joystick.gameObject.SetActive(false);
        ResetRot();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (resetRot) ResetRot();
        else JoystickRotate();
    }

    private void JoystickRotate()
    {
        pivot.transform.position = player.transform.position;

        transform.parent = pivot.transform;

        Vector2 dir = joystick.Direction * maxAngle;
        Quaternion rotationAroundAxis = Quaternion.identity;
        
        rotationAroundAxis = Quaternion.AngleAxis(dir.y, player.rotRight);
        rotationAroundAxis *= Quaternion.AngleAxis(-dir.x, player.rotForwards);

        pivot.transform.rotation = Quaternion.Lerp(prevRot, rotationAroundAxis, Time.deltaTime * updateSpeed);
        prevRot = pivot.transform.rotation;

        transform.parent = null;
    }

    public void zeroRotation()
    {
        resetRot = true;
    }

    private void ResetRot()
    {
        pivot.transform.position = Vector3.zero;

        player.transform.parent = pivot.transform;
        transform.parent = pivot.transform;

        //pivot.transform.position = Vector3.Lerp(pivot.transform.position, -transform.position, Time.deltaTime * resetTransformSpeed);
        pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.identity, Time.deltaTime * resetTransformSpeed);

        if (pivot.transform.eulerAngles.magnitude < 0.001f) resetRot = false;

        player.transform.parent = null;
        transform.parent = null;
    }
}
