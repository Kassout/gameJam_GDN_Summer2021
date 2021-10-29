using System.Collections;
using UnityEngine;

/// <summary>
/// Class <c>ArrowController</c> is a Unity component script used to manage the arrow game object behaviour.
/// </summary>
public class ArrowController : MonoBehaviour
{
    #region Fields/Variables

    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the speed of the arrow.
    /// </summary>
    [SerializeField] private float moveSpeed = 10.0f;
    
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
    /// <param name="oCollider">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D oCollider)
    {
        if (oCollider.CompareTag("Spring"))
        {
            transform.position = oCollider.transform.position;
            _direction = oCollider.GetComponent<SpringController>().GetDirection();
            Rotate();
            transform.Translate(_direction * 0.8f, Space.World);
        } else if (oCollider.CompareTag("InteractionObject"))
        {
            LeverController lever = oCollider.GetComponent<LeverController>();
            if(lever != null) {
                IEnumerator coroutine = lever.PushSequenceOnInteraction();
                lever.PassCoroutineRef(coroutine);
                lever.StartCoroutine(coroutine);
            }
        }
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="oCollision"></param>
    private void OnCollisionEnter2D(Collision2D oCollision)
    {
        if (oCollision.gameObject.CompareTag("Player"))
        {
            oCollision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        if(!oCollision.gameObject.CompareTag("Spring") || !oCollision.gameObject.CompareTag("InteractionObject")) {
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
