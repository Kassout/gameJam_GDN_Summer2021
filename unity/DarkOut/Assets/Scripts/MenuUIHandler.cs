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
    /// Instance variable <c>mainMenuTheme</c> represents the audio clip of the main menu scene.
    /// </summary>
    [SerializeField]
    private AudioSource mainMenuTheme;
    
    /// <summary>
    /// Static variable <c>MAIN_GAME_SCENE</c> represents the scene index of the main game scene.
    /// </summary>
    private static int MAIN_GAME_SCENE = 2;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        PlayMainMenuTheme();
    }

    /// <summary>
    /// This method is used to start the game from the main menu.
    /// </summary>
    public void StartGame()
    {
        StopMainMenuTheme();
        SceneManager.LoadScene(MAIN_GAME_SCENE);
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

    /// <summary>
    /// This method is called to start playing the main menu theme.
    /// </summary>
    private void PlayMainMenuTheme()
    {
        mainMenuTheme.Play();
    }

    /// <summary>
    /// This method is called to stop playing the main menu theme.
    /// </summary>
    private void StopMainMenuTheme()
    {
        mainMenuTheme.Stop();
    }
}