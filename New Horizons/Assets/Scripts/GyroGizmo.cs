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
        transform.forward = player.rotForwards;
    }
}
