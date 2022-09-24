using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.GetComponent<Health>() != null)
         {
             Health health = col.GetComponent<Health>();
             health.Damage(damage);
         }
     }
}
