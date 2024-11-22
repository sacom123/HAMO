using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemEditorManager : MonoBehaviour
{
    public ItemController[] ItemButtons;                        // 버튼들
    public ItemObjectBase[] ItemPrefabs;                        // 함정 프리팹
    public int CurrentButtonPressed;                            // 현재 눌린 버튼 -> 아이템 타입
    [Tooltip("아이템을 생성할 전체 갯수")] public int itemcount;
    public int SpawnCount = 0;                                  // 현재 생성된 아이템 갯수
    public TextMeshProUGUI ItemCountText;                       // 생성된 아이템 갯수 텍스트
    [SerializeField] LevelManager LM;                           // 레벨 매니저


    public List<int> ItemID = new List<int>();
    public List<ItemObjectBase> SpawnItem = new List<ItemObjectBase>();
    public List<Vector3> SpawnItempos = new List<Vector3>();

    public List<GameObject> SpawnGameObject = new List<GameObject>();

    
    [SerializeField] float clickThreshold = 0.5f;


    void Start()
    {
        ItemCountText.text = itemcount.ToString();
        
    }
    

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 wordlpos = Camera.main.ScreenToWorldPoint(screenPos);
        // 아이템 생성
        if(Input.GetMouseButtonDown(0) && ItemButtons[CurrentButtonPressed].Cliked && !EventSystem.current.IsPointerOverGameObject() && itemcount > 0)
        {
            Debug.Log("아이템 생성");
            ItemButtons[CurrentButtonPressed].Cliked = false;
            ItemButtons[CurrentButtonPressed].MOB.BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
            GameObject IteminArray;
            IteminArray = Instantiate(ItemPrefabs[CurrentButtonPressed].GetItemBase,new Vector3(wordlpos.x,wordlpos.y,0),Quaternion.identity);
            IteminArray.transform.parent = LM.ItemParent.transform;

            SpawnGameObject.Add(IteminArray);
            ItemID.Add(ItemButtons[CurrentButtonPressed].ID);
            SpawnItem.Add(ItemPrefabs[CurrentButtonPressed]);
            SpawnItempos.Add(new Vector3(IteminArray.transform.position.x, IteminArray.transform.position.y, 0));
            

            SpawnCount++;
            itemcount--;
            ItemCountText.text = itemcount.ToString();

            ItemButtons[CurrentButtonPressed].MOB.isClicked = false;
            ItemButtons[CurrentButtonPressed].MOB.SelectImage.SetActive(false);
            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));

        }
        //아이템 삭제
        else if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 클릭한 위치와 가장 가까운 오브젝트 찾기
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
            Debug.Log("hitcollider name : " + hitCollider.name);
            // 오브젝트가 있고
            if (hitCollider != null)
            {
                
                // 클릭한 오브젝트와 마우스 클릭 위치 사이의 거리 측정
                float distance = Vector2.Distance(hitCollider.transform.position, mousePosition);
                Debug.Log("거리: " + distance);
                Debug.Log("마우스 포지션 : " +mousePosition);
                Debug.Log("hitcollider 포지션 : " + hitCollider.transform.position);
                Debug.Log("디스턴스 : " + distance);
                // 일정 거리 이내에 있는 경우 클릭 처리
                if (distance < clickThreshold)
                {
                    // 클릭한 오브젝트에 대한 처리 수행
                    Debug.Log("클릭한 오브젝트: " + hitCollider.gameObject.name);
                    int index = (SpawnItempos.Count)-1;

                    for(int i = SpawnItempos.Count; i > 0; i--)
                    {
                        Debug.Log(index);
                        float x = (SpawnItempos[i - 1].y - hitCollider.transform.position.y);
                        if (x <= distance)
                        {
                            Debug.Log("2" + index);
                            // 얻은 index를 통해서 배열에 있는 아이템 삭제, 그 아이템 스크립트에 아이템 갯수 증가
                            // SpawnCount 감소
                            itemcount++;
                            ItemCountText.text = itemcount.ToString();

                            ItemID.RemoveAt(index);
                            SpawnItem.RemoveAt(index);
                            SpawnItempos.RemoveAt(index);
                            Destroy(hitCollider.gameObject);
                            SpawnCount--;
 
                            break;
                        }
                        index--;
                    }
                    

                }
            }
        }
    }

    public void ReSetItem(int itemcount2)
    {
        foreach(GameObject item in SpawnGameObject)
        {
            Destroy(item);
        }
        SpawnGameObject.Clear();
        ItemID.Clear();
        SpawnItem.Clear();
        SpawnItempos.Clear();
        itemcount = itemcount2;
        SpawnCount = 0;
        ItemCountText.text = itemcount.ToString();
    }

    public void ReSetButton()
    {
        ItemButtons[CurrentButtonPressed].ButtonReSet();
    }
}
