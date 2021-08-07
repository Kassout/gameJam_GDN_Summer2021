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
    /// TODO: comments
    /// </summary>
    void Start()
    {
        _activatedTriggeringObjects = new List<TriggeringObject>();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    void Update()
    {
        UpdateReceiverStatus();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void UpdateReceiverStatus()
    {
        if (_triggerCount.Equals(triggeringObjects.Count))
        {
            OnReceiverTriggersActivated();
        }
        else
        {
            foreach (var triggeringObject in triggeringObjects)
            {
                if (!_activatedTriggeringObjects.Contains(triggeringObject) && triggeringObject.IsActivated)
                {
                    _activatedTriggeringObjects.Add(triggeringObject);
                    _triggerCount++;
                }
            }
        }
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    protected abstract void OnReceiverTriggersActivated();
}
