using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [Header("Mouvement")]
    [SerializeField] private int _numberOfJumps = 2;

    public float MoveSpeed = 10;
    public float JumpForce = 10;

    [Tooltip("Vitesse de rotation pour que le joueur regarde la direction du mouvement.")]
    public float rotationSpeed = 10f;

    private int _jumpCount = 0;
    private Vector3 _inputDirection;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(horizontal, 0f, vertical);

        if (_jumpCount < _numberOfJumps && Input.GetButtonDown("Jump"))
            Jump();
    }

    void FixedUpdate()
    {
        // 🔹 Mouvement (dans le plan XZ), géré dans FixedUpdate pour être synchro avec la physique
        Vector3 move = _inputDirection * MoveSpeed;
        move.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = move;

        // 🔹 Rotation lissée vers la direction du mouvement (toujours via le Rigidbody)
        Vector3 flatDir = new Vector3(_inputDirection.x, 0f, _inputDirection.z);
        if (flatDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDir.normalized, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(
                _rigidbody.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
            _rigidbody.MoveRotation(newRotation);
        }
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