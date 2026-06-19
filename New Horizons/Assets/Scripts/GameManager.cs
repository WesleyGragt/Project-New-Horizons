using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager gameManager;
    private GameControl gameControl;

    private void Start()
    {
        if (gameManager != null) Destroy(gameObject);
    }

    public void SetControls(GameControl controls)
    {
        gameControl = controls;
    }
    
    public GameControl GetControls()
    {
        return gameControl;
    }
}
public enum GameControl
{
    Joystick,
    Gyro
}