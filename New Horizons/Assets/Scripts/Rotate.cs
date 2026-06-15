using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float maxAngle = 10f;

    private Player player;
    private GameObject pivot;
    
    private Vector3 rotation;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            rotation.x = maxAngle;
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            rotation.x = -maxAngle;
        }
        else rotation.x = 0;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rotation.z = maxAngle;
        }
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            rotation.z = -maxAngle;
        }
        else rotation.z = 0;

        ParentRotate();
    }

    private void ParentRotate()
    {
        //create empty on player pos and rotate
        if (pivot == null) pivot = new GameObject("pivot");
        pivot.transform.position = player.position;

        transform.parent = pivot.transform;

        pivot.transform.eulerAngles = rotation;

        transform.parent = null;

        //fix position after rotation
        if (rotation == Vector3.zero)
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
