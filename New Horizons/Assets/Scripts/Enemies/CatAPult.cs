using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class CatAPult : MonoBehaviour
{
    [Header("Movement")]
    public float chaseSpeed;
    public float homeSpeed;
    public float rotSpeed;
    [Header("Targets")]
    public Transform home;
    public Player player;
    Transform curTarget;
    public float targetReachedDistance;
    public float homeDelay;
    [Header("Flipper")]
    public HingeJoint joint;
    public float playerReachedDistance;
    public float flipBackDelay;
    [Header("Components")]
    public Rigidbody rigidbody;
    [Header("Animation")]
    public Animator animator;
    [Header("States")]
    public bool isAlerted;
    public bool isChasing;
    public bool isHoming;
    public bool isFlipping;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindFirstObjectByType<Player>();
        }
        curTarget = home;
    }

    private void Update()
    {
        Vector3 realTarget = new Vector3(curTarget.position.x, transform.position.y, curTarget.position.z);
        if ((isAlerted || isChasing || isHoming) && !isFlipping)
        {
            RotateTowards(realTarget);
        }
        if (isChasing && !isFlipping)
        {
            transform.Translate(0,0,chaseSpeed * Time.deltaTime);
            if(Vector3.Distance(curTarget.position, transform.position) <= playerReachedDistance)
            {
                Flip();
            }
        }
        else if (isHoming && !isFlipping)
        {
            transform.Translate(0,0,homeSpeed * Time.deltaTime);
            if (Vector3.Distance(curTarget.position, transform.position) <= targetReachedDistance)
            {
                isHoming = false;
                animator.SetBool("Home", false);
            }
        }
    }

    public void SetAlert(bool alert)
    {
        isAlerted = alert;
        curTarget = player.transform;
        if (alert)
        {
            //play alert effect
            animator.SetBool("Alert", true);
            //play alert sound
        }
        else
        {
            animator.SetBool("Alert", false);
        }
    }

    public void SetChasing(bool chasing)
    {
        isChasing = chasing;
        SetAlert(!chasing);

        if (chasing)
        {
            animator.SetBool("Chase", true);
            //start move sound
            curTarget = player.transform;
        }
        else
        {
            animator.SetBool("Chase", false);
            StartCoroutine(GoHome());
        }
    }

    IEnumerator GoHome()
    {
        yield return new WaitForSeconds(homeDelay);
        curTarget = home;
        isHoming = true;
        animator.SetBool("Home", true);
    }

    void RotateTowards(Vector3 rotTarget)
    {
        Vector3 targetDirection = rotTarget - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void Flip()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        animator.enabled = false;
        joint.useSpring = true;
        isFlipping = true;
        StartCoroutine(FlipBack());
    }

    IEnumerator FlipBack()
    {
        yield return new WaitForSeconds(flipBackDelay);
        joint.useSpring = false;
        isFlipping = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        StartCoroutine(FlipBackDone());
    }

    IEnumerator FlipBackDone()
    {
        yield return new WaitForSeconds(flipBackDelay * 2);
        animator.enabled = true;

    }
}
