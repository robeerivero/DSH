using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class movPlayer : NetworkBehaviour
{
    public float speed = 3.0f;
    public float gravity = -9.8f;
    private float velocityY = 0f;

    private CharacterController controller;
    private Animator animator;

    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (IsOwner)
        {
            var randomPos = GetRandomPositionOnPlane();
            transform.position = randomPos;
            Debug.Log("🎮 Jugador local inicializado.");
        }
    }

    void Update()
    {
        if (!IsOwner || controller == null) return;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        Vector3 move = direction * speed;

        // Gravedad
        if (controller.isGrounded)
        {
            velocityY = -1f;
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = 5f;
                animator?.SetTrigger("Saltar");
            }
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
        }

        move.y = velocityY;
        controller.Move(move * Time.deltaTime);

        // Animaciones
        animator?.SetFloat("PosX", hor);
        animator?.SetFloat("PosY", ver);
        animator?.SetBool("isRunning", hor != 0 || ver != 0);

        if (Input.GetButtonDown("Fire3"))
            animator?.SetTrigger("Atacar");

        if (Input.GetButtonDown("Fire2"))
            animator?.SetTrigger("capoeira");
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }
}
