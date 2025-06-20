using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    [SerializeField, Header("プレイヤー")]
    public PlayerController _playerController;

    [SerializeField, Header("Transform - TextDamage")]
    private Transform _parentTextDamage;

    [SerializeField, Header("Prefab-TextDamage ")]
    private GameObject _prefabTextDamage;

    //タイマー
    [SerializeField, Header("TextTimer")]
    private Text _textTimer;
    public float _gameTimer;
    public float _oldSeconds;

    //エネミースポナー
    [SerializeField, Header("EnemySpawner")]
    private EnemySpawnerController _enemySpawnerController;

    // //プレイヤー生成
    [SerializeField]
    private SliderHPController _sliderHPController;
    [SerializeField]
    private SliderXPController _sliderXPController;
    [SerializeField]
    Slider _sliderXP;
    [SerializeField]
    TextMeshProUGUI _textLv;

    //経験値プレハブリスト
    [SerializeField]
    List<GameObject> prefabXP;




    // Start is called before the first frame update
    void Start()
    {
        // InGameスタート時にPlayerの生成を行う
        int playerId = 0;
        _playerController = CharacterSettings.Instance.CreatePlayer(playerId, this, _enemySpawnerController,_textLv,_sliderXP);

        _sliderHPController.Init(_playerController);
        _sliderXPController.Init(_playerController);



        //初期設定
        _oldSeconds = -1;
        _enemySpawnerController.Init(this, _timemapCollider);

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
        //ゲーム中のタイマーを更新する
       UpdateGameTimer();
    }

    public void DispDamage(GameObject target , float damage)
    {
        GameObject obj = Instantiate(_prefabTextDamage, _parentTextDamage);
        obj.GetComponent<TextDamageController>().Init(target, damage);
    }



    private void UpdateGameTimer()
    {
        _gameTimer += Time.deltaTime;

        //秒数の変化があるかを確認
        int seconds = (int)_gameTimer % 60;
        if (seconds == _oldSeconds) return;
        
        _textTimer.text = Utils.GetTextTimer(_gameTimer);
        _oldSeconds = seconds;
    }


    public void CreateXP(EnemyController enemy)
    {
        float xp = Random.Range(enemy._characterStatus.XP,enemy._characterStatus.MaxXP);
        if (0 > xp) return;

        //5未満
        GameObject prefab = prefabXP[0];

        //10以上
        if (10 <= xp)
        {
            prefab = prefabXP[2];
        }
        else if (5 <= xp)
        {
            prefab = prefabXP[1];
        }

        //初期化とアイテムの生成
        GameObject obj = Instantiate(prefab, enemy.transform.position, Quaternion.identity);
        XPContoroller ctrl = obj.GetComponent<XPContoroller>();
        ctrl.Init(this, xp);
    }   


}


