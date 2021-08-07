using System.Collections;
using UnityEngine;

/// <summary>
/// Class <c>ButtonController</c> is a Unity component script used to manage lever trigger object behaviour.
/// </summary>
public class LeverController : TriggeringObject
{
    /// <summary>
    /// Instance variable <c>holdTime</c> represents the time to hold a button when on the PushButton mode.
    /// </summary>
    [SerializeField] 
    protected float holdTime = 0.15f;

    /// <summary>
    /// Instance variable <c>colliderSizeOnPressure</c> represents the collider size value when activation on pressure.
    /// </summary>
    private float colliderSizeOnPressure = 0.2f;
    
    /// <summary>
    /// Instance variable <c>colliderSizeOnInteraction</c> represents the collider size value when activation on interaction.
    /// </summary>
    private float colliderSizeOnInteraction = 0.5f;
    
    /// <summary>
    /// Instance variable <c>leverAnimator</c> represents the lever Unity component animator.
    /// </summary>
    private Animator _leverAnimator;
    
    /// <summary>
    /// Instance variable <c>leverPushedSound</c> represents the <c>AudioSource</c> Unity component triggering lever pushed sound.
    /// </summary>
    [SerializeField]
    private AudioSource leverPushedSound;
    
    /// <summary>
    /// Static variable <c>Pushed</c> represents the string message to send to the game object animator to change the state of the "isPushed" variable.
    /// </summary>
    private static readonly int Pushed = Animator.StringToHash("isPushed");

    private IEnumerator activatedCoroutine;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        IsActivated = false;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        _leverAnimator = GetComponent<Animator>();
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
        if (activationType.Equals(ActivationType.Pressure))
        {
            leverPushedSound.Play();
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
    /// This method is called when the lever is activated.
    /// </summary>
    public override void OnActivate()
    {
        if (!activationType.Equals(ActivationType.Pressure))
        {
            leverPushedSound.Play();
        }
        IsActivated = true;
        _leverAnimator.SetBool(Pushed, IsActivated);
        foreach (var actionableObject in actionableObjects)
        {
            actionableObject.TriggerActionEvent();
        }
    }

    /// <summary>
    /// This method is used to trigger actions from pushing lever event.
    /// </summary>
    /// <returns>A <c>IEnumerator</c> object representing a list of controls.</returns>
    public override IEnumerator PushSequenceOnInteraction()
    {
        if (activationType.Equals(ActivationType.Interaction))
        {
            yield return new WaitForSeconds(holdTime);
        
            OnDeactivate();
        }
    }

    /// <summary>
    /// This method is called when the lever is deactivated.
    /// </summary>
    public override void OnDeactivate()
    {
        IsActivated = false;
        _leverAnimator.SetBool(Pushed, IsActivated);
        foreach (var actionableObject in actionableObjects)
        {
            actionableObject.KillTriggers();
        }
        activatedCoroutine = null;
    }

    public void PassCoroutineRef(IEnumerator coroutine) {
        if (activatedCoroutine != null) {
            StopCoroutine(activatedCoroutine);
        } else {
            OnActivate();
        }
        activatedCoroutine = coroutine;
    }
}
