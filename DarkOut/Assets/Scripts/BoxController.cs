using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// TODO: comments
/// </summary>
public class BoxController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>groundTileMap</c> represents the ground tile map on which box will stay on.
    /// </summary>
    [SerializeField]
    private Tilemap groundTileMap;

    /// <summary>
    /// Instance variable <c>collisionTileMap</c> represents the tile map containing the different obstacles the box could collide with.
    /// </summary>
    [SerializeField]
    private Tilemap collisionTileMap;
    
    /// <summary>
    /// Instance variable <c>pitfallTileMap</c> represents the tile map containing the different pit tiles the box could fall into.
    /// </summary>
    [SerializeField]
    private Tilemap pitfallTileMap;

    /// <summary>
    /// Instance variable <c>rigidBody</c> represents the box's rigidbody.
    /// </summary>
    private Rigidbody2D _rigidbody;
    
    /// <summary>
    /// Instance variable <c>boxPushedSound</c> represents the <c>AudioSource</c> Unity component triggering box being pushed sound.
    /// </summary>
    [SerializeField]
    private AudioSource boxPushedSound;

    /// <summary>
    /// Instance variable <c>_startingPosition</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    private Vector2 _startPosition;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private bool _bouncing;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private float _bounceDistance;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private float bounceSpeed = 10.0f;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Vector2 _bounceDirection;

    /// <summary>
    /// This method is called on the frame when a script is enabled.
    /// </summary>
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _startPosition = _rigidbody.position;
        _bouncing = false;
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    void FixedUpdate()
    {
        CheckMoveCollision();
        if(_bouncing) {
            ContinueSpring();
        }
        if (_rigidbody.velocity.magnitude == 0)
        {
            boxPushedSound.Pause();
        }
    }

    /// <summary>
    /// This method is called each frame where a collider on another object is touching this object's collider.
    /// </summary>
    /// <param name="other">A <c>Collision2D</c> Unity component data associated with this collision.</param>
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _rigidbody.velocity.magnitude != 0 && !boxPushedSound.isPlaying)
        {
            boxPushedSound.Play();
        }
    }

    /// <summary>
    /// This method is called when a collider on another object stops touching this object's collider.
    /// </summary>
    /// <param name="other">A <c>Collision2D</c> Unity component data associated with this collision.</param>
    private void OnCollisionExit2D(Collision2D other)
    {
        boxPushedSound.Pause();
    }

    /// <summary>
    /// This method is used to check for potential collision with walls and pit tile.
    /// </summary>
    private void CheckMoveCollision() {
        if (!_bouncing) {
            Vector3Int gridPosition = groundTileMap.WorldToCell(_rigidbody.position);
            if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition) || pitfallTileMap.HasTile(gridPosition))
            {
                Kill();
            }
        }
    }

    /// <summary>
    /// This method is used to destroy the box game object.
    /// </summary>
    private void Kill() {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.position = _startPosition;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="direction">TODO: comments</param>
    public void SpringBounce(Vector2 direction) {
        _bouncing = true;
        _bounceDistance = 0.0f;
        _bounceDirection = direction;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    public void ContinueSpring() {
        if (_bounceDistance < 5.0f) {
            Vector2 move = _rigidbody.position + _bounceDirection * bounceSpeed * Time.deltaTime;
            _bounceDistance += bounceSpeed * Time.deltaTime;
            _rigidbody.MovePosition(move);
        } else {
            _bouncing = false;
        }
    }
}
