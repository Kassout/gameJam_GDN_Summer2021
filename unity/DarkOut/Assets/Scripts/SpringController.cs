using UnityEngine;

/// <summary>
/// Class <c>SpringController</c> is a Unity component script used to manage the spring objects behaviour.
/// </summary>
public class SpringController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>direction</c> represents the bouncing direction vector of the spring.
    /// </summary>
    private Vector2 _direction;

    /// <summary>
    /// Instance variable <c>forceAmplitude</c> represents the bouncing force amplitude value of the spring.
    /// </summary>
    [SerializeField]
    private float forceAmplitude;
    
    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    public BouncingDirection bouncingDirection;

    /// <summary>
    /// Instance variable <c>timeToWait</c> represents the bouncing time of the objects which collides with the spring.
    /// </summary>
    [SerializeField]
    private float timeToWait = 0.5f;

    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    public enum BouncingDirection
    {
        Up,
        Bottom,
        Left,
        Right
    }
    
    /// <summary>
    /// Instance variable <c>springTriggerSound</c> represents the <c>AudioSource</c> Unity component triggering spring bounce sound.
    /// </summary>
    [SerializeField]
    private AudioSource springTriggerSound;
    

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        switch (bouncingDirection)
        {
            case BouncingDirection.Up:
                _direction = Vector2.up;
                break;
            case BouncingDirection.Bottom:
                _direction = Vector2.down;
                break;
            case BouncingDirection.Left:
                _direction = Vector2.left;
                break;
            case BouncingDirection.Right:
                _direction = Vector2.right;
                break;
        }
    }

    /// <summary>
    /// This method is called when an incoming collider makes contact with this object's collider
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collision of the object that it collided with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TriggerBounce(_direction * forceAmplitude, timeToWait);
        } else if (other.CompareTag("Ghost"))
        {
            other.GetComponent<GhostController>().TriggerBounce(_direction * forceAmplitude, timeToWait);
        }
        else if (other.CompareTag("Arrow"))
        {

        }
        else if (other.attachedRigidbody)
        {
            BoxController box = other.GetComponent<BoxController>();
            if(box != null) {
                box.SpringBounce(_direction);
            }
            //other.GetComponent<Rigidbody2D>().AddForce(_direction * forceAmplitude * 10, ForceMode2D.Impulse);
        }
        springTriggerSound.Play();
    }

    /// <summary>
    /// This method is called to get the spring bouncing direction.
    /// </summary>
    /// <returns>A <c>Vector2</c> Unity component representing the bouncing direction vector.</returns>
    public Vector2 GetDirection() {
        return _direction;
    }

}