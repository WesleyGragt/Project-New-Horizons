using UnityEngine;

public class Hamster : MonoBehaviour
{
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

        transform.forward = Vector3.Lerp(transform.forward, playerScript.forwards, Time.deltaTime);
    }
}
