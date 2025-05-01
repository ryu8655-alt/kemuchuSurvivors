using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : MonoBehaviour
{
    //タイルマップ
    [SerializeField, Header("マップ")]
    private GameObject _gridMap;
    [SerializeField, Header("マップ上の障害物")]
    private Tilemap _timemapCollider;//のちほどScene内に配置を行う予定
    //マップ全体座標
    public Vector2 _tileMapStart;
    public Vector2 _tileMapEnd;
    public Vector2 _worldStart;
    public Vector2 _worldEnd;


    // Start is called before the first frame update
    void Start()
    {

        //カメラの移動ができる範囲を設定する
        foreach (Transform item in _gridMap.GetComponentInChildren<Transform>())
        {
            //開始位置
            if (_tileMapStart.x > item.position.x)
            {
                _tileMapStart.x = item.position.x;
            }
            if (_tileMapStart.y > item.position.y)
            {
                _tileMapStart.y = item.position.y;
            }

            //終了位置
            if (_tileMapEnd.x < item.position.x)
            {
                _tileMapEnd.x = item.position.x;
            }
            if (_tileMapEnd.y < item.position.y)
            {
                _tileMapEnd.y = item.position.y;
            }
        }

            //画面縦半分の描画範囲(デフォルト状態で5タイル分を想定)
            float cameraSize = Camera.main.orthographicSize;
            //画面の縦横比率
            float aspect = (float)Screen.width / (float)Screen.height;
            //プレイヤーの移動できる範囲
            _worldStart =new Vector2 (_tileMapStart.x - cameraSize * aspect, _tileMapStart.y - cameraSize);
            _worldEnd = new Vector2(_tileMapEnd.x + cameraSize * aspect, _tileMapEnd.y + cameraSize);
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
