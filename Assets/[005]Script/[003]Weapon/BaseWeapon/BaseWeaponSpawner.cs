using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabWeapon;

    //武器データ
    public BaseWeaponStatus _weaponStatus;

    //与えた総ダメージ
    public float _toatalDamage;
    //稼働タイマー
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
