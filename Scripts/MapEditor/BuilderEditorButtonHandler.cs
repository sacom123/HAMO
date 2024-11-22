using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ÿ�� style ���� ��ư �ڵ鷯

public class BuilderEditorButtonHandler : MonoBehaviour
{

    [SerializeField] BuildingobjectBase tile;
    Button button;
    public TileMapEditor TME;
    //public TextMeshProUGUI ButtonText;
    [SerializeField]int TileMapNumber;
    public MouseOnButton MOB;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        TME.SetBuild(tile, TileMapNumber);
    }
}
