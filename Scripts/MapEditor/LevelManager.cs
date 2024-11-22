using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class LevelManager : MonoBehaviour
{
    #region 선언부
    [Header("각종 스크립트")]
    public Tilemap Tm;
    public itemEditorManager IEM;
    public JsonSaveLoad JsonSaveLoadManager;
    [SerializeField] TileMapEditor TME;
    public MakeTexture MT;

    public MakeTexture MT1;
    public MakeTexture MT2;
    public GameObject ItemParent;

    [Header("각종 Object")]
    public ItemlevelData ItemLevelDataTemp = new ItemlevelData();
    [SerializeField] BuildingobjectBase[] buildingobjectBase;           // 타일맵 재료
    [SerializeField] ItemObjectBase[] itemObjectBase;                   // 아이템 재료
    [SerializeField] GameObject NOdataMapUI;                            // 저장 된 맵이 없는 경우

    [SerializeField] GameObject MakeTextureObj;                         // 저장된 맵의 텍스처를 그리는 오브젝트
    [SerializeField] GameObject MakeTextureObj1;                        // 저장된 맵의 텍스처를 그리는 오브젝트
    [SerializeField] GameObject MakeTextureObj2;                        // 저장된 맵의 텍스처를 그리는 오브젝트

    [SerializeField] GameObject NoItemLevelDataUI;                      // 아이템 레벨 데이터가 없는 경우
    [SerializeField] GameObject NoSavedLevelDataUi;

    public LevelData[] SaveLevelData;
    public ItemlevelData[] SaveItemLevelData;
    int MapSaveIndex = 0;
    int MapSaveCount = 0;



    [Header("제이슨 파일이 3개 이상일시 교체하는 팝업창")]         // 저장 된 맵을 불러와서 수정하거나 그림
    [SerializeField] GameObject ChangePoPUPData;

    [Header("저장된 맵 불러오는 팝업창")]
    [SerializeField] GameObject SelectMapUI;                               // 교체할 맵을 고르는 창

    [Header("메인씬으로 넘길 맵 고르는 팝업창")]
    [SerializeField] GameObject SelectMainMapUi;                        // 메인 씬에 넘겨줄 맵을 고르는 창
    [SerializeField] MapEditorToMainScene METMS;

    [Header("타일 맵 카운터")]
    public int TileMapCount = 80;
    [SerializeField] TextMeshProUGUI TileMapCountText;

    [Header("타일 맵 에디터 캠")]
    [SerializeField] Cinemachine.CinemachineVirtualCamera[] MapEditorViewVcam;

    public int Mapindex = 0;

    #endregion

    private void Awake()
    {
        IEM = GameObject.FindGameObjectWithTag("ItemLeveleditor").GetComponent<itemEditorManager>();
    }
    private void Start()
    {
        MapSaveCount = 0;
        FadeInOut.instance.FadeOutGoToMainScene();
        MapSaveIndex = 0;
        TileCountReset(TileMapCount);
        
        

        if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) > 0)
        {
            SaveLevelData = new LevelData[3];
            SaveItemLevelData = new ItemlevelData[3];

            for (int i = 0; i < SaveLevelData.Length; i++)
            {
                SaveLevelData[i] = JsonSaveLoadManager.TileLoadToJson("Tile", i, GameManager.instance.MapEditorIndex);
                SaveItemLevelData[i] = JsonSaveLoadManager.ItemLoadToJson("Item",i, GameManager.instance.MapEditorIndex);
            }

        }
        else if(JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) <= 0)
        {
            SaveLevelData = new LevelData[3];
            SaveItemLevelData = new ItemlevelData[3];
        }

        if(GameManager.instance.MapEditorIndex == 0)
        {
            MapEditorViewVcam[0].gameObject.SetActive(true);
            MapEditorViewVcam[1].gameObject.SetActive(false);
            MapEditorViewVcam[0].m_Priority = 10;
        }
        else if(GameManager.instance.MapEditorIndex == 1)
        {
            MapEditorViewVcam[0].gameObject.SetActive(false);
            MapEditorViewVcam[1].gameObject.SetActive(true);

            MapEditorViewVcam[1].m_Priority = 10;
        }

        if(GameManager.instance.MapEditorIndex == 0)
        {
            MakeTextureObj = MakeTextureObj1;
            MT = MT1;
        }
        else if(GameManager.instance.MapEditorIndex == 1)
        {
            MakeTextureObj = MakeTextureObj2;
            MT = MT2;
        }
    }

    #region 타일 카운터 업데이트
    public void TileTextUpdate()
    {
        TileMapCountText.text = TileMapCount.ToString();
    }
    #endregion

    #region 타일 카운터 리셋
    public void TileCountReset(int countMax)
    {
        TileMapCount = countMax;
        TileTextUpdate();
    }
    #endregion

    #region 제이슨 파일 수를 체크하여 맵 선택 팝업창 띄우기
    public void SaveMapButton()
    {
        BoundsInt? bounds = Tm.cellBounds;
        if(Mapindex == 0)
        {
            if (IEM.SpawnCount < 10 || bounds == null)
            {
                NoItemLevelDataUI.SetActive(true);
                Invoke("NoItemLevelDataUIOff", 2f);
            }
            else
            {
                TME.SetClear();
                Debug.Log(JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex));

                if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) == 0)
                {
                    if (MapSaveCount < 3)
                    {
                        SaveMap();
                        MapSaveCount++;
                    }
                    else
                    {
                        ChangeJsonPOPUP(SaveLevelData.Length);
                    }
                }
                else
                {
                    if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) >= 3)
                    {
                        ChangeJsonPOPUP(SaveLevelData.Length);
                    }
                    else if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) < 3)
                    {
                        SaveMap();
                        MapSaveCount++;
                    }
                }
            }
        }
        else
        {
            if (IEM.SpawnCount < 5 || bounds == null)
            {
                NoItemLevelDataUI.SetActive(true);
                Invoke("NoItemLevelDataUIOff", 2f);
            }
            else
            {
                TME.SetClear();
                Debug.Log(JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex));

                if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) == 0)
                {
                    if (MapSaveCount < 3)
                    {
                        SaveMap();
                        MapSaveCount++;
                    }
                    else
                    {
                        ChangeJsonPOPUP(SaveLevelData.Length);
                    }
                }
                else
                {
                    if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) >= 3)
                    {
                        ChangeJsonPOPUP(SaveLevelData.Length);
                    }
                    else if (JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) < 3)
                    {
                        SaveMap();
                        MapSaveCount++;
                    }
                }
            }
        }
        
        
    }
    #endregion

    #region 맵 저장
    public void SaveMap()
    {

        BoundsInt bounds = Tm.cellBounds;
        #region 타일맵
        LevelData leveldatas = new LevelData();
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    TileBase Temp = Tm.GetTile(new Vector3Int(x, y, 0));
                    if (Temp != null)
                    {
                        for (int z = 0; z < buildingobjectBase.Length; z++)
                        {

                            if (buildingobjectBase[z].TileBase == Temp)
                            {
                                leveldatas.InstanceId.Add(buildingobjectBase[z].InstanceID);
                                leveldatas.tiles.Add(buildingobjectBase[z]);
                                leveldatas.pos.Add(new Vector3Int(x, y, 0));
                            }
                        }
                    }
                }
            }
            #endregion

        #region 아이템
            ItemlevelData itemlevelData = new ItemlevelData();
            for (int z = 0; z < IEM.SpawnItem.Count; z++)
            {
                itemlevelData.ItemInstanceID.Add(IEM.SpawnItem[z].GetInstanceItemID);
                itemlevelData.ItemObj.Add(IEM.SpawnItem[z]);
                itemlevelData.pos.Add(IEM.SpawnItempos[z]);
            }
        #endregion

        if(Mapindex == 0)
        {
            IEM.ReSetItem(10);
        }
        else
        {
            IEM.ReSetItem(5);
        }
        SaveLevelData[MapSaveIndex] = leveldatas;
        SaveItemLevelData[MapSaveIndex] = itemlevelData;
        MapSaveIndex++;
        TME.SetClear();
        Tm.ClearAllTiles();
        TileCountReset(80);
    }
    #endregion


    #region 맵 불러오기
    public void LoadMap(int index)
    {
        TileCountReset(80);
        Tm.ClearAllTiles();
        if(Mapindex == 0)
        {
            IEM.ReSetItem(10);
        }
        else
        {
            IEM.ReSetItem(5);
        }
        

        for(int i=0;i< SaveLevelData[index].pos.Count;i++)
        {
            Tm.SetTile(SaveLevelData[index].pos[i], SaveLevelData[index].tiles[i].TileBase);
            TileMapCount--;
        }
        for(int i=0; i < SaveItemLevelData[index].pos.Count; i++)
        {
            for(int j=0;j<itemObjectBase.Length;j++)
            {
                if (SaveItemLevelData[index].ItemInstanceID[i] == itemObjectBase[j].GetInstanceItemID)
                {
                    SaveItemLevelData[index].ItemObj[i] = itemObjectBase[j];
                    GameObject spawn = Instantiate(SaveItemLevelData[index].ItemObj[i].GetItemBase, SaveItemLevelData[index].pos[i], Quaternion.identity, ItemParent.transform);
                    IEM.SpawnItem.Add(SaveItemLevelData[index].ItemObj[i]);
                    IEM.ItemID.Add(SaveItemLevelData[index].ItemInstanceID[i]);
                    IEM.SpawnGameObject.Add(spawn);
                    IEM.SpawnItempos.Add(SaveItemLevelData[index].pos[i]);
                    IEM.SpawnCount++;
                    IEM.itemcount--;
                }
            }
        }
        TileMapCountText.text = TileMapCount.ToString();

        SelectMapSetActiveFalse();

    }
    #endregion

    #region 맵 교체
    public void MapChange(int index, TileMapSelectButton TMSB)
    {
        BoundsInt bounds = Tm.cellBounds;

        #region 타일맵
        LevelData leveldatas = new LevelData();
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase Temp = Tm.GetTile(new Vector3Int(x, y, 0));

                if (Temp != null)
                {
                    for (int z = 0; z < buildingobjectBase.Length; z++)
                    {

                        if (buildingobjectBase[z].TileBase == Temp)
                        {
                            leveldatas.InstanceId.Add(buildingobjectBase[z].InstanceID);
                            leveldatas.tiles.Add(buildingobjectBase[z]);
                            leveldatas.pos.Add(new Vector3Int(x, y, 0));
                        }
                    }
                }
            }
        }
        #endregion

        #region 아이템
        ItemlevelData itemlevelData = new ItemlevelData();
        for (int z = 0; z < IEM.SpawnItem.Count; z++)
        {
            itemlevelData.ItemInstanceID.Add(IEM.SpawnItem[z].GetInstanceItemID);
            itemlevelData.ItemObj.Add(IEM.SpawnItem[z]);
            itemlevelData.pos.Add(IEM.SpawnItempos[z]);
        }
        #endregion

        TMSB.Level = leveldatas;
        TMSB.Item = itemlevelData;

        SaveLevelData[index] = leveldatas;
        SaveItemLevelData[index] = itemlevelData;

        TME.SetClear();
        Tm.ClearAllTiles();
        TileCountReset(80);
        if(Mapindex == 0)
        {
            IEM.ReSetItem(10);
        }
        else
        {
            IEM.ReSetItem(5);
        }
        SelectMapDisalbe();
    }
    #endregion

    #region 매인으로 맵 보내기
    public void MainMap(int index)
    {
        
        for (int i = 0; i < SaveLevelData[index].pos.Count; i++)
        {
            for (int j = 0; j < buildingobjectBase.Length; j++)
            {
                if (SaveLevelData[index].InstanceId[i] == buildingobjectBase[j].InstanceID)
                {
                    SaveLevelData[index].tiles[i] = buildingobjectBase[j];
                }
            }
        }
        for(int i=0;i< SaveItemLevelData[index].pos.Count;i++)
        {
            for(int j=0;j<itemObjectBase.Length;j++)
            {
                if (SaveItemLevelData[index].ItemInstanceID[i] == itemObjectBase[j].GetInstanceItemID)
                {
                    SaveItemLevelData[index].ItemObj[i] = itemObjectBase[j];
                }
            }
        }
        for (int i = 0; i < SaveLevelData.Length; i++)
        {
            if(SaveLevelData[i] != null)
            {
                JsonSaveLoadManager.ChangeJson(i, GameManager.instance.MapEditorIndex, SaveLevelData[i], SaveItemLevelData[i]);
            }
        }
        
        GameManager.instance.SetTileMap(SaveLevelData[index], SaveItemLevelData[index], Mapindex++);

        FadeInOut.instance.fade.gameObject.SetActive(true);
        FadeInOut.instance.FadeInGoToMainScene(METMS);
    }
    #endregion

    #region 맵 리셋
    public void MapReset()
    {
        Tm.ClearAllTiles();
        TME.SetClear();
        TileCountReset(80);
        if(Mapindex == 0)
        {
            IEM.ReSetItem(10);
        }
        else
        {
            IEM.ReSetItem(5);
        }
    }
    #endregion


    #region 팝업창

    #region 제이슨 파일이 3개 이상일 때 팝업창
    void ChangeJsonPOPUP(int check2)
    {
        ChangePoPUPData.SetActive(true);
        MakeTextureObj.SetActive(true);
        MT.LD = new LevelData[SaveLevelData.Length];

        for (int i = 0; i < check2; i++)
        {
            if (SaveLevelData[i] != null)
            {
                MT.LD[i] = SaveLevelData[i];
                for (int j = 0; j < MT.LD[i].pos.Count; j++)
                {
                    for (int z = 0; z < buildingobjectBase.Length; z++)
                    {
                        if (MT.LD[i].InstanceId[j] == buildingobjectBase[z].InstanceID)
                        {
                            MT.LD[i].tiles[j] = buildingobjectBase[z];
                        }
                    }
                }
            }
        }

        MT.ChangeTextureSelect();
    }
    #endregion

    #region Load버튼 클릭시 맵 저장 된 갯수에 따라 선택
    public void LoadMapButton()
    {
        if (SaveLevelData[0] != null)
        {
            SelecetMapPopUp(SaveLevelData.Length);
        }
        else
        {
            NoSavedLevelDataUi.SetActive(true);
            Invoke("DisableNoSavedLevelData", 3f);
        }
    }
    #endregion

    public void DisableNoSavedLevelData()
    {
        NoSavedLevelDataUi.SetActive(false);
    }


    #region Load 버튼 맵 선택 팝업창
    public void SelecetMapPopUp(int check2)
    {
        SelectMapUI.SetActive(true);
        MakeTextureObj.SetActive(true);
        MT.LD = new LevelData[SaveLevelData.Length];
        for (int i = 0; i < check2; i++)
        {
            if (SaveLevelData[i] != null)
            {
                MT.LD[i] = SaveLevelData[i];
                for (int j = 0; j < MT.LD[i].pos.Count; j++)
                {
                    for (int z = 0; z < buildingobjectBase.Length; z++)
                    {
                        if (MT.LD[i].InstanceId[j] == buildingobjectBase[z].InstanceID)
                        {
                            MT.LD[i].tiles[j] = buildingobjectBase[z];
                        }
                    }
                }
            }
        }
        MT.LoadTextureSelect();
    }
    #endregion


    #region 맵 저장 팝업창 닫기
    void SelectMapSetActiveFalse()
    {
        SelectMapUI.SetActive(false);
        MakeTextureObj.SetActive(false);
    }
    #endregion

    #region 맵 선택 팝업창 닫기
    void SelectMapDisalbe()
    {
        ChangePoPUPData.SetActive(false);
        MakeTextureObj.SetActive(false);
    }
    #endregion

    #region 메인 맵 버튼이랑 연결
    public void GoToMainScene()
    {
        if(MapSaveCount == 0 && JsonSaveLoadManager.CheckJsonFile(GameManager.instance.MapEditorIndex) == 0)
        {
            NoDataMapPopUp();
        }
        else
        {
            SelectMainMapPopUp(SaveLevelData.Length);
        }
    }
    #endregion

    #region 메인 맵 선택 팝업창
    void SelectMainMapPopUp(int check2)
    {
        SelectMainMapUi.SetActive(true);
        MakeTextureObj.SetActive(true);
        MT.LD = new LevelData[SaveLevelData.Length];
        for (int i = 0; i < check2; i++)
        {
            if (SaveLevelData[i] != null)
            {
                MT.LD[i] = SaveLevelData[i];
                for (int j = 0; j < MT.LD[i].pos.Count; j++)
                {
                    for (int z = 0; z < buildingobjectBase.Length; z++)
                    {
                        if (MT.LD[i].InstanceId[j] == buildingobjectBase[z].InstanceID)
                        {
                            MT.LD[i].tiles[j] = buildingobjectBase[z];
                        }
                    }
                }
            }
        }
        MT.MainMapTextureSelect();
    }
    #endregion


   

    #region 맵 데이터가 존재하지 않을때 팝업
    void NoDataMapPopUp()
    {
        NOdataMapUI.SetActive(true);
        Invoke("NoDataMapDisable", 3f);
    }
    #endregion

    #region 맵 데이터가 존재하지 않을때 팝업 창 닫기
    void NoDataMapDisable()
    {
        NOdataMapUI.SetActive(false);
    }
    #endregion


    #region 함정 데이터가 존재하지 않을때 팝업 끄기
    void NoItemLevelDataUIOff()
    {
        NoItemLevelDataUI.SetActive(false);
    }
    #endregion

    #endregion

    public void ItemCountSet()
    {

        if (Mapindex == 0)
        {
            IEM.itemcount = 10;
        }
        else
        {
            IEM.itemcount = 5;
        }
    }
}

// 타일맵 오브젝트
[System.Serializable]
public class LevelData
{
    public List<int>InstanceId = new List<int>();
    public List<BuildingobjectBase> tiles = new List<BuildingobjectBase>();
    public List<Vector3Int> pos = new List<Vector3Int>();
}

// 아이템 오브젝트
[System.Serializable]
public class ItemlevelData
{
    public List<int>ItemInstanceID = new List<int>();
    public List<ItemObjectBase> ItemObj = new List<ItemObjectBase>();
    public List<Vector3> pos = new List<Vector3>();
}