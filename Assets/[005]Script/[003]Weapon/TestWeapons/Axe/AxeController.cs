using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : BaseWeapon
{
  
    // Update is called once per frame
    void Update()
    {
        //常に自身を回転させる
        transform.Rotate(new Vector3(0, 0, -1000 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackEnemy(collision);
    }
}
