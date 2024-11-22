using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public Camera tileMapCamera;

    private void Start()
    {
        tileMapCamera.clearFlags = CameraClearFlags.Color;
        tileMapCamera.cullingMask = 1 << LayerMask.NameToLayer("TileMap");
    }
}
