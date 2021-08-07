
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

    BoxCollider2D collision;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void Awake()
    {
        _doorAnimator = GetComponent<Animator>();
        collision = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    protected override void OnReceiverTriggersActivated()
    {
        OpenDoor();
    }

    protected override void OnReceiverTriggersDeactivated()
    {
        CloseDoor();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void OpenDoor()
    {
        _doorAnimator.SetBool(IsOpen, true);
        collision.enabled = false;
    }

    private void CloseDoor() {
        _doorAnimator.SetBool(IsOpen, false);
        collision.enabled = true;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    public void OnOpenDoor()
    {
        openDoorSound.Play();
    }
}
