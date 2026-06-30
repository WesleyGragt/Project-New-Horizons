using UnityEngine;

public class GyroGizmo : MonoBehaviour
{
    [SerializeField] Player player;

    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        transform.forward = player.rotForwards;
    }
}
