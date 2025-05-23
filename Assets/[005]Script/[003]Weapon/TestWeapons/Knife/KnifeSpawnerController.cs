using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnerController : BaseWeaponSpawner
{
    //生成に時差をつける
    int _onceSpawnCount;
    float _onceSpawnTime = 0.1f;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _onceSpawnCount = (int) _weaponStatus.SpawnCount;
        _playerController = transform.parent.GetComponent<PlayerController>();


    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //武器の生成処理
        KnifeController ctrl = (KnifeController)CreateWeapon(transform.position, _playerController._forward);

        //次の生成タイマー
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //1回の生成が終わったらリセット
        if(1 > _onceSpawnCount)
        {
            _totalDamage = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }

    }
}
