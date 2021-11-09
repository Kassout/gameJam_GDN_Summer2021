using UnityEngine;

/// <summary>
/// TODO: comments
/// </summary>
public class SortOrderController : MonoBehaviour
{
    /// <summary>
    /// TODO: comments
    /// </summary>
    [SerializeField]
    private float offset;

    /// <summary>
    /// TODO: comments
    /// </summary>
    private SpriteRenderer _sprite;

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
}
