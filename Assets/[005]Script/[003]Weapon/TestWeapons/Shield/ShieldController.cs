using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;

public class ShieldController : BaseWeapon
{

    //�v���C���[����̋���
    const float DISTANCE = 1.0f;
    //���݂̊p�x
    public float _angle;

    // Start is called before the first frame update
    void Start()
    {
        //�o�����̃A�j���[�V����
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(1,1,1),1.5f).SetEase(Ease.OutBounce);


    }

    // Update is called once per frame
    void Update()
    {
        _angle  -=_weaponStatus.Speed * Time.deltaTime;

        //Cos�֐��Ƀ��W�A���p���w�肵�AX���W���擾�ADISTANCE����Z���ă��[���h���W�ɕϊ�����
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * DISTANCE;
        //Sin�֐��Ƀ��W�A���p���w�肵�AY���W���擾�ADISTANCE����Z���ă��[���h���W�ɕϊ�����
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * DISTANCE;

        //�|�W�V�����X�V
        transform.position = transform.root.position + new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�G�ȊO�͖�������
        if(!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //���Ε����֓G�𒵂˕Ԃ��E�����o��
        Vector3 foward = enemy.transform.position - transform.position;
        enemy.GetComponent<Rigidbody2D>().AddForce(foward.normalized * 5);

        //�G�ւ̃_���[�W����
        AttackEnemy(collision);
    }
}
