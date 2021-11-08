#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MainMenuUIHandler : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>mainMenuTheme</c> represents the audio clip of the main menu scene.
    /// </summary>
    [SerializeField]
    private AudioSource mainMenuTheme;

    /// <summary>
    /// Instance variable <c>mainMenu</c> represents the main UI menu of the main menu scene.
    /// </summary>
    [SerializeField]
    private GameObject mainMenu;

    /// <summary>
    /// Instance variable <c>settingsMenu</c> represents the settings UI menu of the main menu scene.
    /// </summary>
    [SerializeField]
    private GameObject settingsMenu;

    /// <summary>
    /// Instance variable <c>mainMenuWithSave</c> represents the main UI menu of the main menu scene when the player got a save data file of the game.
    /// </summary>
    [SerializeField]
    private GameObject mainMenuWithSave;

    /// <summary>
    /// Instance variable <c>volumeSlider</c> represents a <c>Slider</c> Unity UI component used to change the global volume of the game.
    /// </summary>
    [SerializeField]
    private Slider volumeSlider;

    /// <summary>
    /// Static variable <c>MAIN_GAME_SCENE</c> represents the scene index of the main game scene.
    /// </summary>
    [SerializeField]
    private int gameSceneToLoad = 2;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        PlayMainMenuTheme();
        OpenMainMenu();
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    /// <summary>
    /// This method is used to start the game from the main menu.
    /// </summary>
    public void StartGame()
    {
        StopMainMenuTheme();
        GameManager.Instance.LoadScene(gameSceneToLoad, false);
    }

    /// <summary>
    /// This method is called to open the settings UI menu.
    /// </summary>
    public void OpenSettings()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
        } else if (mainMenuWithSave.activeSelf)
        {
            mainMenuWithSave.SetActive(false);
        }
        settingsMenu.SetActive(true);
    }

    /// <summary>
    /// This method is called to open the main UI menu.
    /// </summary>
    public void OpenMainMenu()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            mainMenuWithSave.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
        }
    }

    /// <summary>
    /// This method is called to change the volume global parameters of the game.
    /// </summary>
    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
;    }
    
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

    /// <summary>
    /// This method is called to close the settings UI menu.
    /// </summary>
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        OpenMainMenu();
    }
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public void ContinueGame()
    {
        StopMainMenuTheme();
        GameManager.Instance.LoadLevelData();
    }
}