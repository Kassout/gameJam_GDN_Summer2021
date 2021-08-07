
using System;
using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class DoorController : ReceiverObject
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    private Animator _doorAnimator;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private AudioSource openDoorSound;

    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private AudioSource unlockTriggerSound;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void Awake()
    {
        _doorAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    protected override void OnReceiverTriggersActivated()
    {
        OpenDoor();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void OpenDoor()
    {
        _doorAnimator.SetTrigger(IsOpen);
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    public void OnOpenDoor()
    {
        openDoorSound.Play();
    }
}
