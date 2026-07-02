using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI timeText;
    public int seedsColleted;
    public float timeSinceStart;


    private void Update()
    {
        timeSinceStart += Time.deltaTime;
        if (timeText != null)
        {
            timeText.text = timeSinceStart.ToString("#") + "s";
        }

    }

    public void SeedGet()
    {
        seedsColleted++;
        if(seedText != null)
        {
            seedText.text = seedsColleted.ToString();
        }
    }
}
