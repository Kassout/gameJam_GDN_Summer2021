using UnityEngine;

/// <summary>
/// Class <c>ArrowLauncherController</c> is a Unity component script used to manage the arrow launcher trap behaviour.
/// </summary>
public class ArrowLauncherController : ActionableObject
{
    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    [SerializeField]
    private ShootDirection shootDirection;
    
    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    private enum ShootDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    private Vector2 vectorDirection;

    /// <summary>
    /// Instance variable <c>arrow</c> represents the arrow game object launcher by the trap.
    /// </summary>
    [SerializeField]
    private GameObject arrow;

    /// <summary>
    /// Instance variable <c>animator</c> represents the arrow launcher animator Unity component.
    /// </summary>
    private Animator _animator;
    
    /// <summary>
    /// Static variable <c>IsEventTriggered</c> represents the string message to send to the game object animator to change the state of the "isEventTriggered" variable.
    /// </summary>
    private static readonly int IsEventTriggered = Animator.StringToHash("isEventTriggered");

    /// <summary>
    /// Static variable <c>IsEventRepeated</c> represents the string message to send to the game object animator to change the state of the "isEventRepeated" variable.
    /// </summary>
    private static readonly int IsEventRepeated = Animator.StringToHash("isEventRepeated");

    /// <summary>
    /// Static variable <c>ActionTrigger</c> represents the string message to send to the game object animator to change the state of the "actionTrigger" variable.
    /// </summary>
    private static readonly int ActionTrigger = Animator.StringToHash("actionTrigger");

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        IsActive = false;
        _animator = GetComponent<Animator>();
        _animator.SetBool("isFromInteraction", isTriggeredFromInteraction);
        if (actionableStyle.Equals(ActionableStyle.Triggered) ||
            actionableStyle.Equals(ActionableStyle.TriggeredRepeat) ||
            actionableStyle.Equals(ActionableStyle.AutoWhenTriggered))
        {
            _animator.SetBool(IsEventTriggered, true);
            _animator.SetBool(IsEventRepeated, actionableStyle.Equals(ActionableStyle.TriggeredRepeat));
        }
        else
        {
            _animator.SetBool(IsEventRepeated, true);
        }

        switch (shootDirection) {
            case ShootDirection.Up:
                vectorDirection = Vector2.up;
                break;
            case ShootDirection.Down:
                vectorDirection = Vector2.down;
                break;
            case ShootDirection.Left:
                vectorDirection = Vector2.left;
                break;
            case ShootDirection.Right:
                vectorDirection = Vector2.right;
                break;
        }
    }

    /// <summary>
    /// This method is called to instantiate and launch an arrow projectile on animation launching frame.
    /// </summary>
    public void ArrowLauncherEvent()
    {
        GameObject instantiatedArrow = Instantiate(arrow, GetComponent<Rigidbody2D>().position + (vectorDirection * 0.6f), transform.rotation);
        instantiatedArrow.GetComponent<ArrowController>().SetDirection(vectorDirection);
    }

    /// <summary>
    /// This method is used when the arrow launcher get triggered.
    /// </summary>
    public override void TriggerActionEvent()
    {
        Debug.Log("Trigger " + gameObject);
        if (actionableStyle.Equals(ActionableStyle.TriggeredRepeat) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            IsActive = true;
        }
        else if (actionableStyle.Equals(ActionableStyle.Triggered))
        {
            _animator.SetTrigger(ActionTrigger);
        }
        else if (actionableStyle.Equals(ActionableStyle.AutoWhenTriggered) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            _animator.SetBool(IsEventRepeated, true);
            IsActive = true;
        }
    }

    /// <summary>
    /// This method is used when the arrow launcher get deactivated.
    /// </summary>
    public override void KillTriggers()
    {
        Debug.Log("Kill " + gameObject);
        _animator.ResetTrigger(ActionTrigger);
        if(actionableStyle.Equals(ActionableStyle.AutoWhenTriggered) && IsActive) {
            IsActive = false;
            _animator.SetBool(IsEventRepeated, false);
        }
    }
}
