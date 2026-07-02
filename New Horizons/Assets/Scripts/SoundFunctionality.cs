using UnityEngine;

public class SoundFunctionality : MonoBehaviour
{
    public AudioSource audioSource;
    public float randomPitch;
    [Tooltip("0 = never destroy.")]
    public float destroyAfterTime;
    bool doDesroyAfterTime = false;

    private void Start()
    {
        audioSource.pitch += Random.Range(-randomPitch, randomPitch);
        if(destroyAfterTime != 0)
        {
            doDesroyAfterTime = true;
        }
    }

    private void Update()
    {
        if (doDesroyAfterTime)
        {
            destroyAfterTime -= Time.deltaTime;
            if(destroyAfterTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
