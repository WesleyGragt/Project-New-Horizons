using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Gyro : MonoBehaviour
{
    //If time, combine AcceleroMeter with Gyroscope
    //Check out sensor fusion to combine both!!!!

    public Transform fakeAccel;
    public float smoothSpeed = 1f;

    private Vector3 smoothAcceleration = Vector3.zero;

    private Player player;
    private CameraControl camControl;
    private GameObject pivot;

    private Vector3 startPos = Vector3.zero;
    private Vector3 startRot = Vector3.zero;
    private Vector3 acceleration;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        camControl = FindFirstObjectByType<CameraControl>();

        startPos = transform.position;
    }

    private void OnEnable()
    {
        if (!SystemInfo.supportsGyroscope) Debug.Log("Device does not support Gyroscope!");
        if (!SystemInfo.supportsAccelerometer) Debug.Log("Device does not support AcceleroMeter!");

        Input.gyro.enabled = true;
        InputSystem.EnableDevice(Accelerometer.current);
    }

    private void OnDisable()
    {
        Input.gyro.enabled = false;
        InputSystem.DisableDevice(Accelerometer.current);
    }

    private void FixedUpdate()
    {
        if (pivot == null) pivot = new GameObject("GyroPivot");
        pivot.transform.position = player.position;

        AcceleroMeter();
        ResetDisplacement();
    }

    private void AcceleroMeter()
    {
        if (startRot == Vector3.zero) startRot = -Input.acceleration;

        acceleration = -Input.acceleration - startRot;

        smoothAcceleration = Vector3.Lerp(smoothAcceleration, acceleration, smoothSpeed * Time.deltaTime);

        float rotationInputX = Mathf.Abs(smoothAcceleration.x) * -Mathf.Sign(smoothAcceleration.x);
        float rotationInputZ = Mathf.Abs(smoothAcceleration.z) * -Mathf.Sign(smoothAcceleration.z);

        Quaternion rotation = Quaternion.Euler(-rotationInputZ * 90, 0, -rotationInputX * 90);

        pivot.transform.position = player.position;

        transform.parent = pivot.transform;
        pivot.transform.rotation = rotation;
        transform.parent = null;
    }

    private void ResetDisplacement()
    {
        float dot = (acceleration.x * Vector3.up.x) + (acceleration.y * Vector3.up.y) + (acceleration.z * Vector3.up.z);
        float dotMag = acceleration.magnitude * Vector3.up.magnitude;
        float angle = Mathf.Acos(dot / dotMag);

        if (angle < 3)
        {
            pivot.transform.position = Vector3.zero;

            player.transform.parent = pivot.transform;
            transform.parent = pivot.transform;

            pivot.transform.position = -transform.position;

            player.transform.parent = null;
            transform.parent = null;
        }
    }
}
