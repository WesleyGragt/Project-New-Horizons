using System.Collections;
using UnityEngine;

public class CatAPult : MonoBehaviour
{
    [Header("Movement")]
    public float chaseSpeed;
    public float homeSpeed;
    public float rotSpeed;
    [Header("Targets")]
    public Transform home;
    //public Player player;
    public Transform player;
    Transform curTarget;
    public float targetReachedDistance;
    public float homeDelay;
    [Header("Flipper")]
    public HingeJoint joint;
    public float playerReachedDistance;
    public float flipBackDelay;
    [Header("Components")]
    public Rigidbody rigidbody;
    [Header("States")]
    public bool isAlerted;
    public bool isChasing;
    public bool isHoming;

    private void Start()
    {
        if(player == null)
        {
            //player = GameObject.FindFirstObjectByType<Player>();
        }
        curTarget = home;
    }

    private void Update()
    {
        Vector3 realTarget = new Vector3(curTarget.position.x, transform.position.y, curTarget.position.z);
        if (isAlerted || isChasing || isHoming)
        {
            RotateTowards(realTarget);
        }
        if (isChasing)
        {
            transform.Translate(0,0,chaseSpeed * Time.deltaTime);
            if(Vector3.Distance(curTarget.position, transform.position) <= playerReachedDistance)
            {
                Flip();
            }
        }
        else if (isHoming)
        {
            transform.Translate(0,0,homeSpeed * Time.deltaTime);
            if (Vector3.Distance(curTarget.position, transform.position) <= targetReachedDistance)
            {
                isHoming = false;
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
            //play alert anim
            //play alert sound
        }
    }

    public void SetChasing(bool chasing)
    {
        isChasing = chasing;
        SetAlert(!chasing);

        if (chasing)
        {
            //start move anim
            //start move sound
            curTarget = player.transform;
        }
        else
        {
            //stop move anim
            StartCoroutine(GoHome());
        }
    }

    IEnumerator GoHome()
    {
        yield return new WaitForSeconds(homeDelay);
        curTarget = home;
        isHoming = true;
        //start move anim
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
        joint.useSpring = true;
        StartCoroutine(FlipBack());
    }

    IEnumerator FlipBack()
    {
        yield return new WaitForSeconds(flipBackDelay);
        joint.useSpring = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

    }
}
