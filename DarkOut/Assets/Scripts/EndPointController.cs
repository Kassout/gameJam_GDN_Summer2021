using UnityEngine;

/// <summary>
/// Class <c>EndPointController</c> is a Unity component script used to manage the end level point behaviour.
/// </summary>
public class EndPointController : MonoBehaviour
{
    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
