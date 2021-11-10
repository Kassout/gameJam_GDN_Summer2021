using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

/// <summary>
/// Class <c>GameManager</c> is a Unity component script used to manage general gameplay features of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Static variable <c>Instance</c> represents the instance of the class. 
    /// </summary>
    public static GameManager Instance { get; private set; }
    
    /// <summary>
    /// Static variable <c>oldCommands</c> represents the list of old commands invoked by the player.
    /// </summary>
    public static List<Command> oldCommands = new List<Command>();
    
    /// <summary>
    /// Static variable <c>oldDirections</c> represents the list of old directions related to the old commands invoked by the player.
    /// </summary>
    public static List<Vector2> oldDirections = new List<Vector2>();

    /// <summary>
    /// Instance variable <c>gameManagerAnimator</c> represents the game manager animator Unity component.
    /// </summary>
    public Animator gameManagerAnimator;
    
    /// <summary>
    /// Instance variable <c>blockPlayer</c> represents the current blocking status of the player.
    /// </summary>
    public bool blockPlayer;
    
    /// <summary>
    /// Instance variable <c>startingScene</c> represents the index of the starting played scene.
    /// </summary>
    [SerializeField]
    private int startingScene;
        
    /// <summary>
    /// Instance variable <c>playerGhost</c> represents the player ghost game object.
    /// </summary>
    [SerializeField]
    private GameObject playerGhost;
    
    /// <summary>
    /// Instance variable <c>mainTheme</c> represents the main theme game audio source.
    /// </summary>
    [SerializeField]
    private AudioSource mainTheme;
    
    /// <summary>
    /// Instance variable <c>onRecallTimeLoop</c> represents the on recall time loop game audio source.
    /// </summary>
    [SerializeField]
    private AudioSource onRecallTimeLoop;

    /// <summary>
    /// Instance variable <c>transitionScreen</c> represents the transition screen image.
    /// </summary>
    [SerializeField]
    private Image transitionScreen;

    /// <summary>
    /// Static variable <c>MAIN_MENU_SCENE</c> represents the scene index of the main menu scene.
    /// </summary>
    private static int MAIN_MENU_SCENE = 1;
    
    /// <summary>
    /// Static variable <c>PRELOAD_SCENE</c> represents the scene index of the preload scene.
    /// </summary>
    private static int PRELOAD_SCENE = 0;
    
    /// <summary>
    /// Static variable <c>IsLoading</c> represents the string message to send to the game object animator to change the state of the "isLoading" variable.
    /// </summary>
    private static readonly int IsLoading = Animator.StringToHash("isLoading");

    /// <summary>
    /// Static variable <c>IsLoadingOver</c> represents the string message to send to the game object animator to change the state of the "isLoadingOver" variable.
    /// </summary>
    private static readonly int IsLoadingOver = Animator.StringToHash("isLoadingOver");
    
    /// <summary>
    /// Instance variable <c>_currentScene</c> represents the index of the current playing scene.
    /// </summary>
    private int _currentScene;

    /// <summary>
    /// Instance variable <c>_groundTileMap</c> represents the ground tile map on which player will stay on.
    /// </summary>
    private Tilemap _groundTileMap;
    
    /// <summary>
    /// Instance variable <c>_collisionTileMap</c> represents the tile map containing the different obstacles the player could collide with.
    /// </summary>
    private Tilemap _collisionTileMap;
    
    /// <summary>
    /// Instance variable <c>_pitfallTileMap</c> represents the tile map containing the different pit tiles the player could fall into.
    /// </summary>
    private Tilemap _pitfallTileMap;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
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

        gameManagerAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        // Load main menu at start.
        LoadScene(startingScene, false);
        _currentScene = startingScene;
    }

    #endregion

    #region Private

    /// <summary>
    /// This method is used to perform some action when a scene achieved from loading.
    /// </summary>
    /// <param name="scene">A Unity <c>Scene</c> structure representing the loaded scene.</param>
    /// <param name="mode">A Unity <c>LoadSceneMode</c> enumeration representing the type of loading mode used for the loaded scene.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!mainTheme.isPlaying)
        {
            mainTheme.Play();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// This method is used to save the level data before time-looping or game exit.
    /// </summary>
    private void SaveLevelData()
    {
        SavePersistantData();
    }
    
    /// <summary>
    /// This method is responsible for managing the different loading scene step actions.
    /// </summary>
    /// <param name="sceneIndex">An integer value representing the scene index to load.</param>
    /// <param name="isRecall">A boolean value representing the recall status of the player.</param>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    private IEnumerator OnLoadScene(int sceneIndex, bool isRecall)
    {
        gameManagerAnimator.SetTrigger(IsLoading);
        blockPlayer = true;
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
        
        gameManagerAnimator.SetTrigger(IsLoadingOver);
        yield return new WaitForSeconds(0.8f);
        
        blockPlayer = false;
        TimeLoopManager.Instance.StartTimeLoop();

        if (GameObject.Find("Ground Tilemap"))
        {
            _groundTileMap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
        }
        if (GameObject.Find("Collision Tilemap"))
        {
            _collisionTileMap = GameObject.Find("Collision Tilemap").GetComponent<Tilemap>();
        }
        if (GameObject.Find("Pit Tilemap"))
        {
            _pitfallTileMap = GameObject.Find("Pit Tilemap").GetComponent<Tilemap>();
        }
        
        if (isRecall)
        {
            Instantiate(playerGhost, PlayerController.startingPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// This method is used to save the different attributes class values in a json file.
    /// </summary>
    private void SavePersistantData()
    {
        PersistantData data = new PersistantData
        {
            level = _currentScene
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    /// <summary>
    /// This method is used to load the different attributes class values from a json file.
    /// </summary>
    private PersistantData LoadPersistantData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        PersistantData data = new PersistantData();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<PersistantData>(json);
        }

        return data;
    }
    
    #endregion

    #region Public

        /// <summary>
    /// This method is used to load a scene.
    /// </summary>
    /// <param name="sceneIndex">An integer value representing the index of the scene to load.</param>
    /// <param name="isRecall">A boolean value representing the recall status of the player.</param>
    public void LoadScene(int sceneIndex, bool isRecall)
    {
        StartCoroutine(OnLoadScene(sceneIndex, isRecall));
        _currentScene = sceneIndex;
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        if (!sceneIndex.Equals(MAIN_MENU_SCENE) && !sceneIndex.Equals(PRELOAD_SCENE))
        {
            SaveLevelData();
            SceneManager.sceneLoaded += OnSceneLoaded;   
        }
    }

    /// <summary>
    /// This method is called to load the next indexed level scene.
    /// </summary>
    public void LoadNextLevel()
    {
        int nextLevelSceneIndex = _currentScene + 1;
        if (nextLevelSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadScene(nextLevelSceneIndex, false);
        }
        else
        {
            GameEnd();
        }
    }

    /// <summary>
    /// This method is responsible for managing the end game behaviour.
    /// </summary>
    public void GameEnd()
    {
        LoadScene(MAIN_MENU_SCENE, false);
    }

    /// <summary>
    /// This method is responsible for loading the player saved level data.
    /// </summary>
    public void LoadLevelData()
    {
        PersistantData data = LoadPersistantData();
        LoadScene(data.level, false);
    }

    /// <summary>
    /// This method is responsible for playing the player replay command behaviour.
    /// </summary>
    public void PlayerReplay()
    {
        oldCommands = new List<Command>(InputHandler.oldCommands);
        oldDirections = new List<Vector2>(InputHandler.oldDirections);
        onRecallTimeLoop.Play();
        StartCoroutine(OnLoadScene(_currentScene, true));
    }

    /// <summary>
    /// This method is responsible for getting the ground tile map.
    /// </summary>
    /// <returns>A <c>Tilemap</c> Unity component representing the current level ground tile map.</returns>
    public Tilemap GetGround() {
        return _groundTileMap;
    }

    /// <summary>
    /// This method is responsible for getting the collision tile map.
    /// </summary>
    /// <returns>A <c>Tilemap</c> Unity component representing the current level collision tile map.</returns>
    public Tilemap GetCollision() {
        return _collisionTileMap;
    }

    /// <summary>
    /// This method is responsible for getting the pitfall tile map.
    /// </summary>
    /// <returns>A <c>Tilemap</c> Unity component representing the current level pitfall tile map.</returns>
    public Tilemap GetPitfall() {
        return _pitfallTileMap;
    }

    /// <summary>
    /// This method is called on load scene event trigger.
    /// </summary>
    public void OnTriggerLoadScene()
    {
        transitionScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method is called on loading over scene event trigger.
    /// </summary>
    public void OnTriggerLoadingOverScene()
    {
        transitionScreen.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Class <c>PersistantData</c> is used to manager persistant game data and player progress save.
    /// </summary>
    [Serializable]
    public class PersistantData
    {
        /// <summary>
        /// Static variable <c>Level</c> representing the level 
        /// </summary>
        public int level = 1;
    }

    #endregion
}