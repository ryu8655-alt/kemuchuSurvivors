using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabWeapon;

    //����f�[�^
    public BaseWeaponStatus _weaponStatus;

    //�^�������_���[�W
    public float _toatalDamage;
    //�ғ��^�C�}�[
    public float _totalTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
