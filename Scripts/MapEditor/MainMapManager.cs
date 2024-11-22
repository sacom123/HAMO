using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainMapManager : MonoBehaviour
{
    [SerializeField] Tilemap[] tilemaps;
    [SerializeField] Tilemap[] tilemaps_hook;

    [SerializeField] Collider2D[] stagecollider;


    public void setTileMap()
    {
        GameManager.instance.TileMap = tilemaps;
        GameManager.instance.TileMap_hook = tilemaps_hook;
    }

    public void setStageCollider()
    {
        stagecollider[GameManager.instance.MapEditorIndex].gameObject.SetActive(true);
    }
}
