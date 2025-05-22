using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : BaseWeapon
{
  
    // Update is called once per frame
    void Update()
    {
        //í‚É©g‚ğ‰ñ“]‚³‚¹‚é
        transform.Rotate(new Vector3(0, 0, -1000 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackEnemy(collision);
    }
}
