using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface <c>ITriggeringObject</c> is used to defined the behaviours that should leads all the triggering objects.
/// </summary>
public interface ITriggeringObject
{
    /// <summary>
    /// This method is used when the triggering object get activated.
    /// </summary>
    void OnActivate();

    /// <summary>
    /// This method is used when the triggering object get deactivated.
    /// </summary>
    void OnDeactivate();
}

/// <summary>
/// Abstract class <c>TriggeringObject</c> is used to defined the main characteristics shared by all the triggering objects.
/// </summary>
public abstract class TriggeringObject : MonoBehaviour, ITriggeringObject
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>IsActivated</c> represents the state of activation of the triggering object.
    /// </summary>
    public bool IsActivated { get; set; }
    
    /// <summary>
    /// Instance variable <c>activationType</c> represents the activation style of the triggering object.
    /// </summary>
    [SerializeField] 
    protected ActivationType activationType;

    /// <summary>
    /// Instance variable <c>buttonType</c> represents the button type of the triggering object.
    /// </summary>
    [SerializeField] 
    protected ButtonType buttonType;
    
    /// <summary>
    /// Instance variable <c>actionableObjects</c> represents a list of actionable object that can get triggered by the triggering object.
    /// </summary>
    [SerializeField] 
    protected List<ActionableObject> actionableObjects;

    /// <summary>
    /// Instance variable <c>ActivationType</c> represents an enumeration of activation type for the triggering object.
    /// </summary>
    protected enum ActivationType
    {
        Interaction,
        Pressure
    }

    /// <summary>
    /// Instance variable <c>ButtonType</c> represents an enumeration of button type for the triggering object.
    /// </summary>
    protected enum ButtonType
    {
        PushButton,
        Switch
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is used when the triggering object get activated.
    /// </summary>
    public abstract void OnActivate();

    /// <summary>
    /// This method is used when the triggering object get deactivated.
    /// </summary>
    public abstract void OnDeactivate();
    
    /// <summary>
    /// This method is used to trigger actions from pushing event.
    /// </summary>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    public abstract IEnumerator PushSequenceOnInteraction();

    #endregion
}
