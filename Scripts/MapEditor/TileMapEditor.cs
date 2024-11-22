using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapEditor : MonoBehaviour
{
    #region 선언부
    [SerializeField] Tilemap Tm;            // 사용자가 Custom 할 맵
    BuildingobjectBase BB;                  // 사용자가 고른 Tile의 정보가 담겨 있는 ScriptableObject
    [SerializeField] Tilemap previewMap;

    [SerializeField] Camera Camera;         // 마우스의 위치를 계산하기 위한 Camera

    [SerializeField]LevelManager LM;

    [SerializeField] itemEditorManager IEM;

    Vector2 mousePos;
    Vector3Int CurrentGridPosition;
    Vector3Int lastGridPosition;
    public BuilderEditorButtonHandler[] BEBArray;
    public int BEBNumber;
    #endregion

    private void Awake()
    {
        Camera = Camera.main;
    }

    #region 구현부

    
    private void Update()
    {
        #region if Clicked Moust Button Place Tile In TileMap
        if (BB != null)
        {
            Vector3Int pos = Tm.WorldToCell(Camera.ScreenToWorldPoint(Input.mousePosition));            // 포지션을 담고 있는 란
            if (Input.GetMouseButton(0))
            {
                if(Tm.GetTile(pos) == null && !EventSystem.current.IsPointerOverGameObject() && LM.TileMapCount >0)
                { 
                    Placetile(pos);
                }
                else
                {
                    Debug.Log("이미 타일이 있거나 타일을 더이상 배치할 수 없습니다.");
                }
            }
            else if (Input.GetMouseButton(1))
            {
                DePlacetile(pos);
            }

            mousePos = Input.mousePosition;
            Vector3 pos2 = Camera.ScreenToWorldPoint(mousePos);
            Vector3Int girdPos = previewMap.WorldToCell(pos2);

            if (girdPos != CurrentGridPosition)
            {
                lastGridPosition = CurrentGridPosition;
                CurrentGridPosition = girdPos;
                UpdatePreivew();
            }
        }
        #endregion
    }

    #region AllClear in TileMap

    public void SetClear()
    {
        BB = null;
        previewMap.SetTile(lastGridPosition, null);
        previewMap.SetTile(CurrentGridPosition, null);
    }
    #endregion

    #region Preview Update(runtime)
    void UpdatePreivew()
    {
        if(BB != null)
        {
            previewMap.SetTile(lastGridPosition, null);

            previewMap.SetTile(CurrentGridPosition, BB.TileBase);
        }

    }
    #endregion



    #region SetBuild -> TB in BB
    public void SetBuild(BuildingobjectBase TB,int index)
    {
        IEM.ReSetButton();
        Debug.Log("Check");
        if (BEBNumber != index)
        {
            BEBArray[BEBNumber].GetComponent<BuilderEditorButtonHandler>().MOB.isClicked = false;
            BEBArray[BEBNumber].GetComponent<BuilderEditorButtonHandler>().MOB.SelectImage.SetActive(false);
            BEBArray[BEBNumber].GetComponent<BuilderEditorButtonHandler>().MOB.BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
            if (TB != null)
            {
                BB = TB;
                BEBNumber = index;
                BEBArray[index].GetComponent<BuilderEditorButtonHandler>().MOB.isClicked = true;
            }
        }
        else
        {
            if (TB != null)
            {
                BB = TB;
                BEBNumber = index;
                BEBArray[index].GetComponent<BuilderEditorButtonHandler>().MOB.isClicked = true;
            }
        }
    }
    #endregion


    #region PlaceTile in TileMap
    void Placetile(Vector3Int pos)
    {
        Tm.SetTile(pos, BB.TileBase);
        LM.TileMapCount--;
        LM.TileTextUpdate();
    }
    #endregion

    #region DePlaceTile in TileMap
    void DePlacetile(Vector3Int pos)
    {
        if(Tm.GetTile(pos) != null)
        {
            Debug.Log("타일이 있습니다.");
            LM.TileMapCount++;
            LM.TileTextUpdate();
            Tm.SetTile(pos, null);
        }
    }
    #endregion

    #region ResetTileMap
    public void ResetTileMap()
    {
        Tm.ClearAllTiles();
        SetClear();
        LM.TileCountReset(80);
    }
    #endregion

    public void ReSetTile()
    {
        BEBArray[BEBNumber].GetComponent<BuilderEditorButtonHandler>().MOB.isClicked = false;
        BEBArray[BEBNumber].GetComponent<BuilderEditorButtonHandler>().MOB.BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
        BEBArray[BEBNumber ].GetComponent<BuilderEditorButtonHandler>().MOB.SelectImage.SetActive(false);
        SetClear();
    }

    #endregion
}
