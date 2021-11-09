using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class <c>Command</c> is responsible for define the key binding architecture of the game for the children class to operates.
/// </summary>
public abstract class Command
{
    #region Fields / Properties
    
    /// <summary>
    /// Instance variable <c>moveSpeed</c> represents the speed value associate with a movement command.
    /// </summary>
    protected float moveSpeed = 2.5f;
    
    #endregion

    #region Public

    /// <summary>
    /// This method is called to execute a given command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    /// <param name="command">A <c>Command</c> object representing the command type called.</param>
    public abstract void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command);

    /// <summary>
    /// This method is called to reverse the last executed command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    public virtual void Undo(Rigidbody2D rigidbody2D, Vector2 direction) { }
    
    /// <summary>
    /// This method is called to execute a move type command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    public virtual void Move(Rigidbody2D rigidbody2D, Vector2 direction) { }

    #endregion
}

/// <summary>
/// Class <c>PlayerMove</c> is a Command class responsible for define the player behaviour answering command invocation.
/// </summary>
public class PlayerMove : Command
{
    #region Public

    /// <summary>
    /// This method is called to execute a given player command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    /// <param name="command">A <c>Command</c> object representing the command type called.</param>
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command)
    {
        InputHandler.oldCommands.Add(command);
        InputHandler.oldDirections.Add(direction);
    }
    
    /// <summary>
    /// This method is called to reverse the last executed player command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    public override void Undo(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        Vector2 move = rigidbody2D.position - direction.normalized * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(move);
    }

    /// <summary>
    /// This method is called to execute a move player type command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    public override void Move(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        Vector2 move = rigidbody2D.position + direction.normalized * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(move);
    }

    #endregion
}

/// <summary>
/// Class <c>PlayerInteract</c> is a Command class responsible for define the player interaction behaviour answering command invocation.
/// </summary>
public class PlayerInteract : PlayerMove {}


/// <summary>
/// Class <c>DoNothing</c> is a Command class responsible for define a base class for command without defined purpose.
/// </summary>
public class DoNothing : Command
{
    #region Public

    /// <summary>
    /// This method is called to execute a given command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    /// <param name="command">A <c>Command</c> object representing the command type called.</param>
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command)
    {
        //Nothing will happen if we press this key
    }

    #endregion
}

/// <summary>
/// Class <c>UndoCommand</c> is a Command class responsible for define the undo command behaviour answering command invocation.
/// </summary>
public class UndoCommand : Command
{
    #region Public 

    /// <summary>
    /// This method is called to execute a given undo command.
    /// </summary>
    /// <param name="rigidbody2D">A <c>Rigidbody2D</c> Unity component representing the object rigidbody associate to the invoked command.</param>
    /// <param name="direction">A <c>Vector2</c> Unity component representing the object direction vector associate to the invoked command.</param>
    /// <param name="command">A <c>Command</c> object representing the command type called.</param>
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command)
    {
        List<Command> oldCommands = InputHandler.oldCommands;
        List<Vector2> directions = InputHandler.oldDirections;

        if (oldCommands.Count > 0)
        {
            Command latestCommand = oldCommands[oldCommands.Count - 1];

            //Move the box with this command
            latestCommand.Undo(rigidbody2D, directions[oldCommands.Count - 1]);

            //Remove the command from the list
            oldCommands.RemoveAt(oldCommands.Count - 1);
        }
    }

    #endregion
}

