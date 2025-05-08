using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

//エネミースポーンパターンの種類
public enum SpawnPattern
{
    Normmal,
    Group,
}


//エネミースポーン設定データ
[Serializable]
public class EnemySpawnData
{
    //インスペクター上ででのタイトル
    public string _title;

    //出現経過時間
    [Header("出現経過時間")]
    public int _elapsedMinutes;
    public int _elapsedSeconds;

    [Header("スポーンパターン")]
    public SpawnPattern _spawnPattern;

    [Header("スポーン時間")]
    public float _spawnDuration;

    [Header("最大スポーン数")]
    public int _spawncountMax;

    [Header("スポーンをさせるエネミーデータID")]
    public List<int> _enemyIds;

}

public class EnemySpawnerController : MonoBehaviour
{
    //敵データ
    [SerializeField]
    private List<EnemySpawnData> _enemySpawnDatas;
    //スポーンされた敵
    List<EnemyController> enemies;

    //ゲームシーンマネージャー
    GameSceneManager _gameSceneManager;
    //当たり判定のあるタイルマップ
    Tilemap _tilemapCollider;
    EnemySpawnData _enemySpawnData;
    //経過時間
    float _oldSeconds;
    float _spawnTimer;

    //現在のデータ参照位置
    private int _spawnDataIndex;
    //敵のスポーン位置
    const float _spawnRadius = 13;


    //初期化処理
    public　void Init(GameSceneManager gameSceneManager, Tilemap tilemapCollider)
    {
        this._gameSceneManager = gameSceneManager;
        this._tilemapCollider = tilemapCollider;

        //生成した敵データを格納するリストを初期化する
        enemies = new List<EnemyController>();
        //参照データの位置を初期化する
        _spawnDataIndex = -1;
    }

    private void Update()
    {
        //敵生成データの更新
        UpdateEnemySpawnData();

        //スポーン処理
        EnemySpawn();
    }


    /// <summary>
    /// 敵のスポーン処理をEnemySpaenData内のSpawnPatternに従って行う
    /// </summary>
    private void EnemySpawn()
    {
        if (_enemySpawnData == null) return;

        //タイマーの消化処理
        _spawnTimer -= Time.deltaTime;
        if(_spawnTimer > 0) return;

        //SpawnPatternの種類に従ってそれぞれのスポーン処理を実行する
        if (SpawnPattern.Normmal == _enemySpawnData._spawnPattern)
        {
            NormalSpawn();
        }
        else if (SpawnPattern.Group == _enemySpawnData._spawnPattern)
        {
            GroupSpawn();
        }

        _spawnTimer = _enemySpawnData._spawnDuration;

    }



    private void CreateRandomEnemy(Vector3 position)
    {
        //データ内からランダムなIDを取得する
        int rnd = Random.Range(0, _enemySpawnData._enemyIds.Count);
        int id = _enemySpawnData._enemyIds[rnd];

        //敵生成
        EnemyController enemy = CharacterSettings.Instance.CretaeEnemy(id , _gameSceneManager, position);
        enemies.Add(enemy);
    }

    /// <summary>
    /// SpawnPattern.Normmalのスポーン処理
    /// </summary>
    private void NormalSpawn()
    {
        //プレイヤーの位置を中心点として取得する
        Vector3 center = _gameSceneManager._playerController.transform.position;

        //敵の生成処理
        for(int i = 0; i < _enemySpawnData._spawncountMax; ++i)
        {
            //プレイヤーの周りから出現する
            float angle = 360 / _enemySpawnData._spawncountMax * i;
            // Cos関数にラジアン角を指定すると、xの座標を返してくれる、radiusをかけてワールド座標に変換する
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;
            // Sin関数にラジアン角を指定すると、yの座標を返してくれる、radiusをかけてワールド座標に変換する
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

            //スポーン位置を計算する
            Vector2 pos = center + new Vector3(x, y, 0);

            //計算したスポーン位置に当たり判定のあるタイルマップが存在する場合は生成を行わない
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            //敵のスポーン処理
            CreateRandomEnemy(pos);

        }
    }

    private void GroupSpawn()
    {
        //プレイヤーの位置を中心点として取得をする
        Vector3 center = _gameSceneManager._playerController.transform.position;

        //敵をプレイヤーの周りから出現させる
        float angle = Random.Range(0, 360);
        // Cos関数にラジアン角を指定すると、xの座標を返してくれる、radiusをかけてワールド座標に変換する
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;
        // Sin関数にラジアン角を指定すると、yの座標を返してくれる、radiusをかけてワールド座標に変換する
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

        //スポーン位置の計算
        center += new Vector3(x, y, 0);
        float radius = 0.5f;

        //敵スポーン処理
        for (int i = 0; i < _enemySpawnData._spawncountMax; i++)
        {
            //プレイヤーの周りから出現するようにする
            angle = 360 / _enemySpawnData._spawncountMax * i;
            // Cos関数にラジアン角を指定すると、xの座標を返してくれる、radiusをかけてワールド座標に変換する
            x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            // Sin関数にラジアン角を指定すると、yの座標を返してくれる、radiusをかけてワールド座標に変換する
            y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            //生成位置
            Vector2 pos = center + new Vector3(x, y, 0);
            //当たり判定のあるタイルマップが存在する場合は生成を行わない
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            //敵のスポーン処理
            CreateRandomEnemy(pos);
        }
    }


    private void UpdateEnemySpawnData()
    {
        //経過秒数に違いがないときは処理を抜ける
        if (_oldSeconds == _gameSceneManager._gameTimer) return;

        //１つ先のデータを参照する
        int idx = _spawnDataIndex + 1;

        //デー丹生最後に来たときは処理を抜ける
        if (_enemySpawnDatas.Count - 1 < idx) return;

        //設定された経過時間を超えていた場合はデータの入れ替えを行う
        EnemySpawnData data = _enemySpawnDatas[idx];
        int elapsedSeconds = data._elapsedMinutes * 60 + data._elapsedSeconds;

        if(elapsedSeconds < _gameSceneManager._gameTimer)
        {
            _enemySpawnData = _enemySpawnDatas[idx];

            //次回用の設定
            _spawnDataIndex = idx;
            _spawnTimer = 0;
            _oldSeconds = _gameSceneManager._gameTimer;
        }
    }

    public List<EnemyController> GetEnemies()
    {
        enemies.RemoveAll(item => !item);
        return enemies;
    }

}