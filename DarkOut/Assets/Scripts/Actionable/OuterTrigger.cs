using UnityEngine;

/// <summary>
/// Class <c>OuterTrigger</c> is a Unity component script used to manage objects behaviour exiting rotation platform box collider.
/// </summary>
public class OuterTrigger : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>controller</c> represents the rotation platform controller instance.
    /// </summary>
    public RotationPlatformController controller;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    void OnTriggerExit2D(Collider2D other) {
        controller.OuterExited(other);
    }

    #endregion
}
