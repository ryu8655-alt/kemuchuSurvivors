using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSpawnerController : BaseWeaponSpawner
{
    //��x�̐����Ɏ���������
    int _onceSpawnCount;
    float _onceSpawnTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //�ŗL�p�����[�^(��x�̐��������Z�b�g����)
        _onceSpawnCount = (int) _weaponStatus.SpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�̏�������
        _spawnTimer -= Time.deltaTime;
        if (0 < _spawnTimer) return;

        //������̐������f�ō��E�̕��������肷��
        int dir = (_onceSpawnCount % 2 == 0) ? 1 : -1;

        //�ꏊ
        Vector3�@pos =  transform.position;
        pos.x += 2f * dir;

        //��������
        SlashController ctrl = (SlashController)CreateWeapon(pos, transform);
        //�p�x��ύX����
        ctrl.transform.eulerAngles = ctrl.transform.eulerAngles * dir;

        //���̐����^�C�}�[
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //���̐������I�������^�C�}�[�Ɛ����񐔂����Z�b�g����
        if(1> _onceSpawnCount)
        {
            _spawnTimer = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }
    }
}
