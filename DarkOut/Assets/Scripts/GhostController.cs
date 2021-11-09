using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// TODO: comments
/// </summary>
public class GhostController : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    private static List<Command> s_playerOldCommands = new List<Command>();
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private static List<Vector2> s_playerOldDirections = new List<Vector2>();

    /// <summary>
    /// TODO: comments
    /// </summary>
    private Vector2 _ghostStartingPosition;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Coroutine _replayCoroutine;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Rigidbody2D _rigidBody;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private GameObject _currentInteractionObj;

    /// <summary>
    /// Instance variable <c>_animator</c> represents the player's animator controller.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private int _frameCount;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    public GameObject tilemapCollisionPoint;

    /// <summary>
    /// Instance variable <c>StartingPosition</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    public static Vector3 startingPosition;

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
    /// TODO: comments
    /// </summary>
    public Tilemap groundTileMap;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public Tilemap collisionTileMap;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public Tilemap pitfallTileMap;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private bool _isDisabled;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public bool isBouncing;

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _ghostStartingPosition = transform.position;
        _animator = gameObject.GetComponent<Animator>();
        s_playerOldCommands = new List<Command>(GameManager.OldCommands);
        s_playerOldDirections = new List<Vector2>(GameManager.OldDirections);
        _frameCount = 0;
        _isDisabled = false;
        isBouncing = false;
        startingPosition = transform.position;
        groundTileMap = GameManager.Instance.GetGround();
        pitfallTileMap = GameManager.Instance.GetPitfall();
        collisionTileMap = GameManager.Instance.GetCollision();
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        StartReplay();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    void StartReplay()
    {
        if (s_playerOldCommands.Count > 0)
        {
            _frameCount = 0;
            _rigidBody.position = _ghostStartingPosition;
        }
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate() 
    {
        ReplayCommands();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void ReplayCommands()
    {
        if (_frameCount < s_playerOldCommands.Count) {
            //Move the box with the current command
            if (s_playerOldCommands[_frameCount].GetType() == typeof(PlayerInteract))
            {
                Interact();
            }
            else if (!_isDisabled)
            {
                s_playerOldCommands[_frameCount].Move(_rigidBody, s_playerOldDirections[_frameCount]);
            }
            _frameCount += 1;
        }
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void Interact()
    {
        if (_currentInteractionObj && _currentInteractionObj.GetComponent<TriggeringObject>())
        {
            IEnumerator coroutine = _currentInteractionObj.GetComponent<TriggeringObject>().PushSequenceOnInteraction();
            LeverController lever = _currentInteractionObj.GetComponent<LeverController>();
            if(lever != null) {
                lever.PassCoroutineRef(coroutine);
            }
            StartCoroutine(coroutine);
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

    /// <summary>
    /// This method is used to disable player control inputs when being pushed by a spring.
    /// </summary>
    public void StartSpring()
    {
        _isDisabled = true;
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
    /// TODO: comments
    /// </summary>
    public void RestartPosition()
    {
        _animator.ResetTrigger(TriggerFall);
        transform.position = startingPosition;
        if(_currentInteractionObj != null) {
            if (_currentInteractionObj.CompareTag("Spring"))
            {
                _currentInteractionObj.transform.parent.GetComponent<SpringBoxController>().RestartPosition();
            }
        }
    }
}
