using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public static void Quit()
    {
        Application.Quit();
    }

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

    public static void LoadScene(Scene scene) => LoadScene(scene.name);
}
