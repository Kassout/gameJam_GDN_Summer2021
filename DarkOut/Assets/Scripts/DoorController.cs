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
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    /// <summary>
    /// TODO: comments
    /// </summary>
    private BoxCollider2D _collision;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void Awake()
    {
        _doorAnimator = GetComponent<Animator>();
        _collision = GetComponent<BoxCollider2D>();
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
        _collision.enabled = false;
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    private void CloseDoor() {
        _doorAnimator.SetBool(IsOpen, false);
        _collision.enabled = true;
    }
}
