using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashController : BaseWeapon
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       AttackEnemy(collision);
    }
}
