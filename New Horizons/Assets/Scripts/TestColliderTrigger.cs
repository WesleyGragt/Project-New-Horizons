using UnityEngine;

public class TestColliderTrigger : MonoBehaviour
{
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
