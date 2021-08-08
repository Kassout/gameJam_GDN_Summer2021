using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private int frameCount;

    [SerializeField]
    public GameObject TilemapCollisionPoint;

    /// <summary>
    /// Instance variable <c>StartingPosition</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    public static Vector3 StartingPosition;

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

    public Tilemap groundTileMap;
    public Tilemap collisionTileMap;
    public Tilemap pitfallTileMap;

    private bool _isDisabled;
    public bool isBouncing;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _ghostStartingPosition = transform.position;
        _animator = gameObject.GetComponent<Animator>();
        s_playerOldCommands = new List<Command>(GameManager.OldCommands);
        s_playerOldDirections = new List<Vector2>(GameManager.OldDirections);
        frameCount = 0;
        _isDisabled = false;
        isBouncing = false;
        StartingPosition = transform.position;
        groundTileMap = GameManager.Instance.GetGround();
        pitfallTileMap = GameManager.Instance.GetPitfall();
        collisionTileMap = GameManager.Instance.GetCollision();
    }

    private void Start()
    {
        StartReplay();
    }

    //Checks if we should start the replay
    void StartReplay()
    {
        if (s_playerOldCommands.Count > 0)
        {
            frameCount = 0;
            _rigidBody.position = _ghostStartingPosition;
        }
    }

    void FixedUpdate() {
        ReplayCommands();
    }

    private void ReplayCommands()
    {
        if (frameCount < s_playerOldCommands.Count) {
            //Move the box with the current command
            
            if(CanMove((Vector3)s_playerOldDirections[frameCount]) && !_isDisabled) {
                s_playerOldCommands[frameCount].Move(_rigidBody, s_playerOldDirections[frameCount]);
            }

            if (s_playerOldCommands[frameCount].GetType() == typeof(PlayerInteract))
            {
                Interact();
            }
            frameCount += 1;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3Int gridPosition = groundTileMap.WorldToCell(TilemapCollisionPoint.transform.position + direction * 0.05f);
        if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition) || pitfallTileMap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }

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

    public void RestartPosition()
    {
        _animator.ResetTrigger(TriggerFall);
        transform.position = StartingPosition;
        if(_currentInteractionObj != null) {
            if (_currentInteractionObj.CompareTag("Spring"))
            {
                _currentInteractionObj.transform.parent.GetComponent<SpringBoxController>().RestartPosition();
            }
        }
    }
}
