using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class <c>LogoText</c> is a Unity component script used to manage the hover button behaviour.
/// </summary>
public class LogoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    /// <summary>
    /// Instance variable <c>logoText</c> represents the text bubble to activate on mouse hover.
    /// </summary>
    public GameObject logoText;

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

    /// <summary>
    /// This method is called to evaluate current state and transition to appropriate state.
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        logoText.SetActive(true);
    }

    /// <summary>
    /// This method is called to evaluate current state and transition to normal state.
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        logoText.SetActive(false);
    }

    /// <summary>
    /// This method is called to registered IPointerClickHandler callback.
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        logoText.SetActive(false);
    }
}
