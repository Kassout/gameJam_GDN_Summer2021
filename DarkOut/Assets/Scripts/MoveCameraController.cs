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
        Vector3 cameraMovement = new Vector3(InputHandler.lookAround.x * 3.0f, InputHandler.lookAround.y * 3.0f, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraMovement, Time.deltaTime * 5.0f);
    }
}
