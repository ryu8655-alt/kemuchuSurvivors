using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerController : BaseWeaponSpawner
{
    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //���̃^�C�}�[���Z�b�g����
        _spawnTimer = GetRandomSpawnTimer();

        //�G�����Ȃ��ꍇ�͏����𔲂���
        if (1 > enemySpawnerController.GetEnemies().Count) return;

        for(int i = 0; i < (int)_weaponStatus.SpawnCount; i++)
        {
            //���퐶��
            ArrowController ctrl = (ArrowController)CreateWeapon(transform.position);

            //�����_���Ń^�[�Q�b�g�����肷��
            List<EnemyController> enemies = enemySpawnerController.GetEnemies();
            int randomIndex = Random.Range(0, enemies.Count);
            EnemyController�@targetEnemy = enemies[randomIndex];

            ctrl._targetEnemy = targetEnemy;
        }




    }
}
