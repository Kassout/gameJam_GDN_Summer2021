using UnityEngine;

/// <summary>
/// Class <c>RotationPlatformController</c> is a Unity component script used to manage the spring box trap behaviour.
/// </summary>
[ExecuteInEditMode]
public class SpringBoxController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    [SerializeField]
    private BouncingDirection bouncingDirection;

    /// <summary>
    /// Instance variable <c>indexCurrentOrientation</c> represents the index value of the current spring box orientation.
    /// </summary>
    private int _indexCurrentOrientation;
    
    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    private enum BouncingDirection : int
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    /// <summary>
    /// This method is called when the script is loaded or a value is changed in the Inspector.
    /// </summary>
    void OnValidate()
    {
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(false);

        _indexCurrentOrientation = (int)bouncingDirection;
        
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(true);
    }

    /// <summary>
    /// This method is called to execute a spring box rotation.
    /// </summary>
    public void Rotate()
    {
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(false);
        if (_indexCurrentOrientation < transform.childCount - 1)
        {
            _indexCurrentOrientation++;
        }
        else
        {
            _indexCurrentOrientation = 0;
        }
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(true);
    }
}
