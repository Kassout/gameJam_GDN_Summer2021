#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MainMenuUIHandler : MonoBehaviour
{
    #region Fields / Properties

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
    /// Instance variable <c>mainStartFirstButton</c> is a Unity <c>GameObject</c> object representing the button to first select on start main menu set active.
    /// </summary>
    [SerializeField] private GameObject mainStartFirstButton;
    
    /// <summary>
    /// Instance variable <c>mainContinueFirstButton</c> is a Unity <c>GameObject</c> object representing the button to first select on continue main menu set active.
    /// </summary>
    [SerializeField] private GameObject mainContinueFirstButton;
    
    /// <summary>
    /// Instance variable <c>settingsFirstButton</c> is a Unity <c>GameObject</c> object representing the button to first select on settings menu set active.
    /// </summary>
    [SerializeField] private GameObject settingsFirstButton;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        PlayMainMenuTheme();
        OpenMainMenu();
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1.0f);
    }
    
    #endregion

    #region Private

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

    #endregion

    #region Public

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
        
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
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
            
            // clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set a new selected object
            EventSystem.current.SetSelectedGameObject(mainContinueFirstButton);
        }
        else
        {
            mainMenu.SetActive(true);
            
            // clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set a new selected object
            EventSystem.current.SetSelectedGameObject(mainStartFirstButton);
        }
    }

    /// <summary>
    /// This method is called to change the volume global parameters of the game.
    /// </summary>
    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
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
    /// This method is called to close the settings UI menu.
    /// </summary>
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        OpenMainMenu();
    }
    
    /// <summary>
    /// This method is called to resume the game.
    /// </summary>
    public void ContinueGame()
    {
        StopMainMenuTheme();
        GameManager.Instance.LoadLevelData();
    }

    #endregion
}