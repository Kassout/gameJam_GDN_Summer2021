using UnityEngine;

/// <summary>
/// Class <c>EndDoorController</c> is a Unity component script used to manage the end game door behaviour.
/// </summary>
public class EndDoorController : ReceiverObject
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>openDoorSound</c> represents the open end game door audio source.
    /// </summary>
    [SerializeField]
    private AudioSource openDoorSound;

    /// <summary>
    /// Instance variable <c>endPoint</c> represents end game point game object.
    /// </summary>
    [SerializeField]
    private GameObject endPoint;

    /// <summary>
    /// Instance variable <c>_doorAnimator</c> represents the door Unity component animator.
    /// </summary>
    private Animator _doorAnimator;

    /// <summary>
    /// Instance variable <c>_collision</c> represents the door Unity component box collider.
    /// </summary>
    private BoxCollider2D _collision;
    
    /// <summary>
    /// Static variable <c>IsOpen</c> represents the string message to send to the game object animator to change the state of the "IsOpen" variable.
    /// </summary>
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _doorAnimator = GetComponent<Animator>();
        _collision = GetComponent<BoxCollider2D>();
    }

    #endregion

    #region Private

    /// <summary>
    /// This method is called to open the door.
    /// </summary>
    private void OpenDoor()
    {
        _doorAnimator.SetBool(IsOpen, true);
        _collision.enabled = false;
    }

    /// <summary>
    /// This method is called to close the door.
    /// </summary>
    private void CloseDoor() {
        _doorAnimator.SetBool(IsOpen, false);
        _collision.enabled = true;
    }

    #endregion

    #region Protected

    /// <summary>
    /// This method is called on activation trigger event.
    /// </summary>
    public override void OnReceiverTriggersActivated()
    {
        OpenDoor();
    }

    /// <summary>
    /// This method is called on deactivation trigger event.
    /// </summary>
    public override void OnReceiverTriggersDeactivated()
    {
        CloseDoor();
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is called at door opening.
    /// </summary>
    public void OnOpenDoor()
    {
        openDoorSound.Play();
    }

    /// <summary>
    /// This method is called at door end opening.
    /// </summary>
    public void OnDoorWideOpen()
    {
        endPoint.SetActive(true);
    }

    #endregion
}
