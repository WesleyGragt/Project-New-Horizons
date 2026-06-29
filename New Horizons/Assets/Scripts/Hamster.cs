using UnityEditor.Animations;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    public AnimatorController animator;
    public float maxAngle = 22.5f;

    private Transform playerTrans;
    private Player playerScript;

    private void Start()
    {
        playerScript = FindFirstObjectByType<Player>();
        playerTrans = playerScript.transform;
    }

    private void Update()
    {
        transform.position = playerTrans.position;

        float angle = (playerScript.velocity.magnitude / playerScript.maxVelocity) * maxAngle;

        Vector3 rotation = transform.eulerAngles;
        rotation.x = -angle;

        //animator.parameters value: Speed, needs float input instead of int

        transform.eulerAngles = rotation;
        transform.forward = Vector3.Lerp(transform.forward, playerScript.forwards, Time.deltaTime*4);
    }
}
