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
    /// TODO: comments
    /// </summary>
    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// TODO: comments
    /// </summary>
    void Update()
    {
        _sprite.sortingOrder = Mathf.RoundToInt((transform.position.y + offset) * 100f) * -1 + 1000;
    }
}
