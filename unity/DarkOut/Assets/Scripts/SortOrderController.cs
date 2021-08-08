using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderController : MonoBehaviour
{
    [SerializeField]
    private float offset;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = Mathf.RoundToInt((transform.position.y + offset) * 100f) * -1 + 1000;
    }
}
