using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class <c>InputHandler</c> is a Unity component script used to manage the inputs behaviour.
/// </summary>
public class InputHandler : MonoBehaviour
{
    #region Fields / Properties
    
    /// <summary>
    /// Instance field <c>inputActions</c> is a Unity <c>InputSystem</c> component object representing the general input bindings of the game.
    /// </summary>
    private InputController _inputController;

    /// <summary>
    /// Instance static variable <c>oldCommands</c> represents the list of old commands invoked by the player key inputs.
    /// </summary>
    public static List<Command> oldCommands;
    
    /// <summary>
    /// Instance static variable <c>oldDirections</c> represents the list of old directions associated to old commands invoked by the player key inputs.
    /// </summary>
    public static List<Vector2> oldDirections;
    
    /// <summary>
    /// Instance variable <c>movementInput</c> is a Unity <c>Vector2</c> component object representing the movement input vector of the player.
    /// </summary>
    public static Vector2 movementInput;
    
    /// <summary>
    /// Instance variable <c>lookAround</c> is a Unity <c>Vector2</c> component object representing the camera position input vector of the player.
    /// </summary>
    public static Vector2 lookAround;

    /// <summary>
    /// Instance variable <c>pauseInput</c> represents the pause input status of the game.
    /// </summary>
    public static bool pauseInput;
    
    /// <summary>
    /// Instance variable <c>interactionInput</c> represents the interaction input status of the game.
    /// </summary>
    public static bool interactionInput;
    
    /// <summary>
    /// Instance variable <c>replayInput</c> represents the replay input status of the game.
    /// </summary>
    public static bool replayInput;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        oldCommands = new List<Command>();
        oldDirections = new List<Vector2>();
    }
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (_inputController == null)
        {
            _inputController = new InputController();
            
            #if UNITY_WEBGL
                InputBinding moveBinding = _inputController.Player.Move.bindings[5];
                moveBinding.overrideProcessors = "InvertVector2(invertX=false)";
                _inputController.Player.Move.ApplyBindingOverride(5, moveBinding);
            #endif
            
            _inputController.Player.Move.performed += _ => movementInput = _.ReadValue<Vector2>();
            _inputController.Player.LookAround.performed += _ => lookAround = _.ReadValue<Vector2>();

            _inputController.Player.Pause.performed += _ => pauseInput = true;
            _inputController.Player.Interaction.performed += _ => interactionInput = true;
            _inputController.Player.Rewind.performed += _ => replayInput = true;
        }
            
        _inputController.Enable();
    }
        
    /// <summary>
    /// This function is called when the behaviour becomes disabled.
    /// </summary>
    private void OnDisable()
    {
        _inputController.Disable();
    }

    /// <summary>
    /// This function is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
        // To avoid calling input related methods twice in a frame.
        pauseInput = false;
        interactionInput = false;
        replayInput = false;
    }

    #endregion
}
