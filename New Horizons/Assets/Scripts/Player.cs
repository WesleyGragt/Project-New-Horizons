using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject camController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource bonkAudio;
    [SerializeField] private AudioSource rollingAudio;
    [SerializeField] private GameObject bumbParticle;
    [SerializeField] private GameObject starParticle;
    public float speed;
    public float maxVelocity = 10f;
    public float respawnTime = 2f;
    public Transform cam;

    [HideInInspector] public Vector3 forwards;
    [HideInInspector] public Vector3 rotForwards;
    [HideInInspector] public Vector3 rotRight;
    [HideInInspector] public Vector3 velocity;
    
    float timer;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rotForwards = cam.transform.forward;
        rotRight = cam.transform.right;
        //rb.maxLinearVelocity = maxVelocity;
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
        else rb.angularVelocity *= 0.9f;

        Audio();
        Respawner();
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
        if (collision.transform.tag == "Pinpaw")
        {
            collision.transform.GetComponent<PinPaw>().Bounce(Vector3.Normalize(transform.position - collision.contacts[0].point));
        }

        if (rb.linearVelocity.magnitude < velocity.magnitude * .95)
        {
            var puffParticle = Instantiate(bumbParticle, collision.contacts[0].point, Quaternion.identity);
            var startParticle = Instantiate(starParticle, collision.contacts[0].point, Quaternion.identity);
            Destroy(puffParticle, 3);
            Destroy(startParticle, 3);

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
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo, 10))
        {
            if (hitInfo.transform.tag == "CatAPult")
            {
                if (Time.time > timer)
                {
                    camController.SetActive(false);
                    StartCoroutine(ReloadLevel(2));
                }
            } else timer = Time.time + respawnTime;
        }
        else
        {
            if (Time.time > timer)
            {
                camController.SetActive(false);
                StartCoroutine(ReloadLevel(2));
            }
        }
    }

    private IEnumerator ReloadLevel(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
