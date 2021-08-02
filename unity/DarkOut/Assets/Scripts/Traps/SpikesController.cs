using System;
using UnityEngine;

public class SpikesController : ActionableObject 
{
    private Animator _animator;
    
    private static readonly int IsEventTriggered = Animator.StringToHash("isEventTriggered");

    private static readonly int IsEventRepeated = Animator.StringToHash("isEventRepeated");

    private static readonly int ActionTrigger = Animator.StringToHash("actionTrigger");
    
    private void Start()
    {
        IsActive = false;
        _animator = GetComponent<Animator>();
        if (actionableTriggeringStyle.Equals(TriggeringStyle.Triggered) ||
            actionableTriggeringStyle.Equals(TriggeringStyle.TriggeredRepeat))
        {
            _animator.SetBool(IsEventTriggered, true);
            _animator.SetBool(IsEventRepeated, actionableTriggeringStyle.Equals(TriggeringStyle.TriggeredRepeat));
        }
        else
        {
            _animator.SetBool(IsEventRepeated, true);
        }
    }

    public void SpikeEvent()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = !gameObject.GetComponent<BoxCollider2D>().enabled;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kill player");
            PlayerController.Instance.TakeDamage();
        }
    }
    
    public override void TriggerActionEvent()
    {
        if (actionableTriggeringStyle.Equals(TriggeringStyle.TriggeredRepeat) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            IsActive = true;
        }
        else if (actionableTriggeringStyle.Equals(TriggeringStyle.Triggered))
        {
            _animator.SetTrigger(ActionTrigger);
        }
    }
}
