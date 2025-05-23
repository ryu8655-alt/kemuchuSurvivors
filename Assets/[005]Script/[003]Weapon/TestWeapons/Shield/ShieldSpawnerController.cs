using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawnerController : BaseWeaponSpawner
{
    // Update is called once per frame
    void Update()
    {
        //�V�[���h��1�ł��c���Ă���ꍇ�͐������Ȃ�
        if (transform.childCount > 0) return;

        if (isSpawnTimerNotElapsed()) return;

        //���퐶��
        for(int i = 0; i < _weaponStatus.SpawnCount; i++)
        {
           ShieldController ctrl = (ShieldController)CreateWeapon(transform.position, transform);

            //�����p�x�̐ݒ�
            ctrl._angle = 360f / _weaponStatus.SpawnCount * i;
        }

        //���̐����^�C�}�[
        _spawnTimer = GetRandomSpawnTimer();

    }
}
