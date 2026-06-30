using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TextMeshPro seedText;
    public int seedsColleted;

    public void SeedGet()
    {
        seedsColleted++;
        if (seedText != null) seedText.text = seedsColleted.ToString();
    }
}
