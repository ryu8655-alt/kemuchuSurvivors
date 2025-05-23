using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawnerController : BaseWeaponSpawner
{
    // Update is called once per frame
    void Update()
    {
        //シールドが1つでも残っている場合は生成しない
        if (transform.childCount > 0) return;

        if (isSpawnTimerNotElapsed()) return;

        //武器生成
        for(int i = 0; i < _weaponStatus.SpawnCount; i++)
        {
           ShieldController ctrl = (ShieldController)CreateWeapon(transform.position, transform);

            //初期角度の設定
            ctrl._angle = 360f / _weaponStatus.SpawnCount * i;
        }

        //次の生成タイマー
        _spawnTimer = GetRandomSpawnTimer();

    }
}
