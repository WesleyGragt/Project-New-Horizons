using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyAfterTime;

    private void Update()
    {
        destroyAfterTime -= Time.deltaTime;

        if(destroyAfterTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
