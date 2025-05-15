using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

// �E�N���b�N���j���[�ɕ\������Afilename�̓f�t�H���g�̃t�@�C����
[CreateAssetMenu(fileName = "WeaponSpawnerSettings", menuName = "ScriptableObjects/WeaponSpawnerSettings")]


public class WeaponSpawnerSettings : ScriptableObject
{
    //�f�[�^
    public List<BaseWeaponStatus> baseWeaponStatuseList;

    static WeaponSpawnerSettings instance;
    public static WeaponSpawnerSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<WeaponSpawnerSettings>(nameof (WeaponSpawnerSettings));
            }

            return instance;
        }
    }

    //���X�g��ID����f�[�^����������
    public BaseWeaponStatus Get(int id , int lv)
    {
        //�w�肳�ꂽ���x���̃f�[�^�����]���o1�ԍ������x���̃f�[�^��Ԃ�
        BaseWeaponStatus ret = null;

        //�w�背�x���ƈ�v������̂��擾����
        foreach(var item in baseWeaponStatuseList)
        {

            if (id != item.Id) continue;

            //�w�背�x���ƈ�v�����ꍇ
            if(lv == item.Level)
            {
                return (BaseWeaponStatus)item.GetCopy();
            }

            //���̃f�[�^���Z�b�g����Ă��Ȃ����A����𒴂��郌�x���������������ւ���
            if(null == ret)
            {
                ret = item;
            }

            //�T���Ă��郌�x���������ŁA���b��f�[�^�������������ꍇ
            else if (item.Level < lv && ret.Level < item.Level)
            {
                ret = item;
            }
        }

        return (BaseWeaponStatus)ret.GetCopy();
    }


    //�쐬
    public BaseWeaponSpawner CreateWeaponSpawner(int id,EnemySpawnerController enemySpawnerController,Transform parent = null)
    {
        //�f�[�^���擾
        BaseWeaponStatus weaponStatus = Get(id, 1);
        //�I�u�W�F�N�g�̍쐬
        GameObject obj = Instantiate(weaponStatus.Prefab, parent);

        //�f�[�^�Z�b�g
        BaseWeaponSpawner weaponSpawner = obj.GetComponent<BaseWeaponSpawner>();
        weaponSpawner.Init(enemySpawnerController,weaponStatus);

        return weaponSpawner;
    }


}
