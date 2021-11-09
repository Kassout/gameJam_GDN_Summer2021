using System.Collections;
using UnityEngine;

/// <summary>
/// Class <c>ArrowController</c> is a Unity component script used to manage the arrow game object behaviour.
/// </summary>
public class ArrowController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the speed of the arrow.
    /// </summary>
    [SerializeField] 
    private float moveSpeed = 10.0f;
    
    /// <summary>
    /// Instance variable <c>_direction</c> represents the direction vector of the arrow game object.
    /// </summary>
    private Vector2 _direction;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        MoveForward();
    }
    
    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spring"))
        {
            transform.position = other.transform.position;
            _direction = other.GetComponent<SpringController>().GetDirection();
            Rotate();
            transform.Translate(_direction * 0.8f, Space.World);
        } else if (other.CompareTag("InteractionObject"))
        {
            LeverController lever = other.GetComponent<LeverController>();
            if (lever != null) {
                IEnumerator coroutine = lever.PushSequenceOnInteraction();
                lever.PassCoroutineRef(coroutine);
                lever.StartCoroutine(coroutine);
            }
        }
    }

    /// <summary>
    /// This method is called when an incoming collider makes contact with this object's collider.
    /// </summary>
    /// <param name="other">A <c>Collision2D</c> Unity component data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        if(!other.gameObject.CompareTag("Spring") || !other.gameObject.CompareTag("InteractionObject")) {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private

    /// <summary>
    /// This method is called to move the arrow.
    /// </summary>
    private void MoveForward()
    {
        transform.Translate(_direction * (moveSpeed * Time.deltaTime), Space.World);
    }

    /// <summary>
    /// This method is responsible for rotating the arrow according to the arrow object direction vector value.
    /// </summary>
    private void Rotate() {
        if(_direction == Vector2.up) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (_direction == Vector2.down) {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        } else if (_direction == Vector2.left) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is responsible for changing the arrow object direction vector value.
    /// </summary>
    /// <param name="dir">A <c>Vector2</c> Unity component representing the direction to setup the arrow.</param>
    public void SetDirection(Vector2 dir) {
        _direction = dir;
        Rotate();
    }

    #endregion
}
