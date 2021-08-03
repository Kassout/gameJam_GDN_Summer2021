using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    private Vector2 direction;

    [SerializeField]
    private float forceAmplitude;
    
    [SerializeField]
    private BouncingDirection bouncingDirection;

    private float timeToWait = 1.0f;

    private enum BouncingDirection
    {
        Up,
        Bottom,
        Left,
        Right
    }

    private void Awake()
    {
        switch (bouncingDirection)
        {
            case BouncingDirection.Up:
                direction = Vector2.up;
                break;
            case BouncingDirection.Bottom:
                direction = Vector2.down;
                break;
            case BouncingDirection.Left:
                direction = Vector2.left;
                break;
            case BouncingDirection.Right:
                direction = Vector2.right;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);

        PlayerController.Instance.DisableInputs();
        other.rigidbody.AddForce(direction * forceAmplitude);
        while (other.rigidbody.velocity != Vector2.zero)
        {
            // DO NOTHING
        }
        PlayerController.Instance.EnableInputs();
    }

    public IEnumerator GetBounce(GameObject gameObject)
    {
        float timeLeft = 0.0f;
        
        if (gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.DisableInputs();
        }

        while (timeLeft <= timeToWait)
        {
            gameObject.transform.Translate(direction * forceAmplitude * Time.deltaTime);
            timeLeft += Time.deltaTime;
        }
        
        yield return null;
        
        if (gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.EnableInputs();
        }
    }
}