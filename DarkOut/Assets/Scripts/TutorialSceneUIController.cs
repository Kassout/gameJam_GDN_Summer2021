using System.Collections;
using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class TutorialSceneUIController : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField] private GameObject[] slideList;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private int _currentIndex = 0;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private bool _isLoading = false;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private bool _endTutorial = false;

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
        
        if (Input.anyKeyDown && !_isLoading && !_endTutorial)
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

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <returns>TODO: comments</returns>
    private IEnumerator ChangeSlide()
    {
        _isLoading = true;
        GameManager.Instance.gameManagerAnimator.SetTrigger("isLoading");
        
        yield return new WaitForSeconds(1f);
        
        slideList[_currentIndex].SetActive(false);
        _currentIndex++;
        slideList[_currentIndex].SetActive(true);
        
        GameManager.Instance.gameManagerAnimator.SetTrigger("isLoadingOver");
        
        yield return new WaitForSeconds(1f);
        _isLoading = false;
    }
}
