using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject windowManager;

    public void OpenThisWindow()
    {
        if (windowManager.active)
        {
            windowManager.SetActive(false);
        }
        else
        {
            windowManager.SetActive(true);
        }
    }

    public void DeliteWindow()
    {
        Destroy(windowManager);
    }
    public void ChangeScene(int num)
    {
        SceneManager.LoadScene(num);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
