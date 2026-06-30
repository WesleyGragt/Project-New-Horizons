using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public GameObject hitParticle;

    private void Start()
    {
        rb.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts[0].point != null)
        {
            Instantiate(hitParticle, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
