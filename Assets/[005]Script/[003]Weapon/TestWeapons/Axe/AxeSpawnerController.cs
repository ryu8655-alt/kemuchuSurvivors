using UnityEngine;

public class AxeSpawnerController : BaseWeaponSpawner
{
    //一度の生成に対して時差を用意する
    int _onceSpawnCount;
    float _onceSpawnTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        //固有のパラメータをセットする
        _onceSpawnCount = (int)_weaponStatus.SpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーの消化処理
        if (isSpawnTimerNotElapsed()) return;

        //偶数で左右に生成する方向を分ける
        int dir = (_onceSpawnCount % 2 == 0) ? -1 : 1;

        //生成処理
        AxeController ctrl = (AxeController)CreateWeapon(transform.position);
        //斜め上の方向に力を加える
        ctrl.GetComponent<Rigidbody2D>().AddForce(new Vector2(100 * dir, 350));

        //次の生成タイマー
        _spawnTimer = _onceSpawnTime;
        _onceSpawnCount--;

        //生成数が０以下になったら終了
        if (1 > _onceSpawnCount)
        {
            _spawnTimer = GetRandomSpawnTimer();
            _onceSpawnCount = (int)_weaponStatus.SpawnCount;
        }
    }
}
