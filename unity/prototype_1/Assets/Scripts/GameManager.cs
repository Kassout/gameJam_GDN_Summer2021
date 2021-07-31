using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int MAIN_MENU_SCENE = 1;
    private static int PRELOAD_SCENE = 0;

    private bool isPaused = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    private void Start()
    {
        // Load main menu at start.
        LoadScene(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = 1 * (isPaused ? 0 : 1);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        if (!sceneIndex.Equals(MAIN_MENU_SCENE) && !sceneIndex.Equals(PRELOAD_SCENE))
        {
            SaveLevelData();
            SceneManager.sceneLoaded += OnSceneLoaded;   
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        TimeLoopManager.Instance.StartTimeLoop();
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    private void SaveLevelData()
    {
        // TODO : save player progress before loading next scene
    }
}
