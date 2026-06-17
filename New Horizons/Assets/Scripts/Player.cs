using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Vector3 right;
    [HideInInspector] public Vector3 velocity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        forward = transform.forward;
        right = transform.right;
    }

    private void Update()
    {
        if (rb.linearVelocity == Vector3.zero)
        {
            rb.AddForce(Vector3.down);
        }

        position = transform.position;
        
        if (rb.linearVelocity.magnitude > 0.1)
        {
            forward = rb.linearVelocity;
            forward.y = 0;
            forward = forward.normalized;

            right = -Vector3.Cross(forward, Vector3.up);
        }
     
        velocity = rb.linearVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(position, right);
    }
}
