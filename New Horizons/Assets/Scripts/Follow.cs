using UnityEngine;

public class Follow : MonoBehaviour
{
    public float distance;
    public float height;
    
    Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        Vector3 startPos = player.transform.forward - (player.forward * distance);
        startPos.y += height;
        transform.position = startPos;
    }

    private void Update()
    {
        transform.LookAt(player.position);
        
        if (player.velocity.magnitude <= .001f) return;

        Vector3 pos = player.position - (player.forward * distance);
        pos.y += height;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
