using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class <c>PlayerController</c> is a Unity component script used to manage the different player behaviours.
/// </summary>
public class PlayerController : MonoBehaviour
{
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
    private Rigidbody2D _rigidBody;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private Animator _animator;
    
    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the player's movement speed.
    /// </summary>
    [SerializeField]
    private float moveSpeed = 5f;

    /// <summary>
    /// Instance variable <c>isDisabled</c> represents the state of player control inputs.
    /// </summary>
    private bool _isDisabled = false;

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

    /// <summary>
    /// Instance variable <c>currentInteractionObj</c> represents the triggering game object the player is in the range of activation.
    /// </summary>
    private GameObject _currentInteractionObj;

    /// <summary>
    /// Static variable <c>TriggerDamage</c> represents the string message of the animator trigger variable "triggerDamage".
    /// </summary>
    private static readonly int TriggerDamage = Animator.StringToHash("triggerDamage");

    /// <summary>
    /// TODO : comments
    /// </summary>
    Vector2 move;
    
    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _idleSprite = characterSprite.sprite;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        // Input
        if (!_isDisabled)
        {
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
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate()
    {
        // Move
        if (!_isDisabled)
        {
            MovePlayer();
        }

    }

    public Vector2 GetMovement() {
        return move;
    }
    
    /// <summary>
    /// This method is used to move the player.
    /// </summary>
    private void MovePlayer()
    {
        move = _rigidBody.position + _movement.normalized * (moveSpeed * Time.fixedDeltaTime);
        if (CanMove(move))
        {
            _rigidBody.MovePosition(move);
        }
    }

    /// <summary>
    /// This method is used to disable player control inputs when being pushed by a spring.
    /// </summary>
    public void StartSpring()
    {
        _isDisabled = true;
    }

    /// <summary>
    /// This method is used to enable player control inputs after being pushed by a spring.
    /// </summary>
    public void StopSpring()
    {
        _isDisabled = false;
        _rigidBody.velocity = Vector2.zero;
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
        if (Input.GetKeyDown(KeyCode.E) && _currentInteractionObj && _currentInteractionObj.GetComponent<TriggeringObject>())
        {
            StartCoroutine(_currentInteractionObj.GetComponent<TriggeringObject>().PushSequenceOnInteraction());
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
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        yield return new WaitForSeconds(immobilizationTime);
        
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="springForce"></param>
    /// <param name="airTime"></param>
    public void TriggerBounce(Vector2 springForce, float airTime)
    {
        _animator.SetFloat("airTime", airTime);
        _animator.SetTrigger("triggerBump");
        _rigidBody.AddForce(springForce, ForceMode2D.Impulse);
    }
}
