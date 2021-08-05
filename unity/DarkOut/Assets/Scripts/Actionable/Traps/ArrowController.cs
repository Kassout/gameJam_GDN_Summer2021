using UnityEngine;

/// <summary>
/// Class <c>ArrowController</c> is a Unity component script used to manage the arrow game object behaviour.
/// </summary>
public class ArrowController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the speed of the arrow.
    /// </summary>
    [SerializeField]
    private float moveSpeed = 10.0f;

    private Vector2 direction;

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        MoveForward();
    }

    public void SetDirection(Vector2 dir) {
        direction = dir;
        Rotate();
    }

    /// <summary>
    /// This method is called to move the arrow.
    /// </summary>
    private void MoveForward()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spring"))
        {
            Debug.Log("Change Direction");
            direction = other.GetComponent<SpringController>().GetDirection();
            Rotate();
        } else if (other.CompareTag("InteractionObject"))
        {
            LeverController lever = other.GetComponent<LeverController>();
            if(lever != null) {
                StartCoroutine(lever.PushSequenceOnInteraction());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Kill player.");
            other.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        if(!other.gameObject.CompareTag("Spring") || !other.gameObject.CompareTag("InteractionObject")) {
            Destroy(gameObject);
            Debug.Log(other.gameObject);
        }
    }
    
    private void Rotate() {
        if(direction == Vector2.up) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (direction == Vector2.down) {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        } else if (direction == Vector2.left) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
