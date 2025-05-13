using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �v���C���[����ʂ̒��S�ɂƂ炦�Ȃ���Ǐ]���s��
/// �^�C���}�b�v�̊O�ɏo�Ȃ��悤�ɐ���������J�����N���X
/// </summary>

public class CameraController : MonoBehaviour
{
    private Transform _target;

    private GameSceneManager _gameManager;
    private Camera _mainCamera;
    private float _cameraZ;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        if(_mainCamera != null)
        {
            _cameraZ = _mainCamera.transform.position.z;
        }

        //GameSceneManager�̎擾(��ŉ��ς��s���\��)
        _gameManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        if (_gameManager == null)
        {
            Debug.LogError("[CameraController]GameSceneManager���擾�ł��܂���ł���");
        }

        _target = _gameManager._playerController.transform;

    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (_target == null || _mainCamera == null || _gameManager == null) return;

        Vector3 targetPos = _target.position;
        targetPos.z = _cameraZ;

        //�͈͂�Clamps���A�ʒu�̒������s��
        Vector3 clampPos = ClampToMapBounds(targetPos);
        //�J�����̈ʒu���X�V
        _mainCamera.transform.position = clampPos;
    }

    /// <summary> 
    /// �J�������}�b�v�̊O�ɏo�Ȃ��悤�ɐ������s��
    /// </summary>
    private Vector3 ClampToMapBounds(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, _gameManager._tileMapStart.x, _gameManager._tileMapEnd.x);
        position.y = Mathf.Clamp(position.y, _gameManager._tileMapStart.y, _gameManager._tileMapEnd.y);
        return position;
    }
}
