using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //親の生成装置
    protected BaseWeaponSpawner _weaponSpawner;
    //武器ステータス
    protected BaseWeaponStatus _weaponStatus;
    //物理挙動
    protected Rigidbody2D _rigidbody2d;
    //方向
    protected Vector2 _forward;

    //初期化処理
    public void init(BaseWeaponSpawner spawner, Vector2 forward)
    {
        //親の武器生成装置
        this._weaponSpawner = spawner;
        //武器のステータス
        this._weaponStatus = (BaseWeaponStatus)spawner._weaponStatus.GetCopy();
        //物理挙動
        this._rigidbody2d = GetComponent<Rigidbody2D>();
        //方向
        this._forward = forward;

        //生存時間がある場合は設定する
        if (-1 < _weaponStatus.AliveTime)
        {
            Destroy(gameObject, _weaponStatus.AliveTime);
        }
    }

    protected void AttackEnemy(Collider2D collider2d, float attackPower)
    {
        if (!collider2d.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;
        //ダメージ計算処理
        float damage = enemy.Damage(attackPower);

        //総ダメージにも加算する
        _weaponSpawner._totalDamage += damage;

        //貫通数の設定がある場合は減産する
        if (0 > _weaponStatus.PenetrationCount) return;
        _weaponStatus.PenetrationCount--;
        if (0 > _weaponStatus.PenetrationCount)
        {
            //貫通数が0以下になった場合は武器を消滅させる
            Destroy(gameObject);
        }
    }


    //敵への攻撃(デフオルトの攻撃力)
    protected void AttackEnemy(Collider2D collider2d)
    {
        AttackEnemy(collider2d, _weaponStatus.Attack);
    }




}
