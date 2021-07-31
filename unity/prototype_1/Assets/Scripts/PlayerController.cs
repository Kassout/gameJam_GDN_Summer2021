using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    
    [SerializeField]
    private float moveSpeed = 5f;
    
    [SerializeField]
    private MovementStyle moveStyle;

    private Vector2 movement;

    [SerializeField]
    private float timeToMove = 0.2f;

    private bool isMoving = false;

    private void Start()
    {
        if (moveStyle.Equals(MovementStyle.TopDownStyle))
        {
            rigidBody = null;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
    }

    private enum MovementStyle
    {
        FreeStyle,
        TopDownStyle,
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
         // Movement
         if (moveStyle.Equals(MovementStyle.TopDownStyle))
         {
             if (movement.x < 0 && !isMoving)
             {
                 StartCoroutine(MovePlayerOnGrid(Vector3.left));
             }
             else if (movement.x > 0 && !isMoving)
             {
                 StartCoroutine(MovePlayerOnGrid(Vector3.right));
             }

             if (movement.y < 0 && !isMoving)
             {
                 StartCoroutine(MovePlayerOnGrid(Vector3.down));
             } 
             else if (movement.y > 0 && !isMoving)
             {
                 StartCoroutine(MovePlayerOnGrid(Vector3.up));
             }
             
         } else if (moveStyle.Equals(MovementStyle.FreeStyle))
         {
             MovePlayer();
         }
    }

    private void MovePlayer()
    {
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator MovePlayerOnGrid(Vector3 direction)
    {
        isMoving = true;
        
        float elapsedTime = 0.0f;
        Vector3 targetPos = transform.position + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    /*private void MovePlayerOnGrid()
    {
        // TODO : grid-pinned movement
        // transform.position = Vector3.MoveTowards(transform.position, movePoi);
    }*/

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // TODO : Do things on interact
        }
    }
}
