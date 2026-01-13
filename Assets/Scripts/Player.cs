using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField] private int _numberOfJumps = 2;
    
    public float MoveSpeed = 10;
    public float JumpForce = 10;

    private int _jumpCount = 0;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction *= MoveSpeed;
        direction.y = _rigidbody.linearVelocity.y;
    
        _rigidbody.linearVelocity = direction;

        if (_jumpCount < _numberOfJumps && Input.GetButtonDown("Jump"))
            Jump();
    }

    void Jump()
    {
        _jumpCount++;
        _rigidbody.AddForce(JumpForce * Vector3.up,  ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        _jumpCount = 0;
    }
}