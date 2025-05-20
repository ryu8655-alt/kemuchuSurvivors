using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseWeaponSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabWeapon;

    //����f�[�^
    public BaseWeaponStatus _weaponStatus;

    //�^�������_���[�W
    public float _totalDamage;
    //�ғ��^�C�}�[
    public float _totalTimer;

    //�����^�C�}�[
    protected float _spawnTimer;
    //�������s��������̃��X�g
    protected List<BaseWeapon> _weapons;
    //�G�̐������u
    protected EnemySpawnerController enemySpawnerController;


    //�������ϐ�
    public void  Init(EnemySpawnerController enemySpawnerController,BaseWeaponStatus baseWeaponStatus)
    {
        _weapons = new List<BaseWeapon>();
        this.enemySpawnerController = enemySpawnerController;
        this._weaponStatus = baseWeaponStatus;
    }


    //�ғ��^�C�}�[�̍X�V
    public void FixedUpdate()
    {
        _totalTimer += Time.fixedDeltaTime;
    }


    // ���퐶��
    protected BaseWeapon CreateWeapon(Vector3 position, Vector2 forward, Transform parent = null)
    {
        //��������
        GameObject obj = Instantiate(_prefabWeapon, position, _prefabWeapon.transform.rotation, parent);

        //����f�[�^�Z�b�g
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();

        //�f�[�^������
        weapon.init(this, forward);
        //���탊�X�g�ɒǉ�
        _weapons.Add(weapon);

        return weapon;

    }

    //���퐶��(�ȈՔ�)
    protected BaseWeapon CreateWeapon(Vector3 position,Transform parent = null)
    {
        return CreateWeapon(position, Vector2.zero, parent);
    }

    //����̃A�b�v�f�[�g���~����
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;
        //�I�u�W�F�N�g���폜����
        _weapons.RemoveAll(item => !item);
        //��������������~����
        foreach(var item in _weapons)
        {
            item.enabled = enabled;
            //rigidbody2d���~����
            item.GetComponent<Rigidbody2D>().simulated = enabled;
        }
    }


    //�^�C�}�[�����`�F�b�N����
    protected bool isSpawnTimerNotElapsed()
    {

        //�^�C�}�[����
        _spawnTimer -= Time.deltaTime;
        if(0 < _spawnTimer)return true;
        return false;
    }

    protected float GetRandomSpawnTimer()
    {
        return Random.Range(_weaponStatus.SpawnIntervalMin, _weaponStatus.SpawnIntervalMax);
    }



    //���x���A�b�v���̃f�[�^��K�p����

}
