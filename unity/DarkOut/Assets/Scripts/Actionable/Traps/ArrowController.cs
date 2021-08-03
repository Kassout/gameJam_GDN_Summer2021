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

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        MoveForward();
    }

    /// <summary>
    /// This method is called to move the arrow.
    /// </summary>
    private void MoveForward()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kill player.");
            PlayerController.Instance.TakeDamage();
        }
        Destroy(gameObject);
    }
}
