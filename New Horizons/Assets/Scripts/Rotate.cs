using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float maxAngle = 10f;
    public Joystick joystick;

    private Player player;
    private GameObject pivot;
    
    private Vector3 rotation;

    private Quaternion prevRot;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        ParentRotate();
    }

    private void ParentRotate()
    {
        //create empty on player pos and rotate
        if (pivot == null) pivot = new GameObject("pivot");
        pivot.transform.position = player.position;

        transform.parent = pivot.transform;

        Vector2 dir = joystick.Direction * maxAngle;
        Quaternion rotationAroundAxis = Quaternion.identity;
        
        rotationAroundAxis = Quaternion.AngleAxis(dir.y, player.right);
        rotationAroundAxis *= Quaternion.AngleAxis(-dir.x, player.forward);

        pivot.transform.rotation = Quaternion.Lerp(prevRot, rotationAroundAxis, Time.deltaTime);
        prevRot = pivot.transform.rotation;

        transform.parent = null;

        //fix position after rotation
        if (dir.magnitude <= 0.1f)
        {
            pivot.transform.position = Vector3.zero;

            player.transform.parent = pivot.transform;
            transform.parent = pivot.transform;

            pivot.transform.position = -transform.position;

            player.transform.parent = null;
            transform.parent = null;
        }
    }
}
