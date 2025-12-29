using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed!");
        SceneManager.LoadScene("GAME");
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }
}
