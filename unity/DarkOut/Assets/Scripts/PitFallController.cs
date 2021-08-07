using UnityEngine;

/// <summary>
/// Class <c>PitFallController</c> is a Unity component script used to manage the pit tiles behaviour.
/// </summary>
public class PitFallController : MonoBehaviour
{
    /// <summary>
    /// This method is called each frame where another object is within a trigger collider attached to this object
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.attachedRigidbody.velocity.magnitude == 0)
        {
            other.GetComponent<Animator>().SetTrigger("triggerFall");
        }
    }
}
