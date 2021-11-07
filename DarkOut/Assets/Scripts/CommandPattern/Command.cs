using System.Collections.Generic;
using UnityEngine;

//The parent class
public abstract class Command
{
    //How far should the box move when we press a button
    protected float moveSpeed = 2.5f;

    //Move and maybe save command
    public abstract void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command);

    //Undo an old command
    public virtual void Undo(Rigidbody2D rigidbody2D, Vector2 direction) { }
    
    //Move the box
    public virtual void Move(Rigidbody2D rigidbody2D, Vector2 direction) { }
}

public class PlayerMove : Command
{
    //Called when we press a key
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction,  Command command)
    {
        InputHandler.oldCommands.Add(command);
        InputHandler.oldDirections.Add(direction);
    }
    
    //Undo an old command
    public override void Undo(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        Vector2 move = rigidbody2D.position - direction.normalized * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(move);
    }

    //Move the box
    public override void Move(Rigidbody2D rigidbody2D, Vector2 direction)
    {
        float timing = Time.fixedDeltaTime;
        Vector2 move = rigidbody2D.position + direction.normalized * (moveSpeed * timing);
        rigidbody2D.MovePosition(move);
        GameManager.moveGhostRecord.Add(new Vector3(move.x, move.y, timing));
    }
}

public class PlayerInteract : PlayerMove
{
    
}


//For keys with no binding
public class DoNothing : Command
{
    //Called when we press a key
    public override void Execute(Rigidbody2D rigidbody2D, Vector2 direction, Command command)
    {
        //Nothing will happen if we press this key
    }
}

//Undo one command
public class UndoCommand : Command
{
    //Called when we press a key
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

