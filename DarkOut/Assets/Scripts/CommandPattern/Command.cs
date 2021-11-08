using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public abstract class Command
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    protected float moveSpeed = 2.5f;

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    /// <param name="command">TODO: comments</param>
    public abstract void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command);

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    public virtual void Undo(Rigidbody2D rigidbody2D, Vector2 direction) { }
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    public virtual void Move(Rigidbody2D rigidbody2D, Vector2 direction) { }
}

/// <summary>
/// TODO: comments
/// </summary>
public class PlayerMove : Command
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    /// <param name="command">TODO: comments</param>
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction,  Command command)
    {
        InputHandler.oldCommands.Add(command);
        InputHandler.oldDirections.Add(direction);
    }
    
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    public override void Undo(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        Vector2 move = rigidbody2D.position - direction.normalized * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(move);
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    public override void Move(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        Vector2 move = rigidbody2D.position + direction.normalized * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(move);
    }
}

/// <summary>
/// TODO: comments
/// </summary>
public class PlayerInteract : PlayerMove {}


/// <summary>
/// TODO: comments
/// </summary>
public class DoNothing : Command
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    /// <param name="command">TODO: comments</param>
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command)
    {
        //Nothing will happen if we press this key
    }
}

/// <summary>
/// TODO: comments
/// </summary>
public class UndoCommand : Command
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    /// <param name="rigidbody2D">TODO: comments</param>
    /// <param name="direction">TODO: comments</param>
    /// <param name="command">TODO: comments</param>
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
}

