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

    private bool ePressed;
    private bool yPressed;

    private void Awake()
    {
        OldCommands = new List<Command>();
        OldDirections = new List<Vector2>();
        ePressed = false;
        yPressed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Bind keys with commands
        _buttonMove = new PlayerMove();
        _buttonInteract = new PlayerInteract();
        _playerRigidBody = new Rigidbody2D();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            yPressed = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.Instance.blockPlayer)
        {
            HandleInput();
        }
    }

    public void HandleInput()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (ePressed)
        {
            _buttonInteract.Execute(_playerRigidBody, moveDirection, _buttonInteract);
            ePressed = false;
        } else {
            _buttonMove.Execute(_playerRigidBody, moveDirection, _buttonMove);
        }
        if (yPressed)
        {
            GameManager.Instance.PlayerReplay();
            yPressed = false;
        }
    }
}
