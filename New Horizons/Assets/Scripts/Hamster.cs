using UnityEngine;

public class Hamster : MonoBehaviour
{
    [SerializeField] private Player playerScript;
    [SerializeField] private ParticleSystem dustParticle;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject thickHamster;
    [SerializeField] private GameObject flatHamster;
    
    public float maxAngle = 22.5f;
    public float percFlatHamster = 90f;

    private Transform playerTrans;
    
    private void Start()
    {
        if (playerScript == null) playerScript = FindFirstObjectByType<Player>();
        playerTrans = playerScript.transform;
    }

    private void Update()
    {
        transform.position = playerTrans.position;

        DustParticle();

        if ((playerScript.velocity.magnitude / playerScript.maxVelocity) > percFlatHamster / 100)
        {
            thickHamster.SetActive(false);
            flatHamster.SetActive(true);

            transform.parent = playerTrans;
        }
        else
        {
            transform.parent = null;

            thickHamster.SetActive(true);
            flatHamster.SetActive(false);

            RunningHamster();
        }
    }

    private void RunningHamster()
    {
        float angle = (playerScript.velocity.magnitude / playerScript.maxVelocity) * maxAngle;

        Vector3 rotation = transform.eulerAngles;
        rotation.x = -angle;

        float animSpeed = (playerScript.velocity.magnitude / playerScript.maxVelocity) * 10;
        animator.SetFloat("Speed", animSpeed);

        transform.eulerAngles = rotation;
        transform.forward = Vector3.Lerp(transform.forward, playerScript.forwards, Time.deltaTime*4);
    }

    private void DustParticle()
    {
        Vector3 dustPos = transform.position;
        dustPos.y -= transform.lossyScale.x * 4;
        dustParticle.transform.position = dustPos;

        bool ground = false;

        if (Physics.Raycast(new Ray(transform.position, Vector3.down), transform.lossyScale.x * 6)) ground = true;

        if (playerScript.velocity.magnitude < 1 || !ground) dustParticle.Stop();
        else if (!dustParticle.isPlaying && ground) dustParticle.Play();
    }
}
