using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoxController : MonoBehaviour
{
    /// <summary>
    /// Instance variable <c>groundTileMap</c> represents the ground tile map on which player will walk on.
    /// </summary>
    [SerializeField]
    private Tilemap groundTileMap;

    /// <summary>
    /// Instance variable <c>collisionTileMap</c> represents the tile map containing the different obstacles the player could collide with.
    /// </summary>
    [SerializeField]
    private Tilemap collisionTileMap;
    
    /// <summary>
    /// Instance variable <c>pitfallTileMap</c> represents the tile map containing the different pit tiles the player could fall into.
    /// </summary>
    [SerializeField]
    private Tilemap pitfallTileMap;

    Rigidbody2D rb2d;

    Vector2 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartPosition = rb2d.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        CheckMoveCollision();
    }

    private void CheckMoveCollision() {
        Vector3Int gridPosition = groundTileMap.WorldToCell(rb2d.position);
        if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition) || pitfallTileMap.HasTile(gridPosition))
        {
            Kill();
        }
    }

    private void Kill() {
        rb2d.velocity = Vector2.zero;
        rb2d.position = StartPosition;
    }
}
