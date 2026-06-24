using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Transform cam;

    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 forwards;
    [HideInInspector] public Vector3 rotForwards;
    [HideInInspector] public Vector3 rotRight;
    [HideInInspector] public Vector3 velocity;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rotForwards = cam.transform.forward;
        rotRight = cam.transform.right;
    }

    private void Update()
    {
        position = transform.position;
        velocity = Vector3.Lerp(velocity, rb.linearVelocity, Time.deltaTime);

        if (rb.linearVelocity == Vector3.zero)
        {
            rb.AddForce(Vector3.down);
        }

        
        if (rb.linearVelocity.magnitude > 0.1)
        {
            forwards = rb.linearVelocity;
            forwards.y = 0;
            forwards = forwards.normalized;
            rotForwards = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;

            rotRight = -Vector3.Cross(rotForwards, Vector3.up);
        }

        if (velocity.y < .1f) rb.AddForce(Vector3.down * speed);
        else if (velocity.y > .1f && velocity.y < 3) velocity *= 0.9f;
        if (velocity.y > 3) rb.AddForce(-velocity.normalized * speed);

    }
}
