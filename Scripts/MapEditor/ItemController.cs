using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public int ID;
    public bool Cliked = false;
    private itemEditorManager editor;
    private TileMapEditor TME;

    [SerializeField]
    public MouseOnButton MOB;

    Button button;

    void Start()
    {  
        editor = GameObject.FindGameObjectWithTag("ItemLeveleditor").GetComponent<itemEditorManager>();
        TME = GameObject.FindGameObjectWithTag("TileMapEditor").GetComponent<TileMapEditor>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonCliked);
    }

    // 배치한 아이템을 골랐을때 그 아이템으로 속성을 바꿔주는 함수

    public void ButtonCliked()
    {
        TME.ReSetTile();
        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        if (editor.itemcount > 0)
        {
            TME.SetClear();
            editor.CurrentButtonPressed = ID;

            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 wordlpos = Camera.main.ScreenToWorldPoint(screenPos);

            MOB.SelectImage.SetActive(true);
            MOB.isClicked = true;
            Cliked = true;

            Instantiate(editor.ItemPrefabs[ID].ItemImage, new Vector3(wordlpos.x, wordlpos.y, 0), Quaternion.identity);
        }
    }

    public void ButtonReSet()
    {
        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        Cliked = false;
        MOB.isClicked = false;
        MOB.SelectImage.SetActive(false);
        MOB.BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);
    }

}
