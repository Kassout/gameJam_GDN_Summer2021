using UnityEngine;

/// <summary>
/// Class <c>CameraController</c> is a Unity component script used to manage the camera behaviour.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>cameraStyle</c> represents the camera style of the game.
    /// </summary>
    [SerializeField]
    private CameraStyle cameraStyle;

    /// <summary>
    /// Instance variable <c>target</c> represents the position of the target of the camera focus.
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// Instance variable <c>smoothSpeed</c> represents the smooth speed value of the camera movement.
    /// </summary>
    [SerializeField][Range(0.01f, 1f)]
    private float smoothSpeed = 0.125f;

    /// <summary>
    /// Instance variable <c>offset</c> represents the 3D coordinate value offset of the camera related to the target.
    /// </summary>
    [SerializeField] 
    private Vector3 offset;
    
    /// <summary>
    /// Instance variable <c>velocity</c> represents the velocity vector of the camera.
    /// </summary>
    private Vector3 _velocity = Vector3.zero;

    /// <summary>
    /// Instance enumeration <c>CameraStyle</c> represents the different possible camera style.
    /// </summary>
    private enum CameraStyle
    {
        Locked,
        Following,
        SmoothFollowing
    };
    
    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // raw following camera style
        if (cameraStyle.Equals(CameraStyle.Following))
        {
            transform.parent = target.transform;
        }
    }
    
    /// <summary>
    /// This method is called every fixed frame-rate frame.
    /// </summary>
    private void FixedUpdate()
    {
        // smooth following camera style
        if (cameraStyle.Equals(CameraStyle.SmoothFollowing))
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);
        }
    }
}