using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float force;

    private void Start()
    {
        rb.AddForce(transform.forward * force);
    }
}
