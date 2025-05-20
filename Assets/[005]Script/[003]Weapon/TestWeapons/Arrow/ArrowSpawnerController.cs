using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerController : BaseWeaponSpawner
{
    // Update is called once per frame
    void Update()
    {
        if (isSpawnTimerNotElapsed()) return;

        //次のタイマーをセットする
        _spawnTimer = GetRandomSpawnTimer();

        //敵がいない場合は処理を抜ける
        if (1 > enemySpawnerController.GetEnemies().Count) return;

        for(int i = 0; i < (int)_weaponStatus.SpawnCount; i++)
        {
            //武器生成
            ArrowController ctrl = (ArrowController)CreateWeapon(transform.position);

            //ランダムでターゲットを決定する
            List<EnemyController> enemies = enemySpawnerController.GetEnemies();
            int randomIndex = Random.Range(0, enemies.Count);
            EnemyController　targetEnemy = enemies[randomIndex];

            ctrl._targetEnemy = targetEnemy;
        }




    }
}
