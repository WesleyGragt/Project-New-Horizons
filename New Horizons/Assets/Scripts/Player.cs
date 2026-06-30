using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bumbParticle;
    public float speed;
    public Transform cam;

    [HideInInspector] public Vector3 forwards;
    [HideInInspector] public Vector3 rotForwards;
    [HideInInspector] public Vector3 rotRight;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float maxVelocity;
    

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (rb == null) rb = GetComponent<Rigidbody>();

        rotForwards = cam.transform.forward;
        rotRight = cam.transform.right;
        rb.maxLinearVelocity = 100f;
        maxVelocity = 100f;
    }

    private void Update()
    {
        rotForwards = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        rotRight = -Vector3.Cross(rotForwards, Vector3.up);
        
        if (rb.linearVelocity.magnitude > 0.1)
        {
            forwards = rb.linearVelocity;
            forwards.y = 0;
            forwards = forwards.normalized;
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        if (rb.linearVelocity == Vector3.zero)
        {
            rb.AddForce(Vector3.down);
        }

        if (velocity.y < .1f) rb.AddForce(Vector3.down * speed);
        else rb.AddForce(Vector3.down / 5 * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.magnitude < velocity.magnitude * .95)
        {
            var particle = Instantiate(bumbParticle, collision.contacts[0].point, Quaternion.identity);
            particle.transform.localScale *= 3;
            Destroy(particle, 3);

            audioSource.Play();
        }
    }
}
