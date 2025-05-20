using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseWeaponSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabWeapon;

    //武器データ
    public BaseWeaponStatus _weaponStatus;

    //与えた総ダメージ
    public float _totalDamage;
    //稼働タイマー
    public float _totalTimer;

    //生成タイマー
    protected float _spawnTimer;
    //生成を行った武器のリスト
    protected List<BaseWeapon> _weapons;
    //敵の生成装置
    protected EnemySpawnerController enemySpawnerController;


    //初期化変数
    public void  Init(EnemySpawnerController enemySpawnerController,BaseWeaponStatus baseWeaponStatus)
    {
        _weapons = new List<BaseWeapon>();
        this.enemySpawnerController = enemySpawnerController;
        this._weaponStatus = baseWeaponStatus;
    }


    //稼働タイマーの更新
    public void FixedUpdate()
    {
        _totalTimer += Time.fixedDeltaTime;
    }


    // 武器生成
    protected BaseWeapon CreateWeapon(Vector3 position, Vector2 forward, Transform parent = null)
    {
        //生成処理
        GameObject obj = Instantiate(_prefabWeapon, position, _prefabWeapon.transform.rotation, parent);

        //武器データセット
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();

        //データ初期化
        weapon.init(this, forward);
        //武器リストに追加
        _weapons.Add(weapon);

        return weapon;

    }

    //武器生成(簡易版)
    protected BaseWeapon CreateWeapon(Vector3 position,Transform parent = null)
    {
        return CreateWeapon(position, Vector2.zero, parent);
    }

    //武器のアップデートを停止する
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;
        //オブジェクトを削除する
        _weapons.RemoveAll(item => !item);
        //生成した武器を停止する
        foreach(var item in _weapons)
        {
            item.enabled = enabled;
            //rigidbody2dを停止する
            item.GetComponent<Rigidbody2D>().simulated = enabled;
        }
    }


    //タイマー消化チェック処理
    protected bool isSpawnTimerNotElapsed()
    {

        //タイマー消化
        _spawnTimer -= Time.deltaTime;
        if(0 < _spawnTimer)return true;
        return false;
    }

    protected float GetRandomSpawnTimer()
    {
        return Random.Range(_weaponStatus.SpawnIntervalMin, _weaponStatus.SpawnIntervalMax);
    }



    //レベルアップ時のデータを適用する

}
