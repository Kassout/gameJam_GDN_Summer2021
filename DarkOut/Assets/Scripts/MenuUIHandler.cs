using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>pauseMenu</c> represents the pause UI menu of the main game scene.
    /// </summary>
    [SerializeField]
    private GameObject pauseMenu;

    /// <summary>
    /// Instance variable <c>settingsMenu</c> represents the settings UI menu of the main game scene.
    /// </summary>
    [SerializeField]
    private GameObject settingsMenu;

    /// <summary>
    /// Instance variable <c>pauseFirstButton</c> is a Unity <c>GameObject</c> object representing the button to first select on pause menu set active.
    /// </summary>
    [SerializeField] private GameObject pauseFirstButton;
    
    /// <summary>
    /// Instance variable <c>settingsFirstButton</c> is a Unity <c>GameObject</c> object representing the button to first select on settings menu set active.
    /// </summary>
    [SerializeField] private GameObject settingsFirstButton;

    /// <summary>
    /// Instance variable <c>volumeSlider</c> represents a <c>Slider</c> Unity UI component used to change the global volume of the game.
    /// </summary>
    [SerializeField]
    private Slider volumeSlider;
    
    /// <summary>
    /// Instance variable <c>isPaused</c> represents the state of the game pause.
    /// </summary>
    private bool _isPaused;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1.0f);
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        if (InputHandler.pauseInput)
        {
            PauseUnpause();
        }
        Time.timeScale = 1 * (_isPaused ? 0 : 1);
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible for displaying or exiting the pause menu.
    /// </summary>
    private void PauseUnpause()
    {
        if (!settingsMenu.activeSelf)
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                OpenPauseMenu();
            }
            else
            {
                ResumeGame();
            }
        }
        else
        {
            CloseSettings();
        }
    }

    /// <summary>
    /// This method is called to open the pause UI menu.
    /// </summary>
    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is used to stop and close the game.
    /// </summary>
    public void QuitGame()
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
    /// This method is called to open the settings UI menu.
    /// </summary>
    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    /// <summary>
    /// This method is called to resume the game.
    /// </summary>
    public void ResumeGame()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        _isPaused = false;
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
    /// This method is called to close the settings UI menu.
    /// </summary>
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        
        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    #endregion

}
