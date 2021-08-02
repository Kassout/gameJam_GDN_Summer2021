using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kill player.");
            PlayerController.Instance.TakeDamage();
        }
        Destroy(gameObject);
    }
}
