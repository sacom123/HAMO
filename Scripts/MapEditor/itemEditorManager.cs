using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemEditorManager : MonoBehaviour
{
    public ItemController[] ItemButtons;                        // ��ư��
    public ItemObjectBase[] ItemPrefabs;                        // ���� ������
    public int CurrentButtonPressed;                            // ���� ���� ��ư -> ������ Ÿ��
    [Tooltip("�������� ������ ��ü ����")] public int itemcount;
    public int SpawnCount = 0;                                  // ���� ������ ������ ����
    public TextMeshProUGUI ItemCountText;                       // ������ ������ ���� �ؽ�Ʈ
    [SerializeField] LevelManager LM;                           // ���� �Ŵ���


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
        // ������ ����
        if(Input.GetMouseButtonDown(0) && ItemButtons[CurrentButtonPressed].Cliked && !EventSystem.current.IsPointerOverGameObject() && itemcount > 0)
        {
            Debug.Log("������ ����");
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
        //������ ����
        else if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Ŭ���� ��ġ�� ���� ����� ������Ʈ ã��
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
            Debug.Log("hitcollider name : " + hitCollider.name);
            // ������Ʈ�� �ְ�
            if (hitCollider != null)
            {
                
                // Ŭ���� ������Ʈ�� ���콺 Ŭ�� ��ġ ������ �Ÿ� ����
                float distance = Vector2.Distance(hitCollider.transform.position, mousePosition);
                Debug.Log("�Ÿ�: " + distance);
                Debug.Log("���콺 ������ : " +mousePosition);
                Debug.Log("hitcollider ������ : " + hitCollider.transform.position);
                Debug.Log("���Ͻ� : " + distance);
                // ���� �Ÿ� �̳��� �ִ� ��� Ŭ�� ó��
                if (distance < clickThreshold)
                {
                    // Ŭ���� ������Ʈ�� ���� ó�� ����
                    Debug.Log("Ŭ���� ������Ʈ: " + hitCollider.gameObject.name);
                    int index = (SpawnItempos.Count)-1;

                    for(int i = SpawnItempos.Count; i > 0; i--)
                    {
                        Debug.Log(index);
                        float x = (SpawnItempos[i - 1].y - hitCollider.transform.position.y);
                        if (x <= distance)
                        {
                            Debug.Log("2" + index);
                            // ���� index�� ���ؼ� �迭�� �ִ� ������ ����, �� ������ ��ũ��Ʈ�� ������ ���� ����
                            // SpawnCount ����
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
