using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : BaseWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        //�u�[���������΂������������_���Ɍ��肷��
        _forward = new Vector2(Random.Range(-1f,1f),Random.Range(-1f, 1f));
        _forward.Normalize();

        //��x���肵�������Ƃ͋t�ɔ�΂�
        Vector2 force = new Vector2(-_forward.x * _weaponStatus.Speed, -_forward.y * _weaponStatus.Speed);
        _rigidbody2d.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        //��]����
        transform.Rotate(new Vector3 (0,0,5000 * Time.deltaTime));

        //�ړ�����
        _rigidbody2d.AddForce(_forward * _weaponStatus.Speed * Time.deltaTime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackEnemy(collision);
    }
}
