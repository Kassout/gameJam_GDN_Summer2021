using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Rigidbody2D _rigidbody2D;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private GameObject _currentInteractionObj;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _ghostStartingPosition = transform.position;
        s_playerOldCommands = new List<Command>(GameManager.OldCommands);
        s_playerOldDirections = new List<Vector2>(GameManager.OldDirections);
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
            //Stop the coroutine so it starts from the beginning
            if (_replayCoroutine != null)
            {
                StopCoroutine(_replayCoroutine);
            }

            //Start the replay
            _replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }
    
    //The replay coroutine
    IEnumerator ReplayCommands()
    {
        //Move the box to the start position
        _rigidbody2D.position = _ghostStartingPosition;

        for (int i = 0; i < s_playerOldCommands.Count; i++)
        {
            //Move the box with the current command
            s_playerOldCommands[i].Move(_rigidbody2D, s_playerOldDirections[i]);

            if (s_playerOldCommands[i].GetType() == typeof(PlayerInteract))
            {
                Interact();
            }

            yield return null; //new WaitForSeconds(0.3f);
        }
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
}
