using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>InputHandler</c> is a Unity component script used to manage the inputs behaviour.
/// </summary>
public class InputHandler : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance static variable <c>oldCommands</c> represents the list of old commands invoked by the player key inputs.
    /// </summary>
    public static List<Command> oldCommands;
    
    /// <summary>
    /// Instance static variable <c>oldDirections</c> represents the list of old directions associated to old commands invoked by the player key inputs.
    /// </summary>
    public static List<Vector2> oldDirections;

    #endregion

    #region Private

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        oldCommands = new List<Command>();
        oldDirections = new List<Vector2>();
    }

    #endregion
}
