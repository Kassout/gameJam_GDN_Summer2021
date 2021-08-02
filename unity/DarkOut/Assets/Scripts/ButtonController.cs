using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private ActivationType activationType;

    [SerializeField] private ButtonType buttonType;
    
    [SerializeField] private List<ActionableObject> actionableObjects;

    private float colliderSizeOnPosition = 0.2f;

    private float colliderSizeOnInteraction = 0.5f;

    private Animator _buttonAnimator;

    private bool _isPushed = false;
        
    private enum ActivationType
    {
        Interaction,
        Position
    }

    private enum ButtonType
    {
        PushButton,
        Switch
    }

    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        _buttonAnimator = GetComponent<Animator>();
        if (activationType.Equals(ActivationType.Position))
        {
            boxCollider.size = new Vector2(colliderSizeOnPosition, colliderSizeOnPosition);
        }
        else if (activationType.Equals(ActivationType.Interaction))
        {
            boxCollider.size = new Vector2(colliderSizeOnInteraction, colliderSizeOnInteraction);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activationType.Equals(ActivationType.Position) && buttonType.Equals(ButtonType.PushButton))
        {
            _isPushed = !_isPushed;
            _buttonAnimator.SetBool("isPushed", _isPushed);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (activationType.Equals(ActivationType.Position) && !_isPushed)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _isPushed = !_isPushed;
        _buttonAnimator.SetBool("isPushed", _isPushed);
        foreach (var actionableObject in actionableObjects)
        {
            actionableObject.TriggerActionEvent();
        }
    }
}
