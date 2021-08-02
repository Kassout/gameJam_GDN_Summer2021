
using UnityEngine;

public interface IActionableObject
{
    void TriggerActionEvent();
}

public abstract class ActionableObject : MonoBehaviour, IActionableObject
{
    public bool IsActive { get; set; }
    
    [SerializeField]
    protected TriggeringStyle actionableTriggeringStyle;
    
    protected enum TriggeringStyle
    {
        Auto,
        Triggered,
        TriggeredRepeat
    }

    public abstract void TriggerActionEvent();
}