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
    private int _currentScene;
    
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
    public Animator gameManagerAnimator;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private AudioSource mainThemeAudioSource;
    
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
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private int startingScene;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private Tilemap _groundTileMap;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Tilemap _collisionTileMap;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Tilemap _pitfallTileMap;

    /// <summary>
    /// TODO: comments
    /// </summary>
    public bool blockPlayer;

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

    /// <summary>
    /// This method is used to load a scene.
    /// </summary>
    /// <param name="sceneIndex">An integer value representing the index of the scene to load.</param>
    /// <param name="isRecall">TODO: comments</param>
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
    /// TODO: comments
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
    /// TODO: comments
    /// </summary>
    public void GameEnd()
    {
        LoadScene(MAIN_MENU_SCENE, false);
    }

    /// <summary>
    /// This method is used to perform some action when a scene achieved from loading.
    /// </summary>
    /// <param name="scene">A Unity <c>Scene</c> structure representing the loaded scene.</param>
    /// <param name="mode">A Unity <c>LoadSceneMode</c> enumeration representing the type of loading mode used for the loaded scene.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!mainThemeAudioSource.isPlaying)
        {
            mainThemeAudioSource.Play();
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
    /// TODO: comments
    /// </summary>
    public void LoadLevelData()
    {
        PersistantData data = LoadPersistantData();
        LoadScene(data.level, false);
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="oldCommands"></param>
    /// <param name="oldDirections"></param>
    public void PlayerReplay()
    {
        OldCommands = new List<Command>(InputHandler.oldCommands);
        OldDirections = new List<Vector2>(InputHandler.oldDirections);
        onRecallTimeLoopAudioSource.Play();
        StartCoroutine(OnLoadScene(_currentScene, true));
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="sceneIndex">TODO: comments</param>
    /// <param name="isRecall">TODO: comments</param>
    /// <returns>TODO: comments</returns>
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
        
        _groundTileMap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();
        _collisionTileMap = GameObject.Find("Collision Tilemap").GetComponent<Tilemap>();
        _pitfallTileMap = GameObject.Find("Pit Tilemap").GetComponent<Tilemap>();

        if (isRecall)
        {
            Instantiate(playerGhost, PlayerController.startingPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <returns>TODO: comments</returns>
    public Tilemap GetGround() {
        return _groundTileMap;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <returns>TODO: comments</returns>
    public Tilemap GetCollision() {
        return _collisionTileMap;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <returns>TODO: comments</returns>
    public Tilemap GetPitfall() {
        return _pitfallTileMap;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    public void OnTriggerLoadScene()
    {
        transitionScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// TODO: comments
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

    /// <summary>
    /// This method is used to save the different attributes class values in a json file.
    /// </summary>
    private void SavePersistantData()
    {
        PersistantData data = new PersistantData();
        data.level = _currentScene;
        
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
}