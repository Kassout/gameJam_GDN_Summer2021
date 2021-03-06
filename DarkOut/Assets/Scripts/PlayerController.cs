using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class <c>PlayerController</c> is a Unity component script used to manage the different player behaviours.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Static variable <c>StartingPosition</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    public static Vector3 startingPosition;
    
    /// <summary>
    /// Instance variable <c>isBouncing</c> represents the bouncing status of the player.
    /// </summary>
    public bool isBouncing;
    
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
    /// Instance variable <c>pitfallTileMap</c> represents the tile map containing the different pit tiles the player could fall into.
    /// </summary>
    [SerializeField]
    private Tilemap pitfallTileMap;
    
    /// <summary>
    /// Instance variable <c>tilemapCollisionPoint</c> represents the player's collision point game object to interact with tilemaps.
    /// </summary>
    public GameObject tilemapCollisionPoint;

    /// <summary>
    /// Instance variable <c>walkingSound</c> represents the <c>AudioSource</c> Unity component triggering player walking sound.
    /// </summary>
    [SerializeField]
    private AudioSource walkingSound;

    /// <summary>
    /// Instance variable <c>deathSound</c> represents the <c>AudioSource</c> Unity component triggering player death sound.
    /// </summary>
    [SerializeField] 
    private AudioSource deathSound;
    
    /// <summary>
    /// Static variable <c>TriggerDamage</c> represents the string message of the animator trigger variable "triggerDamage".
    /// </summary>
    private static readonly int TriggerDamage = Animator.StringToHash("triggerDamage");
    
    /// <summary>
    /// Static variable <c>AirTime</c> represents the string message to send to the game object animator to change the state of the "airTime" variable.
    /// </summary>
    private static readonly int AirTime = Animator.StringToHash("airTime");
    
    /// <summary>
    /// Static variable <c>TriggerBump</c> represents the string message to send to the game object animator to change the state of the "triggerBump" variable.
    /// </summary>
    private static readonly int TriggerBump = Animator.StringToHash("triggerBump");
    
    /// <summary>
    /// Static variable <c>TriggerFall</c> represents the string message to send to the game object animator to change the state of the "triggerFall" variable.
    /// </summary>
    private static readonly int TriggerFall = Animator.StringToHash("triggerFall");

    /// <summary>
    /// Instance variable <c>_buttonMove</c> represents the player's move command.
    /// </summary>
    private Command _buttonMove;
    
    /// <summary>
    /// Instance variable <c>_buttonInteract</c> represents the player's interact command.
    /// </summary>
    private Command _buttonInteract;
    
    /// <summary>
    /// Instance variable <c>walkSprite</c> represents the player's character walking sprite.
    /// </summary>
    private Sprite _idleSprite;
    
    /// <summary>
    /// Instance variable <c>rigidBody</c> represents the player's rigidbody.
    /// </summary>
    private Rigidbody2D _rigidBody;

    /// <summary>
    /// Instance variable <c>_animator</c> represents the player's animator controller.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance variable <c>isDisabled</c> represents the state of player control inputs.
    /// </summary>
    private bool _isDisabled;

    /// <summary>
    /// Instance variable <c>movement</c> represents the player's movement.
    /// </summary>
    private Vector2 _movement;

    /// <summary>
    /// Instance variable <c>currentInteractionObj</c> represents the triggering game object the player is in the range of activation.
    /// </summary>
    private GameObject _currentInteractionObj;

    /// <summary>
    /// Instance variable <c>move</c> represents movement vector of the player.
    /// </summary>
    private Vector2 _move;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _idleSprite = characterSprite.sprite;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        startingPosition = transform.position;
        
        _buttonMove = new PlayerMove();
        _buttonInteract = new PlayerInteract();
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        // Input
        if (!_isDisabled && !GameManager.Instance.blockPlayer)
        {
            _movement = InputHandler.movementInput;
            
#if UNITY_WEBGL
            _movement.y = -_movement.y;
#endif
            
            characterSprite.flipX = _movement.x < 0;
        
            if (_movement.x != 0 || _movement.y != 0)
            {
                characterSprite.sprite = walkSprite;
                if (!walkingSound.isPlaying)
                {
                    walkingSound.Play();
                }
            }
            else
            {
                characterSprite.sprite = _idleSprite;
                if (walkingSound.isPlaying)
                {
                    walkingSound.Pause();
                }
            }
            
            // Interact
            Interact();
            
            // Replay
            Replay();
        }
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate()
    {
        // Move
        if (!_isDisabled && !GameManager.Instance.blockPlayer)
        {
            MovePlayer();
        }
    }

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("InteractionObject") || other.gameObject.CompareTag("Spring"))
        {
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
            _currentInteractionObj = null;
        }
    }
    
    #endregion

    #region Private

        /// <summary>
    /// This method is used to move the player.
    /// </summary>
    private void MovePlayer()
    {
        _move = _rigidBody.position + _movement.normalized * (moveSpeed * Time.fixedDeltaTime);
        if (CanMove(_move - _rigidBody.position))
        {
            _rigidBody.MovePosition(_move);
            _buttonMove.Execute(_rigidBody, _movement, _buttonMove);
        }
    }
    
    /// <summary>
    /// This method is used to check if whether or not a player can move in the targeted direction considerate eventual obstacles.
    /// </summary>
    /// <param name="direction">A <c>Vector3</c> Unity structure representing the movement direction of the player.</param>
    /// <returns>A boolean value representing the state of movement allowance.</returns>
    private bool CanMove(Vector3 direction)
    {
        Vector3Int gridPosition = groundTileMap.WorldToCell(tilemapCollisionPoint.transform.position + direction * 1.05f);
        if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition) || pitfallTileMap.HasTile(gridPosition))
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
        if (InputHandler.interactionInput && _currentInteractionObj && _currentInteractionObj.GetComponent<TriggeringObject>())
        {
            _buttonInteract.Execute(_rigidBody, _movement, _buttonInteract);
            IEnumerator coroutine = _currentInteractionObj.GetComponent<TriggeringObject>().PushSequenceOnInteraction();
            LeverController lever = _currentInteractionObj.GetComponent<LeverController>();
            if(lever != null) {
                lever.PassCoroutineRef(coroutine);
            }
            StartCoroutine(coroutine);
        }
    }

    /// <summary>
    /// This method is called on player's replay command invocation.
    /// </summary>
    private void Replay()
    {
        if (InputHandler.replayInput)
        {
            GameManager.Instance.PlayerReplay();
        }
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
    
    #endregion

    #region Public

    /// <summary>
    /// This method is called when the player get in collision with a hurting game object.
    /// </summary>
    public void TakeDamage()
    {
        StartCoroutine(GetHurt());
    }

    /// <summary>
    /// This method is called when the player trigger a bouncing mechanism.
    /// </summary>
    /// <param name="springForce">A <c>Vector2</c> Unity component representing the spring force value of the bounce.</param>
    /// <param name="airTime">A float value representing the time the player will be in the air while bounced.</param>
    public void TriggerBounce(Vector2 springForce, float airTime)
    {
        _animator.SetFloat(AirTime, airTime);
        _animator.SetTrigger(TriggerBump);
        _rigidBody.AddForce(springForce, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// This method is used to disable player control inputs when being pushed by a spring.
    /// </summary>
    public void StartSpring()
    {
        _isDisabled = true;
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
        isBouncing = true;
    }

    /// <summary>
    /// This method is used to enable player control inputs after being pushed by a spring.
    /// </summary>
    public void StopSpring()
    {
        _isDisabled = false;
        _rigidBody.velocity = Vector2.zero;
        isBouncing = false;
    }

    /// <summary>
    /// This method is used to restart the player to the game beginning position.
    /// </summary>
    public void RestartPosition()
    {
        _animator.ResetTrigger(TriggerFall);
        deathSound.Stop();
        transform.position = startingPosition;
        if(_currentInteractionObj != null) {
            if (_currentInteractionObj.CompareTag("Spring"))
            {
                _currentInteractionObj.transform.parent.GetComponent<SpringBoxController>().RestartPosition();
            }
        }
    }

    /// <summary>
    /// This method is called when the player fall into a pit.
    /// </summary>
    public void PlayerFall()
    {
        deathSound.Play();
    }

    #endregion
}
