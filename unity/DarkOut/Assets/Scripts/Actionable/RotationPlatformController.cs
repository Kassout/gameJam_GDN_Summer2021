using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>RotationPlatformController</c> is a Unity component script used to manage the rotation platform trap behaviour.
/// </summary>
public class RotationPlatformController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>rotationDirection</c> represents which direction boxes placed on the tile will rotate.
    /// </summary>
    [SerializeField]
    protected RotationDirection rotationDirection;
    
    /// <summary>
    /// Instance variable <c>boxOnPlatformSound</c> represents the <c>AudioSource</c> Unity component triggering box being pushed on platform sound.
    /// </summary>
    [SerializeField]
    private AudioSource boxOnPlatformSound;
    
    /// <summary>
    /// Instance variable <c>RotationDirection</c> represents an enumeration of the two rotation directions.
    /// </summary>
    protected enum RotationDirection
    {
        Clockwise,
        Counterclockwise,
    }

    /// <summary>
    /// Instance variable <c>colliderList</c> represents the game objects that are on the rotation tile.
    /// </summary>
    private List<Collider2D> colliderList = new List<Collider2D>();

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
                colliderList.Add(other);
        }
    }

    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        List<Collider2D> tempColliderList = new List<Collider2D>(colliderList); // This is so that I can modify colliderList during foreach without error
        foreach (Collider2D collider in tempColliderList) {
            Debug.Log(collider.gameObject);
            if(collider == other) {
                colliderList.Remove(collider);
            }
        }
    }

    public void OuterExited(Collider2D other) {
        GameObject connectedObject = other.gameObject;
        if(connectedObject.CompareTag("SpringBox")) {
            List<Collider2D> tempColliderList = new List<Collider2D>(colliderList); // This is so that I can modify colliderList during foreach without error
            foreach (Collider2D collider in tempColliderList) {
                if(collider == other) {
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
}
