using System.Collections;
using UnityEngine;

public class TutorialSceneUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] slideList;

    private int _currentIndex = 0;
    private bool _isLoading = false;
    private bool _endTutorial = false;

    private void Awake()
    {
        slideList[_currentIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
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
