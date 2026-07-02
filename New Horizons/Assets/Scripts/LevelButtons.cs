using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtons : MonoBehaviour
{
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void MenuLevel()
    {
        SceneManager.LoadScene(0);
    }
}
