using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MovementController _controller;
    float _moveSpeed = 15f;
    float _moveHor;
    bool _isJumping;
    bool _isDashing;
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _moveHor = Input.GetAxisRaw("Horizontal") * _moveSpeed;

        if (Input.GetButtonDown("Jump")) _isJumping = true;

        if (Input.GetButtonDown("Fire1")) _isDashing = true;
    }   

    void FixedUpdate()
    {
        _controller.Move(_moveHor * Time.fixedDeltaTime, _isJumping, _isDashing);
        _isJumping = false;
        _isDashing = false;
        AnimState();
    }

    void AnimState()
    {
        if(_moveHor != 0)
        {
            _anim.SetBool("Run", true);
        }
        else
        {
            _anim.SetBool("Run", false);
        }
    }
}