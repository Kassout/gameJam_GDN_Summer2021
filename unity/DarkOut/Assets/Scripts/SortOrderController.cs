using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderController : MonoBehaviour
{
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
}
