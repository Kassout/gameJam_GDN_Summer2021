using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>GameManager</c> is a Unity component script used to manage general gameplay features of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static variable <c>Instance</c> represents the instance of the class. 
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Static variable <c>MAIN_MENU_SCENE</c> represents the scene index of the main menu scene.
    /// </summary>
    private static int MAIN_MENU_SCENE = 1;
    
    /// <summary>
    /// Static variable <c>PRELOAD_SCENE</c> represents the scene index of the preload scene.
    /// </summary>
    private static int PRELOAD_SCENE = 0;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        // Load main menu at start.
        LoadScene(MAIN_MENU_SCENE);
    }

    /// <summary>
    /// This method is used to load a scene.
    /// </summary>
    /// <param name="sceneIndex">An integer value representing the index of the scene to load.</param>
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        if (!sceneIndex.Equals(MAIN_MENU_SCENE) && !sceneIndex.Equals(PRELOAD_SCENE))
        {
            SaveLevelData();
            SceneManager.sceneLoaded += OnSceneLoaded;   
        }
    }

    /// <summary>
    /// This method is used to perform some action when a scene achieved from loading.
    /// </summary>
    /// <param name="scene">A Unity <c>Scene</c> structure representing the loaded scene.</param>
    /// <param name="mode">A Unity <c>LoadSceneMode</c> enumeration representing the type of loading mode used for the loaded scene.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        TimeLoopManager.Instance.StartTimeLoop();
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    /// <summary>
    /// This method is used to save the level data before time-looping or game exit.
    /// </summary>
    private void SaveLevelData()
    {
        // TODO : save player progress before loading next scene
    }
}