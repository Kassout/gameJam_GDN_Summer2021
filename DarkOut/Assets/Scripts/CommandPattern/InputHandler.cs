using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //The box we control with keys
    private Rigidbody2D _playerRigidBody;

    //The different keys we need
    private Command _buttonMove, _buttonInteract;
    
    //Stores all commands for replay and undo
    public static List<Command> oldCommands;
    public static List<Vector2> oldDirections;

    private bool _ePressed;
    private bool _yPressed;

    private void Awake()
    {
        oldCommands = new List<Command>();
        oldDirections = new List<Vector2>();
        _ePressed = false;
        _yPressed = false;
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
        if (!GameManager.Instance.blockPlayer)
        {
            HandleInput();
        }
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            _ePressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _yPressed = true;
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void HandleInput()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            _buttonInteract.Execute(_playerRigidBody, moveDirection, _buttonInteract);
            _ePressed = false;
        } else {
            _buttonMove.Execute(_playerRigidBody, moveDirection, _buttonMove);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManager.Instance.PlayerReplay();
            _yPressed = false;
        }
    }
}
