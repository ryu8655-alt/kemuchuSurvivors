using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawnercontroller : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //���퐶������
        for(int i = 0; i < _weaponStatus.SpawnCount; i++)
        {
            CreateWeapon(transform.position);
        }

        //���퐶�����Ԃ̃��Z�b�g
        _spawnTimer = GetRandomSpawnTimer();
    }
}
