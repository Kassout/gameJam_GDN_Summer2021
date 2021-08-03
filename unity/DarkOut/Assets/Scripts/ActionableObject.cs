using UnityEngine;

/// <summary>
/// Interface <c>IActionableObject</c> is used to defined the behaviours that should leads all the actionable objects.
/// </summary>
public interface IActionableObject
{
    /// <summary>
    /// This method is used when the actionable object get triggered.
    /// </summary>
    void TriggerActionEvent();

    /// <summary>
    /// This method is used when the actionable object get out of triggering conditions.
    /// </summary>
    void KillTriggers();
}

/// <summary>
/// Abstract class <c>ActionableObject</c> is used to defined the main characteristics shared by all the actionable objects.
/// </summary>
public abstract class ActionableObject : MonoBehaviour, IActionableObject
{
    /// <summary>
    /// Instance variable <c>IsActive</c> represents the state of activity of the actionable object.
    /// </summary>
    protected bool IsActive { get; set; }

    /// <summary>
    /// Instance variable <c>IsTriggeredFromInteraction</c> represents the state of interaction triggered source of the actionable object.
    /// </summary>
    public bool isTriggeredFromInteraction;

    /// <summary>
    /// Instance variable <c>actionableStyle</c> represents the triggering style of the actionable object.
    /// </summary>
    [SerializeField]
    protected ActionableStyle actionableStyle;
    
    /// <summary>
    /// Instance variable <c>ActionableStyle</c> represents an enumeration of triggering style for the actionable object.
    /// </summary>
    protected enum ActionableStyle
    {
        Auto,
        Triggered,
        TriggeredRepeat
    }

    /// <summary>
    /// This method is used when the actionable object get triggered.
    /// </summary>
    public abstract void TriggerActionEvent();

    /// <summary>
    /// This method is used when the actionable object get out of triggering conditions.
    /// </summary>
    public abstract void KillTriggers();
}