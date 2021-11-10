using UnityEngine;

/// <summary>
/// Class <c>SortOrderController</c> is a Unity component script used to manage the sorting layers behaviour of sprites to display.
/// </summary>
public class SortOrderController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance variable <c>offset</c> represents the float offset value of the sorting layer of the associated game object.
    /// </summary>
    [SerializeField]
    private float offset;

    /// <summary>
    /// Instance variable <c>_sprite</c> represents the sprite renderer of the game object.
    /// </summary>
    private SpriteRenderer _sprite;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// This method is called once when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
        _sprite.sortingOrder = Mathf.RoundToInt((transform.position.y + offset) * 100f) * -1 + 1000;
    }

    #endregion
}
