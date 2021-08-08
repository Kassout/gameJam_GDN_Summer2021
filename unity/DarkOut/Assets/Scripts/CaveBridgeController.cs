
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveBridgeController : ReceiverObject
{
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private TilemapRenderer tilemapRenderer;
    
    protected override void OnReceiverTriggersActivated()
    {
        tilemap.enabled = true;
        tilemapRenderer.enabled = true;
    }

    protected override void OnReceiverTriggersDeactivated()
    {
        tilemap.enabled = false;
        tilemapRenderer.enabled = false;
    }
}
