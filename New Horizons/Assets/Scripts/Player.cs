using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject camController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource bonkAudio;
    [SerializeField] private AudioSource rollingAudio;
    [SerializeField] private GameObject bumbParticle;
    public float speed;
    public float maxVelocity = 10f;
    public float respawnTime = 2f;
    public Transform cam;

    [HideInInspector] public Vector3 forwards;
    [HideInInspector] public Vector3 rotForwards;
    [HideInInspector] public Vector3 rotRight;
    [HideInInspector] public Vector3 velocity;
    
    bool timerActive = false;
    float respawnTimer;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (rb == null) rb = GetComponent<Rigidbody>();

        rotForwards = cam.transform.forward;
        rotRight = cam.transform.right;
        rb.maxLinearVelocity = maxVelocity;
    }

    private void Update()
    {
        rotForwards = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        rotRight = -Vector3.Cross(rotForwards, Vector3.up);

        if (velocity.y > -maxVelocity && velocity.y < -maxVelocity * .8f && !timerActive)
        {
            camController.SetActive(false);
            respawnTimer = Time.time + respawnTime;
            timerActive = true;
        }

        if (rb.linearVelocity.magnitude > 0.1)
        {
            forwards = rb.linearVelocity;
            forwards.y = 0;
            forwards = forwards.normalized;
        }
        else rb.angularVelocity *= 0.9f;

        Audio();
        if (timerActive) Respawner();
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
            Destroy(particle, 3);

            bonkAudio.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ColliderTrigger")
        {
            other.GetComponent<ColliderTrigger>().EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ColliderTrigger")
        {
            other.GetComponent<ColliderTrigger>().ExitTrigger();
        }
    }

    private void Audio()
    {
        if (velocity.magnitude > 1 && !rollingAudio.isPlaying)
        {
            rollingAudio.loop = true;
            rollingAudio.Play();
        }
        else if (velocity.magnitude < 1)
        {
            rollingAudio.loop = false;
            rollingAudio.Stop();
        }
    }
    private void Respawner()
    {
        if (Time.time > respawnTimer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
