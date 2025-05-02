using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���ɓo�ꂷ��S�L�����N�^�[�̃X�e�[�^�X�����Ǘ�����scriptableObject
/// ID���w�肷�邱�ƂŁA�L�����N�^�[�̃X�e�[�^�X���擾����
/// </summary>
[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableProject/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    [Tooltip("�L�����N�^�[�̃X�e�[�^�X�f�[�^���X�g")]
    public List<CharacterStatus> _characterStatusList;

    private static CharacterSettings _instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
                if(_instance == null)
                {
                    Debug.LogError("[CharacterSettings] Resources�t�H���_����CharacterSettings.asset��������܂���");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// �w�肵�� ID �Ɉ�v����L�����N�^�[�X�e�[�^�X���R�s�[���ĕԂ�
    /// </summary>
    /// <param name="id">�L�����N�^�[ID</param>
    /// <returns>�Y������ CharacterStatus �̃R�s�[�B�Ȃ���� null�B</returns>aram>
    /// <returns></returns>
    public CharacterStatus Get(int id)
    {
        var foundStatus = _characterStatusList.Find(item => item.Id == id);
        if(foundStatus == null)
        {
            Debug.LogError($"[CharacterSettings] ID {id} �̃L�����N�^�[��������܂���");
            return null;
        }

        return (CharacterStatus)foundStatus.GetCopy();


        // return (CharacterStatus)_characterStatusList.Find(item => item.Id == id).GetCopy();
    }
}









/// <summary>
/// �G�̈ړ��^�C�v���`����񋓌^
/// </summary>
public enum MoveType
{
    //�v���C���[�p
    Player,
    //�v���C���[�Ɍ������Đi��ł���
    TargetPlayer,
    //������ɐi��
    TargetDirection,
    //�����Ȃ��ň�苗���߂Â���Player�ɒe���΂�(��قǎ����\��)
    ShootOnSight

}

/// <summary>
/// �v���C���[�E�G���ʂŎg���L�����N�^�[�X�e�[�^�X�f�[�^�\���B
/// ScriptableObject ���Ŏg�p�����B
/// </summary>
[Serializable]
public class CharacterStatus : BaseCharacterStatus
{
    [Header("�L�����N�^�[�v���n�u")]
    public GameObject _characterPrefab;

    [Header("�v���C���[���p�����[�^�[")]
    //���������ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _defaultWeaponIds;
    //�����\�ȕ����ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _usableWeaponIds;
    //�����\�� = Player�ɂĎg�p����p�����[�^�[
    public int _usableWeaponMax;

    [Header("�G���p�����[�^�[")]
    //�ړ��̃^�C�v�@= Enemy�ɂĎg�p����
    public MoveType _moveType;

    //�A�C�e����ǉ����鏈������Œǉ�����
}
