using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    // 1. AJOUTE CETTE VARIABLE POUR LE SOLDAT
    [Header("Animation")]
    public Animator soldierAnimator; // <--- AJOUT ICI (Glisse ton objet "Soldier" ici dans l'inspecteur)

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

        // 2. ENVOIE LA VITESSE A L'ANIMATEUR
        // On calcule la force du mouvement (0 si on bouge pas, 1 si on appuie à fond)
        if (soldierAnimator != null) // On vérifie que la case n'est pas vide pour éviter les erreurs
        {
            soldierAnimator.SetFloat("Speed", _inputDirection.magnitude); // <--- AJOUT ICI
        }

        if (_jumpCount < _numberOfJumps && Input.GetButtonDown("Jump"))
            Jump();
    }

    void FixedUpdate()
    {
        // 🔹 Mouvement (dans le plan XZ), géré dans FixedUpdate pour être synchro avec la physique
        Vector3 move = _inputDirection * MoveSpeed;
        move.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = move;

        // 🔹 Rotation lissée vers la direction du mouvement
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