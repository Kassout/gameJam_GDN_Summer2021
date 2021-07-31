using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeLoopManager : MonoBehaviour
{
    public Text countDownText;
    
    public static TimeLoopManager Instance { get; private set; }
    
    [SerializeField]   
    private float timeLoopDuration = 60.0f;
    
    private void Awake()
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
        Time.timeScale = 1;
    }

    public void StartTimeLoop()
    {
        StartCoroutine(ProcessTimeLoop(timeLoopDuration));
    }

    private IEnumerator ProcessTimeLoop(float countDownTime)
    {
        countDownText = FindObjectOfType<Text>();
        float totalTime = 0;
        while(totalTime <= countDownTime)
        {
            totalTime += Time.deltaTime;
            var integer = (int)totalTime; /* choose how to quantize this */
            /* convert integer to string and assign to text */
            countDownText.text = integer.ToString();
            yield return null;
        }
        
        /*//Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSecondsRealtime(countDownTime);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);*/
        
        GameManager.Instance.LoadScene(2);
    }
}
