using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface <c>IReceiverObject</c> is used to defined the behaviours that should leads all the receivers objects.
/// </summary>
public interface IReceiverObject
{
    /// <summary>
    /// This method is used when the actionable object get out of triggering conditions.
    /// </summary>
    void OnReceiverTriggersActivated();
}

/// <summary>
/// TODO: comment
/// </summary>
public abstract class ReceiverObject : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField] private List<TriggeringObject> triggeringObjects;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private int _triggerCount = 0;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private List<TriggeringObject> _activatedTriggeringObjects;

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        _activatedTriggeringObjects = new List<TriggeringObject>();
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        UpdateReceiverStatus();
    }

    /// <summary>
    /// TODO: comments
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

    /// <summary>
    /// TODO: comments
    /// </summary>
    protected abstract void OnReceiverTriggersActivated();

    /// <summary>
    /// TODO: comments
    /// </summary>
    protected abstract void OnReceiverTriggersDeactivated();
}
