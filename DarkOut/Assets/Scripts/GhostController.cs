using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>GhostController</c> is a Unity component script used to manage the different player ghost behaviours.
/// </summary>
public class GhostController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>tilemapCollisionPoint</c> represents the player's ghost collision point game object to interact with tilemaps.
    /// </summary>
    public GameObject tilemapCollisionPoint;

    /// <summary>
    /// Instance variable <c>StartingPosition</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    public static Vector3 startingPosition;
    
    /// <summary>
    /// Instance variable <c>isBouncing</c> represents the bouncing status of the player's ghost.
    /// </summary>
    public bool isBouncing;
    
    /// <summary>
    /// Static variable <c>s_playerOldCommands</c> represents the list of old commands invoked by the player before the last replay command invocation.
    /// </summary>
    private static List<Command> s_playerOldCommands = new List<Command>();
    
    /// <summary>
    /// Static variable <c>s_playerOldDirections</c> represents the list of old directions related to the old commands invoked by the player before the last replay command invocation.
    /// </summary>
    private static List<Vector2> s_playerOldDirections = new List<Vector2>();
    
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
    /// Instance variable <c>_ghostStartingPosition</c> represents the player's ghost starting position.
    /// </summary>
    private Vector2 _ghostStartingPosition;

    /// <summary>
    /// Instance variable <c>_rigidBody</c> represents the player's ghost rigidbody.
    /// </summary>
    private Rigidbody2D _rigidBody;

    /// <summary>
    /// Instance variable <c>_currentInteractionObj</c> represents the triggering game object the player is in the range of activation.
    /// </summary>
    private GameObject _currentInteractionObj;

    /// <summary>
    /// Instance variable <c>_animator</c> represents the player's ghost animator controller.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance variable <c>_commandCount</c> represents the number of command replayed since the ghost get instanced and the replay command invoked. 
    /// </summary>
    private int _commandCount;

    /// <summary>
    /// Instance variable <c>_isDisabled</c> represents the disabled status of the player's ghost.
    /// </summary>
    private bool _isDisabled;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Vector3 position = transform.position;
        
        _rigidBody = GetComponent<Rigidbody2D>();
        _ghostStartingPosition = position;
        startingPosition = position;
        
        _animator = gameObject.GetComponent<Animator>();
        s_playerOldCommands = new List<Command>(GameManager.oldCommands);
        s_playerOldDirections = new List<Vector2>(GameManager.oldDirections);
            
        _commandCount = 0;
        _isDisabled = false;
        isBouncing = false;
    }

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        StartReplay();
    }

    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate() 
    {
        ReplayCommands();
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
    /// This method is called on player's replay command invocation.
    /// </summary>
    private void StartReplay()
    {
        if (s_playerOldCommands.Count > 0)
        {
            _commandCount = 0;
            _rigidBody.position = _ghostStartingPosition;
        }
    }

    /// <summary>
    /// This method is responsible for replaying the recorded player's command.
    /// </summary>
    private void ReplayCommands()
    {
        if (_commandCount < s_playerOldCommands.Count) {
            //Move the box with the current command
            if (s_playerOldCommands[_commandCount].GetType() == typeof(PlayerInteract))
            {
                Interact();
            }
            else if (!_isDisabled)
            {
                s_playerOldCommands[_commandCount].Move(_rigidBody, s_playerOldDirections[_commandCount]);
            }
            _commandCount += 1;
        }
    }

    /// <summary>
    /// This method is used to interact with game object of <c>TriggeringObject</c> class.
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
    /// This method is used to disable player control inputs when being pushed by a spring.
    /// </summary>
    private void StartSpring()
    {
        _isDisabled = true;
        isBouncing = true;
    }

    /// <summary>
    /// This method is used to enable player control inputs after being pushed by a spring.
    /// </summary>
    private void StopSpring()
    {
        _isDisabled = false;
        _rigidBody.velocity = Vector2.zero;
        isBouncing = false;
    }
    
    /// <summary>
    /// This method is used to restart the player to the game beginning position.
    /// </summary>
    private void RestartPosition()
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

    #endregion

    #region Public

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

    #endregion
}
