using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnerController : BaseWeaponSpawner
{
    //�����Ɏ���������
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

        //����̐�������
        KnifeController ctrl = (KnifeController)CreateWeapon(transform.position, _playerController._forward);

        //���̐����^�C�}�[
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //1��̐������I������烊�Z�b�g
        if(1 > _onceSpawnCount)
        {
            _totalDamage = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }

    }
}
