using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//�E�N���b�N���j���[����ScriptableProject���쐬�\
[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableProject/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    //�L�����N�^�[�f�[�^
    public List<CharacterStatus> _characterStatusList;

    static CharacterSettings _instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
            }
            return _instance;
        }
    }

    //���X�g��ID���Q�Ƃ��f�[�^����������
    public CharacterStatus Get(int id)
    {
        return (CharacterStatus)_characterStatusList.Find(item => item.Id == id).GetCopy();
    }
}









//�G�̓����̎��
public enum MoveType
{
    //�v���C���[�Ɍ������Đi��ł���
    TargetPlayer,
    //������ɐi��
    TargetDirection,
    //�����Ȃ��ň�苗���߂Â���Player�ɒe���΂�(��قǎ����\��)
}

[Serializable]
public class CharacterStatus : BaseCharacterStatus
{
    //�L�����N�^�[�̃v���n�u
    public GameObject _characterPrefab;
    //���������ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _defaultWeaponIds;
    //�����\�ȕ����ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _usableWeaponIds;
    //�����\�� = Player�ɂĎg�p����p�����[�^�[
    public int _usableWeaponMax;
    //�ړ��̃^�C�v�@= Enemy�ɂĎg�p����
    public MoveType _moveType;

    //�A�C�e����ǉ����鏈������Œǉ�����
}
