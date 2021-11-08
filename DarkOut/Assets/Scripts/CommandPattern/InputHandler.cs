using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class InputHandler : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    public static List<Command> oldCommands;
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    public static List<Vector2> oldDirections;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void Awake()
    {
        oldCommands = new List<Command>();
        oldDirections = new List<Vector2>();
    }

}
