using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Rnadom = UnityEngine.Random;

public enum SpawnTyep
{
    Normal,
    Group,
}

[System.Serializable]
public class EnemySpawnData
{
    //インスペクター上での説明用
    public string Title;

    [Header("出現経過時間")]
    public float ElapsedMinutes;
    public int ElapsedSeconds;

    [Header("出現パターン")]
    public SpawnTyep SpawnType;

    [Header("生成時間")]
    public float SpawnDuration;
    [Header("最大生成数")]
    public int _spawnCountMax;
    [Header("生成する敵のID")]
    public List<int> EnemyIDs;
}




public class EnemySpawnerController : MonoBehaviour
{
    //敵のデータ
    [SerializeField,Header("敵のデータ")]
    private List<EnemySpawnData> _enemySpawnDataList;

    //生成をした敵のリスト
    List<EnemyController> enemies;

    GameSceneManager _gameSceneManager;
    //当たり判定のあるタイルマップ
    Tilemap tilemapCollider;
    //現在の参照データ
    EnemySpawnData _enemySpawnData;

    //経過時間
    float _oldSeconds;
    float _spawnTimer;
    //現在のデータ位置
    int _spawnDataIndex;
    //敵の生成位置(生成パターンGropにて使用予定)
    const float SpawnRadius = 13;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //適生成データを更新
       
        //敵のスポーン処理
    }

    public void Init(GameSceneManager sceneManager , Tilemap timemapCollider)
    {
        this._gameSceneManager = sceneManager;
        this.tilemapCollider = timemapCollider;


        //リストの初期化
        enemies = new List<EnemyController>();
        _spawnDataIndex = -1;
    }

    private void SpawnEnemy()
    {
        if (null == _enemySpawnData) return;

        //タイマーの消化
        _spawnTimer -= Time.deltaTime;
        if (0 < _spawnTimer) return;

        if (SpawnTyep.Group == _enemySpawnData.SpawnType)
        {
            SpawnGroup();
        }else if(SpawnTyep.Normal ==_enemySpawnData.SpawnType){

            SpawnNormal();
        }

        _spawnTimer= _enemySpawnData.SpawnDuration;

    }

    private void SpawnNormal()
    {
        //プレイヤーの位置を取得し、スポーン範囲の中心として設定
        Vector3 center = _gameSceneManager._playerController.transform.position;

    

        //敵を生成する
        for (int i = 0; i < _enemySpawnData._spawnCountMax; ++i)
        {
                  
            //プレイヤーの周囲から敵を出現させる
            float angle = Random.Range(0, 360);
            // Cos関数にラジアン角を指定すると、xの座標を返してくれる、radiusをかけてワールド座標に変換する
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius;
            // Sin関数にラジアン角を指定すると、yの座標を返してくれる、radiusをかけてワールド座標に変換する
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius;

            //生成位置の決定
            Vector2 pos = center + new Vector3(x,y,0);

            //当たり判定のあるタイルマップの上に生成されないようにする
            if (Utils.IsColliderTile(tilemapCollider, pos)) continue;

            //生成
            CreateRandomEnemy(pos);
        }
    }

    private void SpawnGroup()
    {

    }

    private void CreateRandomEnemy(Vector3 pos)
    {

    }
}
