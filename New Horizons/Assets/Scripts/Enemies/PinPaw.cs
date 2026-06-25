using System.Collections;
using UnityEngine;

public class PinPaw : MonoBehaviour
{
    [Header("Movement")]
    public float rotSpeed;
    [Header("Bounce")]
    public float force;
    [Header("Targets")]
    //public Player player;
    public Transform player;
    [Header("Animation")]
    public Animator animator;
    [Header("Effects")]
    public ParticleSystem bonkParticle;
    public float effectDelay;
    [Header("States")]
    public bool isAlerted;

    private void Start()
    {
        if (player == null)
        {
            //player = GameObject.FindFirstObjectByType<Player>();
        }
    }

    private void Update()
    {
        if (isAlerted)
        {
            RotateTowards(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    public void SetAlert(bool alert)
    {
        isAlerted = alert;
        if (alert)
        {
            //play alert effect
            animator.SetTrigger("Alerted");
            //play alert sound
        }
        else
        {
        }
    }

    public void Bounce(Vector3 dir)
    {
        animator.SetTrigger("Bounce");
        player.GetComponent<Rigidbody>().AddForce(dir * force);
        //player.rb.AddForce(dir * force);
        StartCoroutine(DelayedEffects());
    }

    void RotateTowards(Vector3 rotTarget)
    {
        Vector3 targetDirection = rotTarget - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    IEnumerator DelayedEffects()
    {
        yield return new WaitForSeconds(effectDelay);
        bonkParticle.Play();
        //play bonk sound
        
    }
}
