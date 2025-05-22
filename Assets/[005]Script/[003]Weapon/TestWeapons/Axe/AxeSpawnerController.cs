using UnityEngine;

public class AxeSpawnerController : BaseWeaponSpawner
{
    //��x�̐����ɑ΂��Ď�����p�ӂ���
    int _onceSpawnCount;
    float _onceSpawnTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        //�ŗL�̃p�����[�^���Z�b�g����
        _onceSpawnCount = (int)_weaponStatus.SpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�̏�������
        if (isSpawnTimerNotElapsed()) return;

        //�����ō��E�ɐ�����������𕪂���
        int dir = (_onceSpawnCount % 2 == 0) ? -1 : 1;

        //��������
        AxeController ctrl = (AxeController)CreateWeapon(transform.position);
        //�΂ߏ�̕����ɗ͂�������
        ctrl.GetComponent<Rigidbody2D>().AddForce(new Vector2(100 * dir, 350));

        //���̐����^�C�}�[
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //���������O�ȉ��ɂȂ�����I��
        if (1 > _onceSpawnCount)
        {
            _spawnTimer = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }
    }
}
