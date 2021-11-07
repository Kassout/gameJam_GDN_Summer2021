using UnityEngine;

/// <summary>
/// Class <c>ArrowLauncherController</c> is a Unity component script used to manage the arrow launcher trap behaviour.
/// </summary>
public class ArrowLauncherController : ActionableObject
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    [SerializeField] 
    private ShootDirection shootDirection;
    
    /// <summary>
    /// Instance variable <c>arrow</c> represents the arrow game object launcher by the trap.
    /// </summary>
    [SerializeField] 
    private GameObject arrow;
    
    /// <summary>
    /// Instance variable <c>arrowLaunchSound</c> represents the <c>AudioSource</c> Unity component triggering arrow launching sound.
    /// </summary>
    [SerializeField] 
    private AudioSource arrowLaunchSound;

    /// <summary>
    /// Instance variable <c>vectorDirection</c> represents a <c>Vector2</c> Unity component of the arrow launching direction.
    /// </summary>
    private Vector2 _vectorDirection;

    /// <summary>
    /// Instance variable <c>animator</c> represents the arrow launcher animator Unity component.
    /// </summary>
    private Animator _animator;
    
    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    private enum ShootDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    
    /// <summary>
    /// Static variable <c>IsEventTriggered</c> represents the string message to send to the game object animator to change the state of the "isEventTriggered" variable.
    /// </summary>
    private static readonly int IsEventTriggered = Animator.StringToHash("isEventTriggered");

    /// <summary>
    /// Static variable <c>IsEventRepeated</c> represents the string message to send to the game object animator to change the state of the "isEventRepeated" variable.
    /// </summary>
    private static readonly int IsEventRepeated = Animator.StringToHash("isEventRepeated");

    /// <summary>
    /// Static variable <c>ActionTrigger</c> represents the string message to send to the game object animator to change the state of the "actionTrigger" variable.
    /// </summary>
    private static readonly int ActionTrigger = Animator.StringToHash("actionTrigger");

    /// <summary>
    /// Static variable <c>IsFromInteraction</c> represents the string message to send to the game object animator to change the state of the "isFromInteraction" variable.
    /// </summary>
    private static readonly int IsFromInteraction = Animator.StringToHash("isFromInteraction");
    
    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called on the frame when a script is enabled
    /// </summary>
    private void Start()
    {
        IsActive = false;
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsFromInteraction, isTriggeredFromInteraction);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (actionableStyle.Equals(ActionableStyle.Triggered) ||
            actionableStyle.Equals(ActionableStyle.TriggeredRepeat) ||
            actionableStyle.Equals(ActionableStyle.AutoWhenTriggered))
        {
            _animator.SetBool(IsEventTriggered, true);
            _animator.SetBool(IsEventRepeated, actionableStyle.Equals(ActionableStyle.TriggeredRepeat));
        }
        else
        {
            _animator.SetBool(IsEventRepeated, true);
        }

        switch (shootDirection) {
            case ShootDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.flipX = false;
                _vectorDirection = Vector2.up;
                break;
            case ShootDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                spriteRenderer.flipX = false;
                _vectorDirection = Vector2.down;
                break;
            case ShootDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipX = false;
                _vectorDirection = Vector2.left;
                break;
            case ShootDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipX = true;
                _vectorDirection = Vector2.right;
                break;
        }
    }

    #endregion

    #region Private
    
    /// <summary>
    /// This method is called to instantiate and launch an arrow projectile on animation launching frame.
    /// </summary>
    private void ArrowLauncherEvent()
    {
        arrowLaunchSound.Play();
        GameObject instantiatedArrow = Instantiate(arrow, GetComponent<Rigidbody2D>().position + (_vectorDirection * 0.6f), transform.rotation);
        instantiatedArrow.GetComponent<ArrowController>().SetDirection(_vectorDirection);
    }

#if UNITY_EDITOR
    /// <summary>
    /// This method is called when the script is loaded or a value is changed in the Inspector.
    /// </summary>
    private void OnValidate()
    {
        switch (shootDirection)
        {
            case ShootDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                GetComponent<SpriteRenderer>().flipX = false;
                _vectorDirection = Vector2.up;
                break;
            case ShootDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                GetComponent<SpriteRenderer>().flipX = false;
                _vectorDirection = Vector2.down;
                break;
            case ShootDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<SpriteRenderer>().flipX = false;
                _vectorDirection = Vector2.left;
                break;
            case ShootDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<SpriteRenderer>().flipX = true;
                _vectorDirection = Vector2.right;
                break;
        }
    }
#endif

    #endregion

    #region Public

    /// <summary>
    /// This method is used when the arrow launcher get triggered.
    /// </summary>
    public override void TriggerActionEvent()
    {
        if (actionableStyle.Equals(ActionableStyle.TriggeredRepeat) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            IsActive = true;
        }
        else if (actionableStyle.Equals(ActionableStyle.Triggered))
        {
            _animator.SetTrigger(ActionTrigger);
        }
        else if (actionableStyle.Equals(ActionableStyle.AutoWhenTriggered) && !IsActive)
        {
            _animator.SetTrigger(ActionTrigger);
            _animator.SetBool(IsEventRepeated, true);
            IsActive = true;
        }
    }

    /// <summary>
    /// This method is used when the arrow launcher get deactivated.
    /// </summary>
    public override void KillTriggers()
    {
        _animator.ResetTrigger(ActionTrigger);
        if(actionableStyle.Equals(ActionableStyle.AutoWhenTriggered) && IsActive) {
            IsActive = false;
            _animator.SetBool(IsEventRepeated, false);
        }
    }

    #endregion
}
