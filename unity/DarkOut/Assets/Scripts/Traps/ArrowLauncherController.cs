using UnityEngine;

/// <summary>
/// Class <c>ArrowLauncherController</c> is a Unity component script used to manage the arrow launcher trap behaviour.
/// </summary>
public class ArrowLauncherController : ActionableObject
{
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
            actionableStyle.Equals(ActionableStyle.TriggeredRepeat))
        {
            _animator.SetBool(IsEventTriggered, true);
            _animator.SetBool(IsEventRepeated, actionableStyle.Equals(ActionableStyle.TriggeredRepeat));
        }
        else
        {
            _animator.SetBool(IsEventRepeated, true);
        }
    }

    /// <summary>
    /// This method is called to instantiate and launch an arrow projectile on animation launching frame.
    /// </summary>
    public void ArrowLauncherEvent()
    {
        Instantiate(arrow, transform.position, transform.rotation);
    }

    /// <summary>
    /// This method is used when the arrow launcher get triggered.
    /// </summary>
    public override void TriggerActionEvent()
    {
        if (actionableStyle.Equals(ActionableStyle.TriggeredRepeat) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            IsActive = true;
        }
        else if (actionableStyle.Equals(ActionableStyle.Triggered))
        {
            _animator.SetTrigger(ActionTrigger);
        }
    }

    /// <summary>
    /// This method is used when the arrow launcher get deactivated.
    /// </summary>
    public override void KillTriggers()
    {
        _animator.ResetTrigger(ActionTrigger);
    }
}
