using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawnercontroller : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //武器生成処理
        for(int i = 0; i < _weaponStatus.SpawnCount; i++)
        {
            CreateWeapon(transform.position);
        }

        //武器生成時間のリセット
        _spawnTimer = GetRandomSpawnTimer();
    }
}
