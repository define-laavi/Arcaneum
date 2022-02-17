using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    /// <summary>Quits the application</summary>
    public static void Quit()
    {
        Application.Quit();
    }

    /// <summary>Loads scene based on it's name, also, replaces exception call with warning </summary>
    public static void LoadScene(string scene)
    {
        try
        {
            SceneManager.LoadScene(scene);
        }
        catch
        {
            Debug.LogWarning($"Scene {scene} not loaded properly, make sure the scene is added to build settings!");
        }
    }

    /// <summary>Helper that loads the provided scene</summary>
    public static void LoadScene(Scene scene) => LoadScene(scene.name);
}
