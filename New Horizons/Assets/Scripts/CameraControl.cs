using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform camera;

    Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        transform.position = player.position;

        transform.forward = player.forward;

        camera.LookAt(player.position);
    }
}
