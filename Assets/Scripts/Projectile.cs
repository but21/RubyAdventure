using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // 子弹没有碰撞到物体时超出一定范围后销毁
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        _rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyControllerPlus enemy = other.gameObject.GetComponent<EnemyControllerPlus>();
        if (enemy != null)
        {
            enemy.Fix();
        }
        
        Destroy(gameObject);
    }
}