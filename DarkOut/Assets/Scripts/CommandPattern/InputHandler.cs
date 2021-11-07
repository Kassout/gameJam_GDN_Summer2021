using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //Stores all commands for replay and undo
    public static List<Command> oldCommands;
    public static List<Vector2> oldDirections;

    private void Awake()
    {
        oldCommands = new List<Command>();
        oldDirections = new List<Vector2>();
    }

}
