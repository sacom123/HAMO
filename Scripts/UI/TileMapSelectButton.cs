using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TileMapSelectButton : MonoBehaviour
{
    [SerializeField]int tileMapIndex;
    public LevelData Level;
    public ItemlevelData Item;
    [SerializeField] LevelManager LM;

    selectMapMouse SMM;

    private void Start()
    {
        SMM = GetComponent<selectMapMouse>();
        SMM.OnMouse = false;
        SMM.OnMouseAction = false;
        SMM.SelectObject.SetActive(false);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void SelecetTileMap()
    {
        SMM.OnMouse = false;
        SMM.OnMouseAction = false;
        LM.MapChange(tileMapIndex,this);
    }

    public void LoadMap()
    {
        LM.LoadMap(tileMapIndex);
    }

    public void MainMap()
    {
        LM.MainMap(tileMapIndex);
    }
}
