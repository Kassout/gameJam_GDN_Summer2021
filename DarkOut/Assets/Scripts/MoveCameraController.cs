using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class MoveCameraController : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    void Update()
    {
        transform.localPosition = new Vector3(Input.GetAxis("ArrowHorizontal") * 3.0f, Input.GetAxis("ArrowVertical") * 3.0f, 0);
    }
}
