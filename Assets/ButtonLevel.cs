using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevel : MonoBehaviour
{
    public static bool isChordMode;
    public static bool isWindMode;

    public void OpenScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void URl(string url)
    {
        Application.OpenURL(url);
    }
}
