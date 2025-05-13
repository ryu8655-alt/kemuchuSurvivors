using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// プレイヤーを画面の中心にとらえながら追従を行い
/// タイルマップの外に出ないように制限をするカメラクラス
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

        //GameSceneManagerの取得(後で改変を行う予定)
        _gameManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        if (_gameManager == null)
        {
            Debug.LogError("[CameraController]GameSceneManagerが取得できませんでした");
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

        //範囲をClampsし、位置の調整を行う
        Vector3 clampPos = ClampToMapBounds(targetPos);
        //カメラの位置を更新
        _mainCamera.transform.position = clampPos;
    }

    /// <summary> 
    /// カメラがマップの外に出ないように制限を行う
    /// </summary>
    private Vector3 ClampToMapBounds(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, _gameManager._tileMapStart.x, _gameManager._tileMapEnd.x);
        position.y = Mathf.Clamp(position.y, _gameManager._tileMapStart.y, _gameManager._tileMapEnd.y);
        return position;
    }
}
