using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawnercontroller : BaseWeaponSpawner
{

    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //•Ší¶¬ˆ—
        for(int i = 0; i < _weaponStatus.SpawnCount; i++)
        {
            CreateWeapon(transform.position);
        }

        //•Ší¶¬ŠÔ‚ÌƒŠƒZƒbƒg
        _spawnTimer = GetRandomSpawnTimer();
    }
}
