using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    /// TODO: comments
    /// </summary>
    private static int s_currentScene;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public static List<Command> OldCommands = new List<Command>();
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public static List<Vector2> OldDirections = new List<Vector2>();
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private GameObject playerGhost;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private Animator _gameManagerAnimator;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private AudioSource onTimeLoopOverAudioSource;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private AudioSource onRecallTimeLoopAudioSource;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private Image transitionScreen;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private static readonly int IsLoading = Animator.StringToHash("isLoading");

    /// <summary>
    /// TODO: comments
    /// </summary>
    private static readonly int IsLoadingOver = Animator.StringToHash("isLoadingOver");

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

        _gameManagerAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        // Load main menu at start.
        LoadScene(MAIN_MENU_SCENE, false);
        s_currentScene = MAIN_MENU_SCENE;
    }

    /// <summary>
    /// This method is used to load a scene.
    /// </summary>
    /// <param name="sceneIndex">An integer value representing the index of the scene to load.</param>
    /// <param name="isRecall">TODO: comments</param>
    public void LoadScene(int sceneIndex, bool isRecall)
    {
        StartCoroutine(OnLoadScene(sceneIndex, isRecall));
        s_currentScene = sceneIndex;
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

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="oldCommands"></param>
    /// <param name="oldDirections"></param>
    public void PlayerReplay(List<Command> oldCommands, List<Vector2> oldDirections)
    {
        OldCommands = new List<Command>(oldCommands);
        OldDirections = new List<Vector2>(oldDirections);
        onRecallTimeLoopAudioSource.Play();
        StartCoroutine(OnLoadScene(s_currentScene, true));
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private IEnumerator OnLoadScene(int sceneIndex, bool isRecall)
    {
        _gameManagerAnimator.SetTrigger(IsLoading);
        yield return new WaitForSeconds(1f);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        _gameManagerAnimator.SetTrigger(IsLoadingOver);
        yield return new WaitForSeconds(0.8f);

        if (isRecall)
        {
            Instantiate(playerGhost, PlayerController.StartingPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    public void OnTriggerLoadScene()
    {
        transitionScreen.gameObject.SetActive(true);
    }

    public void OnTriggerLoadingOverScene()
    {
        transitionScreen.gameObject.SetActive(false);
    }
}