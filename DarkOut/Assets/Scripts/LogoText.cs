using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class <c>LogoText</c> is a Unity component script used to manage the hover button behaviour.
/// </summary>
public class LogoText : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>logoText</c> represents the text bubble to activate on mouse hover.
    /// </summary>
    public GameObject logoText;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        logoText.SetActive(false);
    }

    /// <summary>
    /// This method is called when the behaviour becomes disabled.
    /// </summary>
    private void OnDisable()
    {
        logoText.SetActive(false);
    }

    #endregion

    #region SelectHandler

    /// <summary>
    /// This method is called to evaluate current selected state and transition to appropriate state.
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public void OnSelect(BaseEventData eventData)
    {
        logoText.SetActive(true);
    }
    
    /// <summary>
    /// This method is called to evaluate current Deselected state and transition to appropriate state.
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public void OnDeselect(BaseEventData eventData)
    {
        logoText.SetActive(false);
    }

    #endregion
}
