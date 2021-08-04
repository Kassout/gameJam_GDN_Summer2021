using UnityEngine;

/// <summary>
/// Class <c>RotationPlatformController</c> is a Unity component script used to manage the rotation platform trap behaviour.
/// </summary>
public class RotationPlatformController : ActionableObject
{
    /// <summary>
    /// Instance variable <c>connectedObject</c> represents the game object placed on the platform.
    /// </summary>
    private GameObject _connectedObject;

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            _connectedObject = other.gameObject;
            other.attachedRigidbody.velocity = Vector2.zero;
            other.attachedRigidbody.MovePosition(gameObject.GetComponent<Collider2D>().bounds.center);
        }
    }

    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        _connectedObject = null;
    }

    /// <summary>
    /// This method is used when the platform rotation get triggered.
    /// </summary>
    public override void TriggerActionEvent()
    {
        if (_connectedObject.transform.parent.CompareTag("SpringBox"))
        {
            _connectedObject.GetComponentInParent<SpringBoxController>().Rotate();
        }
    }

    /// <summary>
    /// This method is used when the rotation platform get deactivated.
    /// </summary>
    public override void KillTriggers()
    {
        // DO NOTHING
    }
}
