using UnityEngine;

/// <summary>
/// Class <c>ButtonController</c> is a Unity component script used to manage button triggered object behaviour.
/// </summary>
public class ButtonController : TriggeringObject
{
    /// <summary>
    /// Instance variable <c>colliderSizeOnPosition</c> represents the collider size value when activation on pressure.
    /// </summary>
    private float colliderSizeOnPressure = 0.2f;

    /// <summary>
    /// Instance variable <c>colliderSizeOnInteraction</c> represents the collider size value when activation on interaction.
    /// </summary>
    private float colliderSizeOnInteraction = 0.5f;

    /// <summary>
    /// Instance variable <c>buttonAnimator</c> represents the button Unity component animator.
    /// </summary>
    private Animator _buttonAnimator;
    
    /// <summary>
    /// Static variable <c>Pushed</c> represents the string message to send to the game object animator to change the state of the "isPushed" variable.
    /// </summary>
    private static readonly int Pushed = Animator.StringToHash("isPushed");

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        IsActivated = false;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        _buttonAnimator = GetComponent<Animator>();
        if (activationType.Equals(ActivationType.Pressure))
        {
            boxCollider.size = new Vector2(colliderSizeOnPressure, colliderSizeOnPressure);
        }
        else if (activationType.Equals(ActivationType.Interaction))
        {
            boxCollider.size = new Vector2(colliderSizeOnInteraction, colliderSizeOnInteraction);
        }
    }
    
    /// <summary>
    /// This method is called when another object enters a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activationType.Equals(ActivationType.Interaction) && !IsActivated)
        {
            OnActivate();
        }
    }

    /// <summary>
    /// This method is called each frame where another object is within a trigger collider attached to this object
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (activationType.Equals(ActivationType.Pressure))
        {
            OnActivate();
        }
    }

    /// <summary>
    /// This method is called when another object leaves a trigger collider attached to this object.
    /// </summary>
    /// <param name="other">A <c>Collider2D</c> Unity component representing the collider of the object that it collides with.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (activationType.Equals(ActivationType.Pressure) && buttonType.Equals(ButtonType.PushButton))
        {
            OnDeactivate();
        }
    }

    /// <summary>
    /// This method is called when the button is activated.
    /// </summary>
    public override void OnActivate()
    {
        IsActivated = true;
        _buttonAnimator.SetBool(Pushed, IsActivated);
        foreach (var actionableObject in actionableObjects)
        {
            actionableObject.TriggerActionEvent();
        }
    }

    /// <summary>
    /// This method is called when the button is deactivated.
    /// </summary>
    public override void OnDeactivate()
    {
        IsActivated = false;
        _buttonAnimator.SetBool(Pushed, IsActivated);
        foreach (var actionableObject in actionableObjects)
        {
            actionableObject.KillTriggers();
        }
    }
}
