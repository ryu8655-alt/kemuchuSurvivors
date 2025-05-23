using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSpawnerController : BaseWeaponSpawner
{
    //一度の生成に時差をつける
    int _onceSpawnCount;
    float _onceSpawnTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //固有パラメータ(一度の生成数をセットする)
        _onceSpawnCount = (int) _weaponStatus.SpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーの消化処理
        _spawnTimer -= Time.deltaTime;
        if (0 < _spawnTimer) return;

        //偶数回の生成判断で左右の方向を決定する
        int dir = (_onceSpawnCount % 2 == 0) ? 1 : -1;

        //場所
        Vector3　pos =  transform.position;
        pos.x += 2f * dir;

        //生成する
        SlashController ctrl = (SlashController)CreateWeapon(pos, transform);
        //角度を変更する
        ctrl.transform.eulerAngles = ctrl.transform.eulerAngles * dir;

        //次の生成タイマー
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //一回の生成が終わったらタイマーと生成回数をリセットする
        if(1> _onceSpawnCount)
        {
            _spawnTimer = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }
    }
}
