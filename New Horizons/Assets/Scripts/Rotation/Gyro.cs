using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Gyro : MonoBehaviour
{
    //If time, combine AcceleroMeter with Gyroscope
    //Check out sensor fusion to combine both!!!!
    public GameObject pivot;
    [Range(0, 1)] public float sensitivity = 1f;
    public float maxAngle = 45f;
    public float smoothSpeed = 1f;
    public float updateSpeed = 2f;
    public float resetTransformSpeed = 3f;

    private Vector3 smoothAcceleration = Vector3.zero;

    private Player player;

    private Vector3 startRot = Vector3.zero;
    private Vector3 acceleration;

    private Quaternion prevRot;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;

    private bool resetRot = false;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        lowPassValue = Input.acceleration;
    }

    private void OnEnable()
    {
        if (!SystemInfo.supportsAccelerometer) Debug.Log("Device does not support AcceleroMeter!");

        Input.gyro.enabled = true;
        InputSystem.EnableDevice(Accelerometer.current);
    }

    private void OnDisable()
    {
        Input.gyro.enabled = false;
        InputSystem.DisableDevice(Accelerometer.current);
    }

    private void Update()
    {
        if (!SystemInfo.supportsAccelerometer) return;

        pivot.transform.position = player.transform.position;

        AcceleroMeter();
        if (resetRot) ResetRot();
        Debug.Log(resetRot);
        //ResetDisplacement(); Fucked with camera. For this we will hold a council of me, myself and I. The council has decided to execute order 66.
    }

    private void AcceleroMeter()
    {
        if (startRot == Vector3.zero) startRot = -Input.acceleration;

        pivot.transform.position = player.transform.position; //Vector3.Lerp(pivot.transform.position, player.position, Time.deltaTime);
        transform.parent = pivot.transform;

        Vector3 acc = Vector3.zero;
        float period = 0.0f;

        foreach (AccelerationEvent evnt in Input.accelerationEvents)
        {
            acc += evnt.acceleration * evnt.deltaTime;
            period += evnt.deltaTime;
        }
        if (period > 0)
        {
            acc *= 1.0f / period;
        }

        acceleration = -acc - startRot;

        smoothAcceleration = Vector3.Lerp(smoothAcceleration, acceleration, smoothSpeed * Time.deltaTime);

        float rotationInputX = Mathf.Abs(smoothAcceleration.x * sensitivity) * -Mathf.Sign(smoothAcceleration.x * sensitivity);
        float rotationInputZ = Mathf.Abs(smoothAcceleration.z * sensitivity) * -Mathf.Sign(smoothAcceleration.z * sensitivity);
        Vector3 rotation = new Vector3(-rotationInputZ * 90, 0, -rotationInputX * 90);

        if (rotation.x > maxAngle) rotation.x = maxAngle;
        else if (rotation.x < -maxAngle) rotation.x = -maxAngle;
        if (rotation.z > maxAngle) rotation.z = maxAngle;
        else if (rotation.z < -maxAngle) rotation.z = -maxAngle;

        Quaternion rotationAroundAxis = Quaternion.AngleAxis(rotation.x, player.rotRight);
        rotationAroundAxis *= Quaternion.AngleAxis(rotation.z, player.rotForwards);

        pivot.transform.rotation = Quaternion.Lerp(prevRot, rotationAroundAxis, Time.deltaTime * updateSpeed);
        prevRot = pivot.transform.rotation;

        transform.parent = null;
    }

    Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
    {
        Vector3 newValue = Vector3.Lerp(prevValue, Input.acceleration, lowPassFilterFactor);
        return newValue;
    }

    public void SetRot()
    {
        startRot = -Input.acceleration;
    }

    public void SetSensitivity(float gyroSense)
    {
        sensitivity = gyroSense;
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
        //pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.identity, Time.deltaTime * resetTransformSpeed);
        pivot.transform.rotation = Quaternion.identity;

        if (pivot.transform.eulerAngles.magnitude < 0.001f) resetRot = false;

        player.transform.parent = null;
        transform.parent = null;
    }
}

