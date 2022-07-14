using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    [SerializeField] float _jumpForce = 4f;
    [Range(0, 1f)] [SerializeField] float _moveSmoothing = 0.36f;
    [SerializeField] bool _airControl;
    [SerializeField] LayerMask _groundMask;                          
    [SerializeField] Transform _checkGround;        
    
    float _dashForce = 10f;
    float _dashCd = 1f;

    const float _groundedRadius = 0.2f;    
    bool _isGrounded;
    Rigidbody2D _rb;
    bool _facing = true;
    Vector3 _velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();        
    }

    void FixedUpdate()
    {
        _dashCd -= Time.fixedDeltaTime;
        bool wasGrounded = _isGrounded;
        _isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkGround.position, _groundedRadius, _groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

	public void Move(float move, bool jump, bool dash)
	{
        if (_isGrounded || _airControl)
		{			
			Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);			
			_rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _moveSmoothing);			
			if (move > 0 && !_facing)
			{				
				Flip();
			}			
			else if (move < 0 && _facing)
            {
                Flip();
			}
		}
        Jump(jump);
        Dash(dash);
	}

    void Jump(bool jump)
    {
        if (_isGrounded && jump)
        {
            _isGrounded = false;
            _rb.AddForce(new Vector2(0f, _jumpForce));
        }
    }

    void Flip()
    {       
        _facing = !_facing;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Dash(bool dash)
    {
        if (dash && _dashCd <= 0)
        {
            _dashCd = 1f;
            _rb.gravityScale = 0f;            
            _rb.velocity = new Vector2(transform.localScale.x * _dashForce, 0f);
            StartCoroutine(nameof(DashCd));
        }
    }

    IEnumerator DashCd()
    {
        yield return new WaitForSeconds(0.25f);
        _rb.gravityScale = 1f;
    }
}