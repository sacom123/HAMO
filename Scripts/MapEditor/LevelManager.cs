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
    #region �����
    [Header("���� ��ũ��Ʈ")]
    public Tilemap Tm;
    public itemEditorManager IEM;
    public JsonSaveLoad JsonSaveLoadManager;
    [SerializeField] TileMapEditor TME;
    public MakeTexture MT;

    public MakeTexture MT1;
    public MakeTexture MT2;
    public GameObject ItemParent;

    [Header("���� Object")]
    public ItemlevelData ItemLevelDataTemp = new ItemlevelData();
    [SerializeField] BuildingobjectBase[] buildingobjectBase;           // Ÿ�ϸ� ���
    [SerializeField] ItemObjectBase[] itemObjectBase;                   // ������ ���
    [SerializeField] GameObject NOdataMapUI;                            // ���� �� ���� ���� ���

    [SerializeField] GameObject MakeTextureObj;                         // ����� ���� �ؽ�ó�� �׸��� ������Ʈ
    [SerializeField] GameObject MakeTextureObj1;                        // ����� ���� �ؽ�ó�� �׸��� ������Ʈ
    [SerializeField] GameObject MakeTextureObj2;                        // ����� ���� �ؽ�ó�� �׸��� ������Ʈ

    [SerializeField] GameObject NoItemLevelDataUI;                      // ������ ���� �����Ͱ� ���� ���
    [SerializeField] GameObject NoSavedLevelDataUi;

    public LevelData[] SaveLevelData;
    public ItemlevelData[] SaveItemLevelData;
    int MapSaveIndex = 0;
    int MapSaveCount = 0;



    [Header("���̽� ������ 3�� �̻��Ͻ� ��ü�ϴ� �˾�â")]         // ���� �� ���� �ҷ��ͼ� �����ϰų� �׸�
    [SerializeField] GameObject ChangePoPUPData;

    [Header("����� �� �ҷ����� �˾�â")]
    [SerializeField] GameObject SelectMapUI;                               // ��ü�� ���� ���� â

    [Header("���ξ����� �ѱ� �� ���� �˾�â")]
    [SerializeField] GameObject SelectMainMapUi;                        // ���� ���� �Ѱ��� ���� ���� â
    [SerializeField] MapEditorToMainScene METMS;

    [Header("Ÿ�� �� ī����")]
    public int TileMapCount = 80;
    [SerializeField] TextMeshProUGUI TileMapCountText;

    [Header("Ÿ�� �� ������ ķ")]
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

    #region Ÿ�� ī���� ������Ʈ
    public void TileTextUpdate()
    {
        TileMapCountText.text = TileMapCount.ToString();
    }
    #endregion

    #region Ÿ�� ī���� ����
    public void TileCountReset(int countMax)
    {
        TileMapCount = countMax;
        TileTextUpdate();
    }
    #endregion

    #region ���̽� ���� ���� üũ�Ͽ� �� ���� �˾�â ����
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

    #region �� ����
    public void SaveMap()
    {

        BoundsInt bounds = Tm.cellBounds;
        #region Ÿ�ϸ�
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

        #region ������
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


    #region �� �ҷ�����
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

    #region �� ��ü
    public void MapChange(int index, TileMapSelectButton TMSB)
    {
        BoundsInt bounds = Tm.cellBounds;

        #region Ÿ�ϸ�
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

        #region ������
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

    #region �������� �� ������
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

    #region �� ����
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


    #region �˾�â

    #region ���̽� ������ 3�� �̻��� �� �˾�â
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

    #region Load��ư Ŭ���� �� ���� �� ������ ���� ����
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


    #region Load ��ư �� ���� �˾�â
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


    #region �� ���� �˾�â �ݱ�
    void SelectMapSetActiveFalse()
    {
        SelectMapUI.SetActive(false);
        MakeTextureObj.SetActive(false);
    }
    #endregion

    #region �� ���� �˾�â �ݱ�
    void SelectMapDisalbe()
    {
        ChangePoPUPData.SetActive(false);
        MakeTextureObj.SetActive(false);
    }
    #endregion

    #region ���� �� ��ư�̶� ����
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

    #region ���� �� ���� �˾�â
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


   

    #region �� �����Ͱ� �������� ������ �˾�
    void NoDataMapPopUp()
    {
        NOdataMapUI.SetActive(true);
        Invoke("NoDataMapDisable", 3f);
    }
    #endregion

    #region �� �����Ͱ� �������� ������ �˾� â �ݱ�
    void NoDataMapDisable()
    {
        NOdataMapUI.SetActive(false);
    }
    #endregion


    #region ���� �����Ͱ� �������� ������ �˾� ����
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

// Ÿ�ϸ� ������Ʈ
[System.Serializable]
public class LevelData
{
    public List<int>InstanceId = new List<int>();
    public List<BuildingobjectBase> tiles = new List<BuildingobjectBase>();
    public List<Vector3Int> pos = new List<Vector3Int>();
}

// ������ ������Ʈ
[System.Serializable]
public class ItemlevelData
{
    public List<int>ItemInstanceID = new List<int>();
    public List<ItemObjectBase> ItemObj = new List<ItemObjectBase>();
    public List<Vector3> pos = new List<Vector3>();
}