using UnityEngine;

/// <summary>
/// Class <c>SpikesController</c> is a Unity component script used to manage the spikes trap behaviour.
/// </summary>
public class SpikesController : ActionableObject 
{
    #region Fields/Variables

    /// <summary>
    /// Instance variable <c>spikesUpSound</c> represents the <c>AudioSource</c> Unity component triggering spikes sound.
    /// </summary>
    [SerializeField] private AudioSource spikesUpSound;
    
    /// <summary>
    /// Instance variable <c>animator</c> represents the spikes animator Unity component.
    /// </summary>
    private Animator _animator;
    
    /// <summary>
    /// Static variable <c>Pushed</c> represents the string message to send to the game object animator to change the state of the "isEventTriggered" variable.
    /// </summary>
    private static readonly int IsEventTriggered = Animator.StringToHash("isEventTriggered");

    /// <summary>
    /// Static variable <c>Pushed</c> represents the string message to send to the game object animator to change the state of the "isEventRepeated" variable.
    /// </summary>
    private static readonly int IsEventRepeated = Animator.StringToHash("isEventRepeated");

    /// <summary>
    /// Static variable <c>Pushed</c> represents the string message to send to the game object animator to change the state of the "actionTrigger" variable.
    /// </summary>
    private static readonly int ActionTrigger = Animator.StringToHash("actionTrigger");

    #endregion

    #region MonoBehaviour
    
    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        IsActive = false;
        _animator = GetComponent<Animator>();
        if (actionableStyle.Equals(ActionableStyle.Triggered) ||
            actionableStyle.Equals(ActionableStyle.TriggeredRepeat))
        {
            _animator.SetBool(IsEventTriggered, true);
            _animator.SetBool(IsEventRepeated, actionableStyle.Equals(ActionableStyle.TriggeredRepeat));
        }
        else
        {
            _animator.SetBool(IsEventRepeated, true);
        }
    }

    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kill player");
            other.GetComponent<PlayerController>().TakeDamage();
        }
    }
    
    #endregion

    #region Private

    /// <summary>
    /// This method is called to activate the box collider of the spikes on the animation trap triggering frame.
    /// </summary>
    private void SpikeEvent()
    {
        if (!spikesUpSound.isPlaying)
        {
            spikesUpSound.Play();
        }
        
        gameObject.GetComponent<BoxCollider2D>().enabled = !gameObject.GetComponent<BoxCollider2D>().enabled;
    }

    #endregion

    #region Public

    /// <summary>
    /// This method is used when the spikes trap get triggered.
    /// </summary>
    public override void TriggerActionEvent()
    {
        if (actionableStyle.Equals(ActionableStyle.TriggeredRepeat) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            IsActive = true;
        }
    }

    /// <summary>
    /// This method is used when the spikes trap get deactivated.
    /// </summary>
    public override void KillTriggers()
    {
        _animator.ResetTrigger(ActionTrigger);
    }

    #endregion
}
