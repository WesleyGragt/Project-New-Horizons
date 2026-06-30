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
        seedText.text = seedsColleted.ToString();
    }
}
