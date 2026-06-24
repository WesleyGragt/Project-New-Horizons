using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Gyro : MonoBehaviour
{
    //If time, combine AcceleroMeter with Gyroscope
    //Check out sensor fusion to combine both!!!!
    public float maxAngle = 45f;
    public float smoothSpeed = 1f;
    public float updateSpeed = 1f;
    public float resetTransformSpeed = 1f;

    private Vector3 smoothAcceleration = Vector3.zero;

    private Player player;
    private GameObject pivot;

    private Vector3 startRot = Vector3.zero;
    private Vector3 acceleration;

    private Quaternion prevRot;

    private Vector3 lowPassValue;
    float LowPassFilterFactor = 1 / 60 / 1;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        pivot = new GameObject("GyroPivot");
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

        pivot.transform.position = player.position;

        AcceleroMeter();
        //ResetDisplacement(); Fucked with camera. For this we will hold a council of me, myself and I. The council has decided to execute order 66.
    }

    private void AcceleroMeter()
    {
        if (startRot == Vector3.zero) startRot = -Input.acceleration;

        pivot.transform.position = player.position; //Vector3.Lerp(pivot.transform.position, player.position, Time.deltaTime);
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

        float rotationInputX = Mathf.Abs(smoothAcceleration.x) * -Mathf.Sign(smoothAcceleration.x);
        float rotationInputZ = Mathf.Abs(smoothAcceleration.z) * -Mathf.Sign(smoothAcceleration.z);
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

    private void ResetDisplacement()
    {
        float dot = (acceleration.x * Vector3.up.x) + (acceleration.y * Vector3.up.y) + (acceleration.z * Vector3.up.z);
        float dotMag = acceleration.magnitude * Vector3.up.magnitude;
        float angle = Mathf.Acos(dot / dotMag);

        if (angle < 10)
        {
            pivot.transform.position = Vector3.zero;

            player.transform.parent = pivot.transform;
            transform.parent = pivot.transform;

            //pivot.transform.position = Vector3.Lerp(pivot.transform.position, -transform.position, Time.deltaTime * resetTransformSpeed);
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.identity, Time.deltaTime * resetTransformSpeed);

            player.transform.parent = null;
            transform.parent = null;
        }
    }
}

