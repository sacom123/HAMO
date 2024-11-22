using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    #region 움직이는 배경
    [SerializeField] private Transform cameraTransform;

    private Vector3 cameraStartPosition;
    private Vector2 distance;

    [SerializeField]private Material BackGround;
    [SerializeField]private float LayerMovespeed;


    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float parallaxSpeed;
    #endregion

    #region 배경 크기 조절
    private Vector2 initialSize;
    [SerializeField]private Camera maincamera;
    public Cinemachine.CinemachineVirtualCamera vcam;

    #endregion

    private void Awake()
    {
        cameraStartPosition = vcam.transform.position;
        BackGround = GetComponent<Renderer>().material;
        initialSize = transform.localScale;
    }
    private void Start()
    {
        Vector3 BGPos = new Vector3(vcam.transform.position.x, vcam.transform.position.y, 10f);
        transform.position = BGPos;

    }

    public void SetBGSize(Cinemachine.CinemachineVirtualCamera vcm)
    {
        float scaleFactor = (vcm.m_Lens.OrthographicSize / initialSize.y) * 5;
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, initialSize.x * scaleFactor, Time.deltaTime),
            Mathf.Lerp(transform.localScale.y, initialSize.y * scaleFactor, Time.deltaTime), Time.deltaTime * 15);
    }

    private void LateUpdate()
    {
        distance = (vcam.transform.position - cameraStartPosition) * parallaxSpeed;
        
        transform.position = new Vector3(vcam.transform.position.x, vcam.transform.position.y, transform.position.z);

        Vector2 offset = new Vector2(distance.x, distance.y) * LayerMovespeed;
        BackGround.SetTextureOffset("_MainTex", offset);


        float scaleFactor = (vcam.m_Lens.OrthographicSize / initialSize.y)*5;
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, initialSize.x * scaleFactor, Time.deltaTime),
            Mathf.Lerp(transform.localScale.y, initialSize.y * scaleFactor, Time.deltaTime), Time.deltaTime * 15); 
    }

    private void OnEnable()
    {
        Vector3 BGPos = new Vector3(vcam.transform.position.x, vcam.transform.position.y, 10f);
        transform.position = BGPos;
        
    }

}
