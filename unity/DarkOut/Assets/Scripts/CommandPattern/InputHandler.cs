using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //The box we control with keys
    private Rigidbody2D _playerRigidBody;

    //The different keys we need
    private Command _buttonMove, _buttonInteract;
    
    //Stores all commands for replay and undo
    public static List<Command> OldCommands;
    public static List<Vector2> OldDirections;

    private void Awake()
    {
        OldCommands = new List<Command>();
        OldDirections = new List<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Bind keys with commands
        _buttonMove = new PlayerMove();
        _buttonInteract = new PlayerInteract();
        _playerRigidBody = new Rigidbody2D();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (moveDirection.magnitude != 0)
        {
            _buttonMove.Execute(_playerRigidBody, moveDirection, _buttonMove);
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManager.Instance.PlayerReplay(OldCommands, OldDirections);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            _buttonInteract.Execute(_playerRigidBody, moveDirection, _buttonInteract);
        }
    }
}
