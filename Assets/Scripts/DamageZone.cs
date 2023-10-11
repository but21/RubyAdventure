using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private int _damageAmount = -1;


    private void FixedUpdate()
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby != null && ruby.CurrentHealth > 0)
        {
            ruby.ChangeHealth((_damageAmount));
        }
    }
}