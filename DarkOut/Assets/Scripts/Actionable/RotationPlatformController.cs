using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>RotationPlatformController</c> is a Unity component script used to manage the rotation platform trap behaviour.
/// </summary>
public class RotationPlatformController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>rotationDirection</c> represents which direction boxes placed on the tile will rotate.
    /// </summary>
    [SerializeField]
    private RotationDirection rotationDirection;
    
    /// <summary>
    /// Instance variable <c>boxOnPlatformSound</c> represents the <c>AudioSource</c> Unity component triggering box being pushed on platform sound.
    /// </summary>
    [SerializeField]
    private AudioSource boxOnPlatformSound;
    
    /// <summary>
    /// Instance variable <c>RotationDirection</c> represents an enumeration of the two rotation directions.
    /// </summary>
    private enum RotationDirection
    {
        Clockwise,
        Counterclockwise,
    }

    /// <summary>
    /// Instance variable <c>colliderList</c> represents the game objects that are on the rotation tile.
    /// </summary>
    private readonly List<Collider2D> _colliderList = new List<Collider2D>();

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (rotationDirection) {
            case RotationDirection.Clockwise:
                spriteRenderer.flipX = false;
                break;
            case RotationDirection.Counterclockwise:
                spriteRenderer.flipX = true;
                break;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// This method is called when the script is loaded or a value is changed in the Inspector.
    /// </summary>
    void OnValidate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (rotationDirection)
        {
            case RotationDirection.Clockwise:
                spriteRenderer.flipX = false;
                break;
            case RotationDirection.Counterclockwise:
                spriteRenderer.flipX = true;
                break;
        }
    }
#endif
    
    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            /*_connectedObject = other.gameObject;
            other.attachedRigidbody.velocity = Vector2.zero;
            other.attachedRigidbody.MovePosition(gameObject.GetComponent<Collider2D>().bounds.center);*/
            GameObject connectedObject = other.gameObject;
            if(connectedObject.CompareTag("SpringBox"))
                _colliderList.Add(other);
        }
    }
    
    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        List<Collider2D> tempColliderList = new List<Collider2D>(_colliderList); // This is so that I can modify colliderList during foreach without error
        foreach (Collider2D oCollider in tempColliderList) {
            if(oCollider == other) {
                _colliderList.Remove(oCollider);
            }
        }
    }
    
    #endregion

    #region Public

    /// <summary>
    /// This method is responsible for handling actions triggered by the collider exiting the rotation platform.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the object exiting the rotation platform collider area.</param>
    public void OuterExited(Collider2D other) {
        GameObject connectedObject = other.gameObject;
        if(connectedObject.CompareTag("SpringBox")) {
            List<Collider2D> tempColliderList = new List<Collider2D>(_colliderList); // This is so that I can modify colliderList during foreach without error
            foreach (Collider2D oCollider in tempColliderList) {
                if(oCollider == other) {
                    if(rotationDirection == RotationDirection.Clockwise) {
                        connectedObject.GetComponent<SpringBoxController>().Rotate(true);
                        boxOnPlatformSound.Play();
                    } else {
                        connectedObject.GetComponent<SpringBoxController>().Rotate(false);
                        boxOnPlatformSound.Play();
                    }
                    
                }
            }
        }
    }

    #endregion
}
