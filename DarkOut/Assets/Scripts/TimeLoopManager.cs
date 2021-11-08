using System.Collections;
using UnityEngine;

/// <summary>
/// Class <c>TimeLoopManager</c> is a Unity component script used to manage the time loop mechanism of the game.
/// </summary>
public class TimeLoopManager : MonoBehaviour
{
    /// <summary>
    /// Static variable <c>Instance</c> represents the instance of the class. 
    /// </summary>
    public static TimeLoopManager Instance { get; private set; }
    
    /// <summary>
    /// Instance variable <c>timeLoopDuration</c> represents the number of seconds a time loop lasts.
    /// </summary>
    [SerializeField]   
    private float timeLoopDuration = 60.0f;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private Coroutine _coroutine;
    
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
        Time.timeScale = 1;
    }

    /// <summary>
    /// This method is used to start a time loop.
    /// </summary>
    public void StartTimeLoop()
    {
        if(_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(ProcessTimeLoop(timeLoopDuration));
    }

    /// <summary>
    /// This method is used to launch the time loop counter.
    /// </summary>
    /// <param name="countDownTime">A float value representing the duration of the time loop.</param>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    private IEnumerator ProcessTimeLoop(float countDownTime)
    {
        float totalTime = 0;
        while(totalTime <= countDownTime)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        
        _coroutine = null;
        
        GameManager.Instance.PlayerReplay();
    }
}