using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Vector3 velocity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.linearVelocity == Vector3.zero)
        {
            rb.AddForce(Vector3.down);
        }

        position = transform.position;
        forward = rb.linearVelocity.normalized;
        velocity = rb.linearVelocity;

        Debug.Log(velocity.magnitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, velocity);
    }
}
