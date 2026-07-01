using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [Header("Scene")]
    public string nextLevelName;
    [Header("Player")]
    public GameObject playerCamObj;
    public LevelManager levelManager;
    [Header("Sound")]
    public AudioSource finishSound;
    [Header("UI")]
    public GameObject finishMenuObj;
    public float finishMenuPopupDelay;
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI timeText;

    public void Finish()
    {
        playerCamObj.SetActive(false);
        finishSound.Play();
        StartCoroutine(FinishMenuPopup());
        timeText.text = levelManager.timeSinceStart.ToString("#");
    }

    IEnumerator FinishMenuPopup()
    {
        yield return new WaitForSeconds(finishMenuPopupDelay);

        finishMenuObj.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }
}
