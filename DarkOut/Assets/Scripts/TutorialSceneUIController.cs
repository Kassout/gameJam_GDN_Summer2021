using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Class <c>TutorialSceneUIController</c> is a Unity component script used to manage the game tutorial behaviour.
/// </summary>
public class TutorialSceneUIController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>slideList</c> represents the list of tutorial slide game object to display on screen.
    /// </summary>
    [SerializeField] 
    private GameObject[] slideList;

    /// <summary>
    /// Instance variable <c>_currentIndex</c> represents the current tutorial slide index displayed on screen.
    /// </summary>
    private int _currentIndex;
    
    /// <summary>
    /// Instance variable <c>_isLoading</c> represents the current screen loading status.
    /// </summary>
    private bool _isLoading;
    
    /// <summary>
    /// Instance variable <c>_endTutorial</c> represents the finished tutorial status.
    /// </summary>
    private bool _endTutorial;

    /// <summary>
    /// Static variable <c>IsLoading</c> represents the string message to send to the game object animator to change the state of the "isLoading" variable.
    /// </summary>
    private static readonly int IsLoading = Animator.StringToHash("isLoading");
    
    /// <summary>
    /// Static variable <c>IsLoadingOver</c> represents the string message to send to the game object animator to change the state of the "isLoadingOver" variable.
    /// </summary>
    private static readonly int IsLoadingOver = Animator.StringToHash("isLoadingOver");

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        slideList[_currentIndex].SetActive(true);
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        if (_currentIndex == slideList.Length)
        {
            _endTutorial = true;
        }
        
        if ((Keyboard.current.anyKey.wasPressedThisFrame || Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic)) && !_isLoading && !_endTutorial)
        {
            StartCoroutine(ChangeSlide());
        }
        
        if (_endTutorial)
        {
            _endTutorial = false;
            GameManager.Instance.LoadNextLevel();
            this.enabled = false;
        }
    }

    #endregion

    #region Private

    /// <summary>
    /// This method is responsible for unloading the current slide and load the next one.
    /// </summary>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    private IEnumerator ChangeSlide()
    {
        _isLoading = true;
        GameManager.Instance.gameManagerAnimator.SetTrigger(IsLoading);
        
        yield return new WaitForSeconds(1f);
        
        slideList[_currentIndex].SetActive(false);
        _currentIndex++;
        slideList[_currentIndex].SetActive(true);
        
        GameManager.Instance.gameManagerAnimator.SetTrigger(IsLoadingOver);
        
        yield return new WaitForSeconds(1f);
        _isLoading = false;
    }

    #endregion
}
