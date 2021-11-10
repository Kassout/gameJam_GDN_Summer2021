using UnityEngine;

/// <summary>
/// Class <c>SpringBoxController</c> is a Unity component script used to manage the spring box trap behaviour.
/// </summary>
[ExecuteInEditMode]
public class SpringBoxController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>bouncingDirection</c> represents the bouncing direction type of the spring.
    /// </summary>
    [SerializeField]
    private BouncingDirection bouncingDirection;
    
    /// <summary>
    /// Instance variable <c>startingPoint</c> represents the 3D coordinate value of the starting point of the game object.
    /// </summary>
    private Vector3 _startingPoint;
    
    /// <summary>
    /// Instance variable <c>indexCurrentOrientation</c> represents the index value of the current spring box orientation.
    /// </summary>
    private int _indexCurrentOrientation;
    
    /// <summary>
    /// Instance variable <c>BouncingDirection</c> represents an enumeration of bouncing direction type for the spring object.
    /// </summary>
    private enum BouncingDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _indexCurrentOrientation = (int)bouncingDirection;
        _startingPoint = transform.position;
    }

#if UNITY_EDITOR
    /// <summary>
    /// This method is called when the script is loaded or a value is changed in the Inspector.
    /// </summary>
    private void OnValidate()
    {
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(false);

        _indexCurrentOrientation = (int)bouncingDirection;
        
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(true);
    }
#endif

    #endregion

    #region Public

    /// <summary>
    /// This method is called to execute a spring box rotation.
    /// </summary>
    public void Rotate(bool clockwise)
    {
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(false);
        if (clockwise) {
            if (_indexCurrentOrientation < transform.childCount - 1)
            {
                _indexCurrentOrientation++;
            }
            else
            {
                _indexCurrentOrientation = 0;
            }
        } else {
            if (_indexCurrentOrientation > 0)
            {
                _indexCurrentOrientation--;
            }
            else
            {
                _indexCurrentOrientation = transform.childCount - 1;
            }
        }
        transform.GetChild(_indexCurrentOrientation).gameObject.SetActive(true);
        bouncingDirection = (BouncingDirection)_indexCurrentOrientation;
    }

    /// <summary>
    /// This method is used to restart the spring box to the game beginning position.
    /// </summary>
    public void RestartPosition()
    {
        transform.position = _startingPoint;
    }

    #endregion
}
