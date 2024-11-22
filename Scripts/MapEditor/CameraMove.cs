using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class CameraMove : MonoBehaviour
{
    #region ī�޶� �̵�(�巡��, Ű����) ���� ����
    [SerializeField]
    float _dragSpeed = 10f;
    [SerializeField]
    float _inputSpeed = 20;

    #endregion

    #region �� ����
    private const float ZoomSpeed = 20f; // �ѹ��� �� �Է��� �� �Ǵ� ����
    private const float MinZoomSize = 5f; // �ּ� ī�޶� ������
    private const float MaxZoomSize = 11f; //  �ִ� ī�޶� ������
    // ���� : �� ����
    private float _targetZoomSize; // ��ǥ ī�޶� ũ��
    #endregion

    // ������Ʈ
    [SerializeField] Camera _camera; // ī�޶� ������Ʈ

    [SerializeField]
    Cinemachine.CinemachineVirtualCamera vcam;
    [SerializeField] 
    CinemachineConfiner2D confiner;
    [SerializeField]
    private Collider2D confinerCollider;
    [SerializeField] GameObject BackGround;


    public float posX;
    public float posZ;

    private void Start()
    {
        _targetZoomSize = vcam.m_Lens.OrthographicSize;

        if(confiner != null)
        {
            confinerCollider = confiner.m_BoundingShape2D;
            
            if(confinerCollider == null)
            {
                Debug.LogError("Confiner�� Collider2D�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Confiner�� �����ϴ�.");
        }
    }

    private void LateUpdate()
    {
        CameraDrag(); 
        CameraInput();
        CameraZoom();
        ZoomUpdate();
    }

    #region  Camera zoom
    void CameraZoom()
    {
        // ���콺 ��ũ�� �Է� �ޱ�
        var scrollInput = Input.GetAxis("Mouse ScrollWheel");
        var hasScrollInput = Mathf.Abs(scrollInput) > Mathf.Epsilon;
        if (!hasScrollInput)
        {
            return;
        }

        // ī�޶� ũ�⸦ ���콺 ��ũ�� �Է¿� ���� �����Ͽ� Ȯ��/���
        var newSize = vcam.m_Lens.OrthographicSize - scrollInput * ZoomSpeed;

        // ī�޶� ũ�� ���� �ּҰ��� �ִ밪 ���̷� ����
        _targetZoomSize = Mathf.Clamp(newSize, MinZoomSize, MaxZoomSize);
    }
    #endregion

    #region Camera Zoom Update
    private void ZoomUpdate()
    {
        // ī�޶� ũ�⸦ �ε巴�� ����
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, _targetZoomSize, Time.deltaTime * 10f);
        //AdjustColliderSize();
    }
    #region Camera Collider
    private void AdjustColliderSize()
    {
        // Get the size of the camera's viewport
        float cameraHeight = vcam.m_Lens.OrthographicSize * 2f;
        float cameraWidth = cameraHeight * vcam.m_Lens.Aspect;

        // Set the collider size to match the camera viewport
        var collider2D = GetComponent<BoxCollider2D>();
        collider2D.size = new Vector2(cameraWidth, cameraHeight);
    }
    #endregion
    #endregion


    #region Camera drag
    void CameraDrag()
    {
        if (Input.GetMouseButton(2))
        {
            posX = Input.GetAxis("Mouse X");
            posZ = Input.GetAxis("Mouse Y");

            Quaternion v3Rotation = Quaternion.Euler(0f, transform.eulerAngles.y, -10f);
            Vector3 newPos = v3Rotation * new Vector3(posX * -_dragSpeed, posZ * -_dragSpeed, -10f);
            
            if (confinerCollider != null)
            {
                Bounds bounds = confinerCollider.bounds;                                                    // ī�޶� �̵��� �� �ִ� ���� Bounds�� �Ǵ�
                newPos.x = Mathf.Clamp(newPos.x + transform.position.x, bounds.min.x, bounds.max.x);        // Clamp�Լ��� x�� �̵����� ����
                newPos.y = Mathf.Clamp(newPos.y + transform.position.y, bounds.min.y, bounds.max.y);        // Clamp�Լ��� y�� �̵����� ����
            }
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * _dragSpeed);
        }
    }
    #endregion

    #region Camera Input
    private void CameraInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
            p_Velocity += new Vector3(0, 1f, 0);
        if (Input.GetKey(KeyCode.S))
            p_Velocity += new Vector3(0, -1f, 0);
        if (Input.GetKey(KeyCode.Alpha1))
            p_Velocity += new Vector3(0, 0, 1f);
        if (Input.GetKey(KeyCode.Alpha2))
            p_Velocity += new Vector3(0, 0, -1f);
        if (Input.GetKey(KeyCode.A))
            p_Velocity += new Vector3(-1f, 0, 0);
        if (Input.GetKey(KeyCode.D))
            p_Velocity += new Vector3(1f, 0, 0);

        Vector3 p = p_Velocity;
        if (p.sqrMagnitude > 0)
        {
            p.x = Mathf.Clamp(p.x, -_inputSpeed, _inputSpeed);
            p.y = Mathf.Clamp(p.y, -_inputSpeed, _inputSpeed);
            p.z = Mathf.Clamp(p.z, -_inputSpeed, _inputSpeed);
            
            Vector3 p2 = p;
            if (confinerCollider != null)
            {
                Bounds bounds = confinerCollider.bounds;
                p2.x = Mathf.Clamp(p2.x + transform.position.x, bounds.min.x, bounds.max.x);
                p2.y = Mathf.Clamp(p2.y + transform.position.y, bounds.min.y, bounds.max.y);
                p2.z = -10;
            }
            //transform.Translate(p2);
            transform.position = p2;
        }
    }

    #endregion


    private void OnEnable()
    {
        Vector3 BGPos = new Vector3(transform.position.x, transform.position.y, 10f);
        BackGround.SetActive(true);
        BackGround.transform.position = BGPos;
    }
}
