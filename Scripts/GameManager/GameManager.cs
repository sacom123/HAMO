using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region TileMap Editor
    [Header("TileMap Editor")]
    public Tilemap[] TileMap;
    public Tilemap[] TileMap_hook;

    [Header("Draw TileMap Level")]
    [SerializeField] LevelData TileMap1LevelData = new LevelData();
    [Header("Draw Item Level")]
    [SerializeField] ItemlevelData itemlevelDatas = new ItemlevelData();

    [Header("BackGround Check")]
    CameraController CC;

    [Header("FadeIn/Out")]
    [SerializeField] Image Fade;
    public GameObject FadeObject;


    int? MapIndex = null;
    public bool TileMap1Check = false;
    #endregion

    #region Player State
    [Header("Player State")]
    public bool isPlayerDead = false;
    [SerializeField]
    GameObject PlayerObject;
    int PlayerHP;
    public GameObject player;
    CheckCameraPoint PlayerCam;
    public int cameraIndex;
    GameObject BackGround;
    public bool isClear = false;
    public int _MapCount = -1;
    #endregion

    #region Statge Check
    [Header("Stage Check")]
    [SerializeField] Transform[] SpawnPoint;
    [SerializeField] Collider2D[] StageCollider;
    [SerializeField] List<GameObject> MapCollider = new List<GameObject>();
    [SerializeField] Transform MapMaxGroup;
    MainMapManager MMM;
    public int MapEditorIndex;
    #endregion

    #region Time Line Event
    public PlayableDirector[] pd;
    public TimelineAsset[] ta;

    [SerializeField] bool StartTimeline = false;
    [SerializeField] bool FirstTimeLine = false;
    [SerializeField] bool secondTimeLine = false;
    [SerializeField] bool srdTimeLine = false;
    public int count = 0;
    #endregion

    #region Sound Manger
    public Slider _BGM;
    public Slider _SFX;
    #endregion

    [SerializeField] int MainGameSceneIndex;
    [SerializeField] int TitleSceneIndex;
    [SerializeField] int ClearSceneIndex;

    private void Awake()
    {
        #region GameManager signleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == MainGameSceneIndex && !StartTimeline)
        {
            pd[count].Play(ta[count]);
            count++;
            StartTimeline = true;
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (MapCollider.Count == 0)
        {
            SetCollider();
        }
        FadeObject = GameObject.FindGameObjectWithTag("FadeInOutManager");
    }



    #region 타임라인 이벤트
    public void StartTimeLine(Transform target)
    {
        target.gameObject.SetActive(false);

        if (count == 1 && !FirstTimeLine)
        {
            FirstTimeLine = true;
            pd[count].Play(ta[count]);
        }
        else if (count == 2 && !secondTimeLine)
        {
            secondTimeLine = true;
            pd[count].Play(ta[count]);
        }
        else if (count == 3 && !srdTimeLine)
        {
            srdTimeLine = true;
            pd[count].Play(ta[count]);
        }
        count++;
    }
    #endregion

    #region 그린 타일맵을 LD에 저장
    public void SetTileMap(LevelData LD, ItemlevelData item, int index)
    {
        TileMap1LevelData = LD;
        itemlevelDatas = item;
        TileMap1Check = true;
        MapIndex = index;
    }

    #endregion

    #region 그린 타일맵에 Collider 추가
    public void SetTileMapCollider(int index)
    {
        if (MapIndex != null)
        {
            if (TileMap[index].GetComponent<TilemapCollider2D>() == null)
            {
                TileMap[index].AddComponent<TilemapCollider2D>();
            }

            CompositeCollider2D compositeCollider = TileMap[index].GetComponent<CompositeCollider2D>();
            if (compositeCollider == null)
            {
                TileMap[index].AddComponent<CompositeCollider2D>();
                TileMap[index].AddComponent<Rigidbody2D>();
                Rigidbody2D Rb = TileMap[index].GetComponent<Rigidbody2D>();
                Rb.bodyType = RigidbodyType2D.Static;
            }

            if (TileMap_hook[index].GetComponent<TilemapCollider2D>() == null)
            {
                TileMap_hook[index].AddComponent<TilemapCollider2D>();
            }

            CompositeCollider2D compositeCollider2 = TileMap_hook[index].GetComponent<CompositeCollider2D>();
            if (compositeCollider2 == null)
            {
                TileMap_hook[index].AddComponent<CompositeCollider2D>();
                TileMap_hook[index].AddComponent<Rigidbody2D>();
                Rigidbody2D Rb = TileMap_hook[index].GetComponent<Rigidbody2D>();
                Rb.bodyType = RigidbodyType2D.Static;
            }

        }
        if (TileMap[index].GetComponent<TilemapCollider2D>() != null
            && TileMap[index].GetComponent<CompositeCollider2D>() != null
            && TileMap[index].GetComponent<Rigidbody2D>() != null)
        {
            TileMap[index].GetComponent<TilemapCollider2D>().usedByComposite = true;
            TileMap[index].GetComponent<CompositeCollider2D>().enabled = false;
            TileMap[index].GetComponent<CompositeCollider2D>().enabled = true;
        }

        if (TileMap_hook[index].GetComponent<TilemapCollider2D>() != null
            && TileMap_hook[index].GetComponent<CompositeCollider2D>() != null
            && TileMap_hook[index].GetComponent<Rigidbody2D>() != null)
        {
            TileMap_hook[index].GetComponent<TilemapCollider2D>().usedByComposite = true;
            TileMap_hook[index].GetComponent<CompositeCollider2D>().enabled = false;
            TileMap_hook[index].GetComponent<CompositeCollider2D>().enabled = true;
            TileMap_hook[index].GetComponent<CompositeCollider2D>().isTrigger = true;
        }
    }

    #endregion

    #region 게임 매니저가 특정 씬에서 실행될때를 검사하기 위함 (Main Game Scene)
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == MainGameSceneIndex)
        {
            for (int i = 0; i < GameObject.FindGameObjectWithTag("TimLineTable").transform.childCount; i++)
            {
                pd[i] = GameObject.FindGameObjectWithTag("TimLineTable").transform.GetChild(i).GetComponent<PlayableDirector>();
            }
            SoundObjinit();
            MapMovemontCollderChange();
        }


        if (scene.buildIndex == TitleSceneIndex)
        {
            Destroy(SoundManager.instance);
        }

        if(scene.buildIndex == ClearSceneIndex)
        {
            SoundManager.instance.BGMchange(3);
        }


        var PlayerObj = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObj != null)
        {
            PlayerObject = PlayerObj;
        }

      

        if (scene.buildIndex == MainGameSceneIndex && TileMap1Check == true)
        {
            FadeInOut.instance.FadeOutGoToMainScene();
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CheckCameraPoint>().count = (int)cameraIndex + 1;
            pd[1] = GameObject.FindGameObjectWithTag("PlayerDir").GetComponent<PlayableDirector>();

            GameObject.FindGameObjectWithTag("MainMapManager").GetComponent<MainMapManager>().setTileMap();
            GameObject.FindGameObjectWithTag("MainMapManager").GetComponent<MainMapManager>().setStageCollider();
            CC = GameObject.FindGameObjectWithTag("MainVcam").GetComponent<CameraController>();
            MapMaxGroup = GameObject.FindGameObjectWithTag("MainMapManager").transform;

            SetCollider();


            player.transform.position = SpawnPoint[MapEditorIndex].position;
            CC.BackGround.SetActive(true);
            MapCollider[MapEditorIndex].SetActive(true);

            for (int j = 0; j < TileMap1LevelData.pos.Count; j++)
            {
                // Hook 타입이라면.
                if (TileMap1LevelData.tiles[j].categoryType == Category.Hook)
                {
                    TileMap_hook[MapEditorIndex].SetTile(TileMap1LevelData.pos[j], TileMap1LevelData.tiles[j].TileBase);
                }
                // Floor 타입이라면.
                else
                {
                    TileMap[MapEditorIndex].SetTile(TileMap1LevelData.pos[j], TileMap1LevelData.tiles[j].TileBase);
                }
            }

            for (int i = 0; i < itemlevelDatas.pos.Count; i++)
            {
                Instantiate(itemlevelDatas.ItemObj[i].GetItemBase, itemlevelDatas.pos[i], Quaternion.identity);
            }
            SetTileMapCollider(MapEditorIndex);
            TileMap1Check = false;
            StageCollider[MapEditorIndex].gameObject.SetActive(false);

        }

        else if (scene.name == "MapEditorScene")
        {
            FadeInOut.instance.fade.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().Mapindex = MapEditorIndex;
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().ItemCountSet();
            SoundObjinit();
        }
        Soundinit();
    }
    #endregion

    #region 이전 스테이지의 출입을 막기 위한 collider
    void SetCollider()
    {
        MapCollider.Clear();
        for (int i = 0; i < MapMaxGroup.childCount; i++)
        {
            MapCollider.Add(MapMaxGroup.GetChild(i).gameObject);
        }
    }
    #endregion

    #region 플레이어가 죽고 타임라인 초기화
    public void TimeLineReSet()
    {
        StartTimeline = false;
        FirstTimeLine = false;
        secondTimeLine = false;
    }
    #endregion


    #region 사운드 추가 이벤트

    void SoundObjinit()
    {
        _BGM = GameObject.FindWithTag("UiCanvers").transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>();
        _SFX = GameObject.FindWithTag("UiCanvers").transform.GetChild(1).transform.GetChild(2).GetComponent<Slider>();

    }
    void Soundinit()
    {
        SoundManager.instance.SetSoundSlider(_BGM,_SFX);
    }
    #endregion

    void MapMovemontCollderChange()
    {
        Transform[] _CollderArr = new Transform[5];  
        for (int i = 0; i< _MapCount;i++)
        {
            _CollderArr[i] = GameObject.FindWithTag("CollderArr").transform.GetChild(i).transform;
            _CollderArr[i].GetComponent<BoxCollider2D>().isTrigger = false;
        }
    } 

    // MapEditor 씬으로 씬 전환을 하기 위한 함수
    public void GoToScene()
    {
        FadeInOut.instance.fade.gameObject.SetActive(true);
        FadeInOut.instance.FadeInGoToMapEditorScene();
    }
}
