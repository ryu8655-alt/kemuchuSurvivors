using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    //�e�̐������u
    protected BaseWeaponSpawner _weaponSpawner;
    //����X�e�[�^�X
    protected BaseWeaponStatus _weaponStatus;
    //��������
    protected Rigidbody2D _rigidbody2d;
    //����
    protected Vector2 _forward;

    //����������
    public void init(BaseWeaponSpawner spawner, Vector2 forward)
    {
        //�e�̕��퐶�����u
        this._weaponSpawner = spawner;
        //����̃X�e�[�^�X
        this._weaponStatus = (BaseWeaponStatus)spawner._weaponStatus.GetCopy();
        //��������
        this._rigidbody2d = GetComponent<Rigidbody2D>();
        //����
        this._forward = forward;

        //�������Ԃ�����ꍇ�͐ݒ肷��
        if (-1 < _weaponStatus.AliveTime)
        {
            Destroy(gameObject, _weaponStatus.AliveTime);
        }
    }

    protected void AttackEnemy(Collider2D collider2d, float attackPower)
    {
        if (!collider2d.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;
        //�_���[�W�v�Z����
        float damage = enemy.Damage(attackPower);

        //���_���[�W�ɂ����Z����
        _weaponSpawner._totalDamage += damage;

        //�ђʐ��̐ݒ肪����ꍇ�͌��Y����
        if (0 > _weaponStatus.PenetrationCount) return;
        _weaponStatus.PenetrationCount--;
        if (0 > _weaponStatus.PenetrationCount)
        {
            //�ђʐ���0�ȉ��ɂȂ����ꍇ�͕�������ł�����
            Destroy(gameObject);
        }
    }


    //�G�ւ̍U��(�f�t�I���g�̍U����)
    protected void AttackEnemy(Collider2D collider2d)
    {
        AttackEnemy(collider2d, _weaponStatus.Attack);
    }




}
