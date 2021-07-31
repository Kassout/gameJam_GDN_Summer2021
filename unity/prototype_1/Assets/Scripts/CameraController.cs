using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraStyle cameraStyle;

    [SerializeField]
    private Transform target;

    [SerializeField][Range(0.01f, 1f)]
    private float smoothSpeed = 0.125f;

    [SerializeField] 
    private Vector3 offset;
    
    private Vector3 velocity = Vector3.zero;

    private enum CameraStyle
    {
        Locked,
        Following,
        SmoothFollowing
    };
    
    void Awake()
    {
        if (cameraStyle.Equals(CameraStyle.Following))
        {
            transform.parent = target.transform;
        }
    }
    
    private void FixedUpdate()
    {
        if (cameraStyle.Equals(CameraStyle.SmoothFollowing))
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }
}
