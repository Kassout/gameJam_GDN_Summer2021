using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface <c>IReceiverObject</c> is used to defined the behaviours that should leads all the receivers objects.
/// </summary>
public interface IReceiverObject
{
    /// <summary>
    /// This method is used when the actionable objects reunite the triggering conditions.
    /// </summary>
    void OnReceiverTriggersActivated();
    
    /// <summary>
    /// This method is used when the actionable objects get out of triggering conditions.
    /// </summary>
    void OnReceiverTriggersDeactivated();
}

/// <summary>
/// Class <c>ReceiverObject</c> is a Unity component script used to manage the different player behaviours.
/// </summary>
public abstract class ReceiverObject : MonoBehaviour, IReceiverObject
{
    #region Fields / Properties
    
    /// <summary>
    /// Instance variable <c>triggeringObjects</c> represents the list of objects to trigger to activate.
    /// </summary>
    [SerializeField] 
    private List<TriggeringObject> triggeringObjects;

    /// <summary>
    /// Instance variable <c>_triggerCount</c> represents the current number of triggered actionable game object.
    /// </summary>
    private int _triggerCount;

    /// <summary>
    /// Instance variable <c>_activatedTriggeringObjects</c> represents the list of current triggered actionable game object.
    /// </summary>
    private List<TriggeringObject> _activatedTriggeringObjects;
    
    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called on the frame when a script is enabled.
    /// </summary>
    private void Start()
    {
        _activatedTriggeringObjects = new List<TriggeringObject>();
    }

    /// <summary>
    /// This method is called once per frame.
    /// </summary>
    private void Update()
    {
        UpdateReceiverStatus();
    }

    #endregion

    #region Private

    /// <summary>
    /// This method is called to update the receiver game object activation status.
    /// </summary>
    private void UpdateReceiverStatus()
    {
        foreach (var triggeringObject in triggeringObjects)
        {
            if (!_activatedTriggeringObjects.Contains(triggeringObject))
            {
                if(triggeringObject.IsActivated) {
                    _activatedTriggeringObjects.Add(triggeringObject);
                    _triggerCount++;
                    if (_triggerCount.Equals(triggeringObjects.Count))
                    {
                        OnReceiverTriggersActivated();
                    }
                }
            } else {
                if(!triggeringObject.IsActivated) {
                    if (_triggerCount.Equals(triggeringObjects.Count))
                    {
                        OnReceiverTriggersDeactivated();
                    }
                    _activatedTriggeringObjects.Remove(triggeringObject);
                    _triggerCount--;
                }
            }
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is used when the actionable objects reunite the triggering conditions.
    /// </summary>
    public abstract void OnReceiverTriggersActivated();

    /// <summary>
    /// This method is used when the actionable objects get out of triggering conditions.
    /// </summary>
    public abstract void OnReceiverTriggersDeactivated();

    #endregion
}
