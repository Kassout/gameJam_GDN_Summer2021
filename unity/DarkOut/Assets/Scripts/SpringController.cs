using System.Collections;
using UnityEngine;

/// <summary>
/// Class <c>SpringController</c> is a Unity component script used to manage the spring objects behaviour.
/// </summary>
public class SpringController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>direction</c> represents the bouncing direction vector of the spring.
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// Instance variable <c>forceAmplitude</c> represents the bouncing force amplitude value of the spring.
    /// </summary>
    [SerializeField]
    private float forceAmplitude;
    
    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    [SerializeField]
    private BouncingDirection bouncingDirection;

    /// <summary>
    /// Instance variable <c>timeToWait</c> represents the bouncing time of the objects which collides with the spring.
    /// </summary>
    [SerializeField]
    private float timeToWait = 0.5f;

    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    private enum BouncingDirection
    {
        Up,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        switch (bouncingDirection)
        {
            case BouncingDirection.Up:
                direction = Vector2.up;
                break;
            case BouncingDirection.Bottom:
                direction = Vector2.down;
                break;
            case BouncingDirection.Left:
                direction = Vector2.left;
                break;
            case BouncingDirection.Right:
                direction = Vector2.right;
                break;
        }
    }

    /// <summary>
    /// This method is called when an incoming collider makes contact with this object's collider
    /// </summary>
    /// <param name="other">A <c>Collision2D</c> Unity component representing the collision of the object that it collided with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        StartCoroutine(GetBounced(other.gameObject));
    }

    /// <summary>
    /// This method is used to bounce and trigger action on the collided object.
    /// </summary>
    /// <param name="gameObject">A Unity <c>GameObject</c> object that collided with the spring.</param>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    private IEnumerator GetBounced(GameObject gameObject)
    {
        gameObject.GetComponent<PlayerController>().StartSpring();
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * forceAmplitude, ForceMode2D.Impulse);

        yield return new WaitForSeconds(timeToWait);
        
        gameObject.GetComponent<PlayerController>().StopSpring();
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}