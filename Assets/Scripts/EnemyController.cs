using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public float moveTime = 10.0f;
    public bool vertical;

    private float _moveTimer;
    private bool _isUp = true;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _position;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _moveTimer = moveTime;
    }


    private void Update()
    {
        _moveTimer -= Time.deltaTime;
        if (_moveTimer < 0)
        {
            _isUp = !_isUp;
            _moveTimer = moveTime;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _position = transform.position;

        if (vertical)
        {
            if (_isUp)
            {
                _position.y += speed * Time.deltaTime;
            }
            else
            {
                _position.y -= speed * Time.deltaTime;
            }
        }
        else
        {
            if (_isUp)
            {
                _position.x += speed * Time.deltaTime;
            }
            else
            {
                _position.x -= speed * Time.deltaTime;
            }
        }


        _rigidbody2D.MovePosition(_position);
    }
}