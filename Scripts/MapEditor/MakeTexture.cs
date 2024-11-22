using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MakeTexture : MonoBehaviour
{
    [Header("������ �� ��")]
    public Tilemap[] TM;                                                            // �������� �� ��

    [Header("������ �Ϸ��� �ý�ó")]
    [SerializeField]Texture2D[] TileMapTexture = new Texture2D[3];                  // ������ �Ϸ��� �ؽ��ĸ� ���� �ؽ��� 2d

    [Header("������ �� ī�޶�")]
    public Camera[] RenderCam;                                                      // �������� ī�޶�

    [Header("������ �̹���")]
    [SerializeField] RawImage[] raw = new RawImage[3];                              // �������� �̹����� ���� RawImage

    [Header("�� ��ü")]
    [SerializeField] RawImage[] SelectTextuer;                                      // ���� ��ü�� �ؽ���
    [SerializeField] GameObject[] SelectButtonUi;                                   // ���� ��ü�� ��ư

    [Header("����� �� �ҷ�����")]
    [SerializeField] RawImage[] LoadSelectTextuer;                                  // ����� ���� �ҷ��� �ؽ���
    [SerializeField] GameObject[] LoadSelectButtonUi;                               // ����� ���� �ҷ��� ��ư

    [Header("���� ��")]
    [SerializeField] RawImage[] MainSelectTextuer;                                  // ���� �� �ؽ���
    [SerializeField] GameObject[] MainSelectButtonUi;                               // ���� �� ��ư


    [Header("�� ������")]
    public LevelData[] LD;                                                          // Ÿ�ϸ� �����͸� ���� �迭

    [SerializeField] GameObject[] ButtonUI = new GameObject[3];                     // ��ư�� �������� �������� UI


    #region �� Ÿ�Կ� �´� �ؽ�ó�� ���� �ؽ�ó ��ü
    public void ChangeTextureSelect()
    {
        for (int i = 0; i < SelectButtonUi.Length; i++)
        {
            ButtonUI[i] = SelectButtonUi[i];
            raw[i] = SelectTextuer[i];
        }
        MakeTM();
    }

    public void LoadTextureSelect()
    {
        for (int i = 0; i < LoadSelectButtonUi.Length; i++)
        {
            ButtonUI[i] = LoadSelectButtonUi[i];
            raw[i] = LoadSelectTextuer[i];
        }
        MakeTM();
    }

    public void MainMapTextureSelect()
    {
        for (int i = 0; i < MainSelectButtonUi.Length; i++)
        {
            ButtonUI[i] = MainSelectButtonUi[i];
            raw[i] = MainSelectTextuer[i];
        }
        MakeTM();
    }
    #endregion

    #region �ؽ�ó�� �������� ���� �׸��� �Լ�
    void MakeTM()
    { 
        if(LD.Length == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < LD.Length; i++)
            {
                if (LD[i] == null)
                {
                    return;
                }
                else
                {
                    for (int j = 0; j < LD[i].pos.Count; j++)
                    {
                        TM[i].SetTile(LD[i].pos[j], LD[i].tiles[j].TileBase);
                    }
                    RenderTextureAction(i);
                }
            }
        }
    }
    #endregion

    #region �׸� ���� �ؽ�óȭ ��Ű�� �Լ�
    void RenderTextureAction(int i)
    {
        ButtonUI[i].SetActive(true);
       
        RenderCam[i].enabled = true;
       
        RenderTexture TileManpRenderTexture;    // �������� �� �����ؽ���
       
        TileManpRenderTexture = new RenderTexture(490, 853, 0);
       
        TileMapTexture[i] = new Texture2D(490, 853);
       
        RenderCam[i].targetTexture = TileManpRenderTexture;
       
        RenderCam[i].Render();
       
        RenderTexture.active = TileManpRenderTexture;
       
        TileMapTexture[i].ReadPixels(new Rect(0, 0, TileManpRenderTexture.width, TileManpRenderTexture.height), 0, 0);
       
        TileMapTexture[i].Apply();

        raw[i].texture = TileMapTexture[i];
       
        RenderTexture.active = null;
       
        RenderCam[i].targetTexture = null;
       
        RenderCam[i].enabled = false;

        TM[i].ClearAllTiles();
       
        Destroy(TileManpRenderTexture);
    }
    #endregion
}
