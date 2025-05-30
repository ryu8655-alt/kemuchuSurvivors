using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : BaseWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        //角度の変換を行う
        float angle = Mathf.Atan2(_forward.y, _forward.x) * Mathf.Rad2Deg;
        //角度を代入
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody2d.position += _forward.normalized * _weaponStatus.Speed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackEnemy(collision);
    }
}
