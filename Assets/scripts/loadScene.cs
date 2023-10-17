using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [System.Serializable]
    public class SceneLoadEvent : UnityEvent<string> { }

    // Remove the initialSceneToLoad variable since we want to load the menu scene.

    [Header("Events")]
    public SceneLoadEvent onSceneLoadRequested;

    // Remove the Start function, which was causing the automatic scene load.

    public void LoadScene(string sceneName)
    {
        // Raise the event to allow control of the scene to load from the Inspector
        onSceneLoadRequested.Invoke(sceneName);

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
