using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float maxAngle = 10f;
    public Joystick joystick;

    [Space]
    public float resetTransformSpeed = 1f;

    private Player player;
    private CameraControl camControl;
    private GameObject pivot;
    
    private Quaternion prevRot;

    private void OnEnable()
    {
        joystick.gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        joystick.gameObject.SetActive(false);
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        camControl = FindFirstObjectByType<CameraControl>();
        pivot = new GameObject("pivot");
    }

    void Update()
    {
        Vector2 dir = joystick.Direction * maxAngle;
        if (dir.magnitude <= 0.1f)
        {
            ResetRot();
        } else JoystickRotate();
    }

    private void JoystickRotate()
    {
        pivot.transform.position = player.position;

        transform.parent = pivot.transform;

        Vector2 dir = joystick.Direction * maxAngle;
        Quaternion rotationAroundAxis = Quaternion.identity;
        
        rotationAroundAxis = Quaternion.AngleAxis(dir.y, player.right);
        rotationAroundAxis *= Quaternion.AngleAxis(-dir.x, player.forward);

        pivot.transform.rotation = Quaternion.Lerp(prevRot, rotationAroundAxis, Time.deltaTime);
        prevRot = pivot.transform.rotation;

        transform.parent = null;
    }

    private void ResetRot()
    {
        pivot.transform.position = Vector3.zero;

        player.transform.parent = pivot.transform;
        camControl.transform.parent = pivot.transform;
        transform.parent = pivot.transform;

        pivot.transform.position = Vector3.Lerp(pivot.transform.position, -transform.position, Time.deltaTime * resetTransformSpeed);
        pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.identity, Time.deltaTime * resetTransformSpeed);

        player.transform.parent = null;
        camControl.transform.parent = null;
        transform.parent = null;
    }
}
