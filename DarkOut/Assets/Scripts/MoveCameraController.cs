using UnityEngine;

/// <summary>
/// Class <c>MoveCameraController</c> is a Unity component script used to manage the camera movement behaviour.
/// </summary>
public class MoveCameraController : MonoBehaviour
{
    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        transform.localPosition = new Vector3(Input.GetAxis("ArrowHorizontal") * 3.0f, Input.GetAxis("ArrowVertical") * 3.0f, 0);
    }
}
