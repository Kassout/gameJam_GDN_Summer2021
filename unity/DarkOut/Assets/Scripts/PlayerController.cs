using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class <c>PlayerController</c> is a Unity component script used to manage the different player behaviours.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Static variable <c>Instance</c> representing the instance of the class. 
    /// </summary>
    public static PlayerController Instance { get; private set; }
    
    /// <summary>
    /// Instance variable <c>characterSprite</c> represents the player's character sprite.
    /// </summary>
    [SerializeField]
    private SpriteRenderer characterSprite;

    /// <summary>
    /// Instance variable <c>walkSprite</c> represents the player's character walking sprite.
    /// </summary>
    [SerializeField]
    private Sprite walkSprite;
    
    /// <summary>
    /// Instance variable <c>walkSprite</c> represents the player's character walking sprite.
    /// </summary>
    private Sprite _idleSprite;
    
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
    /// Instance variable <c>immobilizationTime</c> represents the player's time of immobilization when getting hurt.
    /// </summary>
    [SerializeField] 
    private float immobilizationTime = 0.3f;

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
    /// Instance variable <c>movement</c> represents the player's movement.
    /// </summary>
    private Vector2 _movement;

    private GameObject _currentInteractionObj;

    /// <summary>
    /// Static variable <c>TriggerDamage</c> represents the string message of the animator trigger variable "triggerDamage".
    /// </summary>
    private static readonly int TriggerDamage = Animator.StringToHash("triggerDamage");

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        _idleSprite = characterSprite.sprite;
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        // Input
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        characterSprite.flipX = _movement.x < 0;
        
        if (_movement.x != 0 || _movement.y != 0)
        {
            characterSprite.sprite = walkSprite;
        }
        else
        {
            characterSprite.sprite = _idleSprite;
        }
        
        // Interact
        Interact();
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
        Vector2 direction = rigidBody.position + _movement.normalized * (moveSpeed * Time.fixedDeltaTime);
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
    
    /// <summary>
    /// This method is used to interact with game object of <c>TriggeringObject</c> class.
    /// </summary>
    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentInteractionObj)
        {
            StartCoroutine(_currentInteractionObj.GetComponent<TriggeringObject>().PushSequence());
        }
    }

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("InteractionObject"))
        {
            Debug.Log(other.gameObject.name);
            _currentInteractionObj = other.gameObject;
        }
    }
    
    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("InteractionObject"))
        {
            Debug.Log(other.gameObject.name);
            _currentInteractionObj = null;
        }
    }

    /// <summary>
    /// This method is called when the player get in collision with a hurting game object.
    /// </summary>
    public void TakeDamage()
    {
        StartCoroutine(GetHurt());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// This method is called when the player get hurt.
    /// </summary>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    private IEnumerator GetHurt()
    {
        GetComponentInChildren<Animator>().SetTrigger(TriggerDamage);
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        yield return new WaitForSeconds(immobilizationTime);
        
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
