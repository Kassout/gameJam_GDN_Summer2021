using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class <c>PlayerController</c> is a Unity component script used to manage the different player behaviours.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>rigidBody</c> represents the player's rigidbody.
    /// </summary>
    public Rigidbody2D rigidBody;
    
    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the player's movement speed.
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5f;

    /// <summary>
    /// Instance variable <c>groundTileMap</c> represents the ground tile map on which player will walk on.
    /// </summary>
    [SerializeField]
    private Tilemap groundTileMap;

    /// <summary>
    /// Instance variable <c>collisionTileMap</c> represents the tile map containing the different obstacles the player could collide with.
    /// </summary>
    [SerializeField]
    private Tilemap collisionTileMap;

    /// <summary>
    /// Instance variable <c>interactionTileMap</c> represents the tile map containing the different object the player could interact with.
    /// </summary>
    [SerializeField]
    private Tilemap interactionTileMap;

    /// <summary>
    /// Instance variable <c>movement</c> represents the player's movement.
    /// </summary>
    private Vector2 _movement;

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        // Input
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate()
    {
        // Move
        MovePlayer();
    }
    
    /// <summary>
    /// This method is used to move the player.
    /// </summary>
    private void MovePlayer()
    {
        Vector2 direction = rigidBody.position + _movement * (moveSpeed * Time.fixedDeltaTime);
        if (CanMove(direction))
        {
            rigidBody.MovePosition(direction);
        }
    }

    /// <summary>
    /// This method is used to check if whether or not a player can move in the targeted direction considerate eventual obstacles.
    /// </summary>
    /// <param name="direction">A <c>Vector3</c> Unity structure representing the movement direction of the player.</param>
    /// <returns>A boolean value representing the state of movement allowance.</returns>
    private bool CanMove(Vector3 direction)
    {
        Vector3Int gridPosition = groundTileMap.WorldToCell(direction);
        if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }
    
    // TODO : comments
    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // TODO : do things on interact
        }
    }
}
