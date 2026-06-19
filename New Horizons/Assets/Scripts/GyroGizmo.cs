using UnityEngine;

public class GyroGizmo : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        transform.position = player.position;
        transform.forward = player.forward;
    }
}
