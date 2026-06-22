using UnityEngine;

public class SniPurr : MonoBehaviour
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
    [Header("Animation")]
    public Animator animator;
    [Header("States")]
    public bool isAlerted;
    public bool isShooting;

    private void Start()
    {
        if (player == null)
        {
            //player = GameObject.FindFirstObjectByType<Player>();
        }
        curTarget = home;
    }

    private void Update()
    {
        if ((isAlerted) && !isShooting)
        {
            Vector3 realTarget = new Vector3(curTarget.position.x, transform.position.y, curTarget.position.z);
            RotateTowards(realTarget);
        }
        if (isShooting)
        {
            Vector3 realTarget = new Vector3(curTarget.position.x, transform.position.y, curTarget.position.z);
            RotateTowards(realTarget);
            
        }
    }

    void RotateTowards(Vector3 rotTarget)
    {
        Vector3 targetDirection = rotTarget - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
