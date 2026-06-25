using UnityEngine;

public class TestColliderTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Pinpaw")
        {
            collision.transform.GetComponent<PinPaw>().Bounce(Vector3.Normalize(transform.position - collision.contacts[0].point));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ColliderTrigger")
        {
            other.GetComponent<ColliderTrigger>().EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ColliderTrigger")
        {
            other.GetComponent<ColliderTrigger>().ExitTrigger();
        }
    }
}
