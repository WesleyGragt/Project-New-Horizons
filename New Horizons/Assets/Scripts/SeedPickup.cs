using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    public LevelManager levelManager;
    public GameObject pickUpParticle;
    public GameObject pickUpSound;

    private void Start()
    {
        if(levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }
    }

    public void PickUp()
    {
        Instantiate(pickUpParticle, transform.position, Quaternion.identity);
        Instantiate(pickUpSound, transform.position, Quaternion.identity);
        levelManager.SeedGet();
        Destroy(gameObject);
    }
}
