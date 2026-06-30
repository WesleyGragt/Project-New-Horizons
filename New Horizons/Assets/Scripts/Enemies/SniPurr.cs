using UnityEngine;

public class SniPurr : MonoBehaviour
{
    [Header("Movement")]
    public float rotSpeed;
    [Header("Shooting")]
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float shootAnimStartOffset;
    public float shootCooldown;
    float curShootCooldown;
    public int bulletsPerBurst;
    int curBurstAmount;
    public float burstShotCooldown;
    float curBurstShotCooldown;
    [Header("Targets")]
    public Transform home;
    public Transform player;
    Transform curTarget;
    [Header("Effects")]
    public ParticleSystem suckParticle;
    public ParticleSystem shootParticle;
    [Header("Sounds")]
    public GameObject alertSound;
    public GameObject shootSound;
    public GameObject suckSound;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [Header("States")]
    public bool isAlerted;
    public bool isShooting;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindFirstObjectByType<Player>().transform;
        }
        curTarget = home;
        curBurstShotCooldown = burstShotCooldown;
    }

    private void Update()
    {
        if (isAlerted && !isShooting)
        {
            RotateTowards(curTarget.position);
            animator.SetBool("IsShooting", false);
        }
        if (isShooting)
        {
            //Vector3 predictedLoc = curTarget.position + (player.GetComponent<Rigidbody>().linearVelocity);
            RotateTowards(curTarget.position);
            curShootCooldown -= Time.deltaTime;
            if(curShootCooldown < shootAnimStartOffset)
            {
                animator.SetBool("IsShooting", true);

            }
            if (curShootCooldown < 0)
            {
                curBurstShotCooldown -= Time.deltaTime;
                if(curBurstShotCooldown < 0)
                {
                    curBurstShotCooldown = burstShotCooldown;
                    Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    Instantiate(shootSound, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    shootParticle.Play();
                    curBurstAmount++;
                    if(curBurstAmount >= bulletsPerBurst)
                    {
                        curBurstAmount = 0;
                        curShootCooldown = shootCooldown;
                        animator.SetBool("IsShooting", false);

                    }
                }
            }

        }
    }

    void RotateTowards(Vector3 rotTarget)
    {
        Vector3 targetDirection = rotTarget - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void SetAlert(bool alert)
    {
        isAlerted = alert;
        curTarget = player.transform;
        curShootCooldown = shootCooldown;
        if (alert)
        {
            //play alert effect
            animator.SetTrigger("Alerted");
            Instantiate(alertSound, transform.position, Quaternion.identity);
        }
        else
        {
        }
    }

    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
        SetAlert(!shooting);

        if (shooting)
        {
            curTarget = player.transform;
            Instantiate(suckSound, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            suckParticle.Play();

        }
        else
        {
        }
    }
}
