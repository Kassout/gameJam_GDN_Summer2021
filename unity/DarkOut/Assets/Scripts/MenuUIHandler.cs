#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    /// <summary>
    /// This method is used to start the game from the main menu.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    
    /// <summary>
    /// This method is used to stop and close the game.
    /// </summary>
    public void Exit()
    {
        // pre-compilation tasks, if in unity editor compile code to exit play mode,
        // else compile to quit the application.
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}