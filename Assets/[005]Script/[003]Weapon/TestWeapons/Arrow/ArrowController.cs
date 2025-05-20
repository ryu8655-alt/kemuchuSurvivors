using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɂ��ẴN���X
/// �����𐧌䂷��
/// --��ȋ���--
/// �v���C���[�̂���ʒu��胉���_���G�Ɍ������Ė�����
/// ���Ԋu�A��x�ɔ��˂���{���A��̃X�s�[�h�Ȃǂ̃p�����[�^��
/// ScriptableObject WeaponSpawnerSettings�Őݒ肷��
/// /// </summary>



public class ArrowController : BaseWeapon
{

    //��𔭎˂���^�[�Q�b�g
    public EnemyController _targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //�i�s�����x�N�g��
        Vector2 forward = _targetEnemy.transform.position - transform.position;
        //�p�x�ɕϊ�����
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        //�p�x��������
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g��������Ȃ��ꍇ�͎��g���폜����
        if(!_targetEnemy)
        {
            Destroy(gameObject);
            return;
        }

        //�^�[�Q�b�g�Ɍ������Ĕ��ł���
        Vector2 forward = _targetEnemy.transform.position - transform.position;
        _rigidbody2d.position += forward.normalized * _weaponStatus.Speed * Time.deltaTime;

    }



    //�Փ˂����ꍇ�̏���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�G�ȊO�ɂ��������ꍇ�ɂ͏����𔲂���
        if(!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //�ʏ�̃_���[�W���擾����
        float attackPower = _weaponStatus.Attack;
        //���������Ώۂ��^�[�Q�b�g�������ꍇ�̓^�[�Q�b�g�Ώۂ���ɂ���
        if(_targetEnemy == enemy)
        {
            _targetEnemy = null;
        }
        else//�^�[�Q�b�g�ȊO�ɂ��������ꍇ��30%�̃_���[�W��^����
        {
            attackPower /= 3;
        }
        
        AttackEnemy(collision, attackPower);
    }
}
