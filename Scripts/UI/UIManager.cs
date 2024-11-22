using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("�� ������ ��ü ������Ʈ")]
    #region �� ������ obj
    [SerializeField] GameObject MapEditor;
    [SerializeField] GameObject MapEditorPoPUpButton;
    [SerializeField] GameObject MapEditorSaveUI;
    #endregion

    [Header("�� ���� ����")]
    #region �� ���� ����
    [Header("Ÿ�ϸ�")]
    [SerializeField] GameObject TileSelectTopBoutton;
    [SerializeField] GameObject TileSelecter;
    [SerializeField] GameObject TileSelectBG;
    [Header("�� Ÿ��")]
    [SerializeField] GameObject TileHookSelectTopButton;
    [SerializeField] GameObject TileHookSelect;
    [SerializeField] GameObject TileHookSelectBG;
    [Header("������")]
    [SerializeField] GameObject ItemSelectTopBoutton;
    [SerializeField] GameObject ItemSelecter;
    [SerializeField] GameObject ItemSelectBG;


    [Header("��Ÿ")]
    [SerializeField] TextMeshProUGUI TEXT;
    [SerializeField] GameObject IdleSelecter;
    #endregion

    [Header("�� ������ ��ü ��")]
    #region �� ������ ī�޶�
    [SerializeField] Cinemachine.CinemachineVirtualCamera MainViewCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera MainEditorCam;

    [SerializeField] Cinemachine.CinemachineVirtualCamera ViewCam1;
    [SerializeField] Cinemachine.CinemachineVirtualCamera ViewCam2;

    [SerializeField] Cinemachine.CinemachineVirtualCamera MapEditorCam1;
    [SerializeField] Cinemachine.CinemachineVirtualCamera MapEditorCam2;

    [SerializeField] ParallaxBackGround PBG1;
    [SerializeField] ParallaxBackGround PBG2;
    [SerializeField] ParallaxBackGround MainPBG;
    #endregion


    public void Start()
    {
        #region �� ������ ������ ���� ī�޶� ��ü
        if (GameManager.instance.MapEditorIndex  == 0)
        {
            MainViewCam = ViewCam1;
            MainEditorCam = MapEditorCam1;
            MainPBG = PBG1;
        }
        else if(GameManager.instance.MapEditorIndex == 1)
        {
            MainViewCam = ViewCam2;
            MainEditorCam = MapEditorCam2;
            MainPBG = PBG2;
        }
        #endregion
    }

    #region �� ��üȭ�� ��ư
    public void MapAllView()
    {
        MainViewCam.gameObject.SetActive(true);

        MainPBG.vcam = MainViewCam;
        MainPBG.SetBGSize(MainViewCam);
    }
    public void MapEditorView()
    {
        MainViewCam.gameObject.SetActive(false);
        MainPBG.vcam = MainEditorCam;
        MainPBG.SetBGSize(MainEditorCam);
    }
    #endregion
    #region �� ������ â ���� ��ư
    public void MapEditorSelectButton()
    {
        MapEditor.SetActive(true);
        MapEditor.GetComponent<TileSelectPoPUp>().PoPUp();
        MapEditorSaveUI.SetActive(true);
        MapEditorPoPUpButton.SetActive(false);
    }
    #endregion

    #region �� ������ â �ݱ� ��ư
    public void XButtonAction()
    {
        MapEditor.SetActive(false);
        MapEditorSaveUI.SetActive(false);
        MapEditorPoPUpButton.SetActive(true);
    }
    #endregion

    #region Open Tile Selecter
    public void TileSelecterButton()
    {
        TileSelecter.SetActive(true);
        TileSelectBG.SetActive(true);
        TileSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = true;

        ItemSelecter.SetActive(false);
        ItemSelectBG.SetActive(false);
        ItemSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = false;
        ItemSelectTopBoutton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);


        TileHookSelect.SetActive(false);
        TileHookSelectBG.SetActive(false);
        TileHookSelectTopButton.GetComponent<MouseOnButton>().isClicked = false;
        TileHookSelectTopButton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);

        TEXT.transform.gameObject.SetActive(true);

        IdleSelecter.SetActive(false);
    }
    #endregion

    #region Open Item Selecter
    public void ItemSelectButton()
    {
        ItemSelecter.SetActive(true);
        ItemSelectBG.SetActive(true);
        ItemSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = true;

        TEXT.transform.gameObject.SetActive(false);

        TileSelecter.SetActive(false);
        TileSelectBG.SetActive(false);
        TileSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = false;
        TileSelectTopBoutton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);

        TileHookSelect.SetActive(false);
        TileHookSelectBG.SetActive(false);
        TileHookSelectTopButton.GetComponent<MouseOnButton>().isClicked = false;
        TileHookSelectTopButton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);

        IdleSelecter.SetActive(false);
    }
    #endregion

    #region Open Hook Selecter
    public void HookSelectButton()
    {
        TileHookSelect.SetActive(true);
        TileHookSelectBG.SetActive(true);
        TileHookSelectTopButton.GetComponent<MouseOnButton>().isClicked = true;

        TEXT.transform.gameObject.SetActive(true);

        TileSelecter.SetActive(false);
        TileSelectBG.SetActive(false);
        TileSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = false;
        TileSelectTopBoutton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);

        ItemSelecter.SetActive(false);
        ItemSelectBG.SetActive(false);
        ItemSelectTopBoutton.GetComponent<MouseOnButton>().isClicked = false;
        ItemSelectTopBoutton.GetComponent<MouseOnButton>().BackGroundImage.color = new Color(1f, 1f, 1f, 0.4f);

        IdleSelecter.SetActive(false);
    }
    #endregion
}
