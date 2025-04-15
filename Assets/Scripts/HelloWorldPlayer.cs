using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        private Animator animator;

        public override void OnNetworkSpawn()
        {
            animator = GetComponent<Animator>();
            Move();
        }

        public void Move()
        {
            if (IsServer)
            {
                SubmitPositionRequestServerRpc();
            }
            else
            {
                SubmitPositionRequestOwnerRpc();
            }
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        [Rpc(SendTo.Owner)]
        void SubmitPositionRequestOwnerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            if (!IsOwner) return;

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var move = new Vector3(horizontal, 0, vertical) * 3 * Time.deltaTime;
            transform.position += move;

            // Movimiento animado
            animator?.SetFloat("PosX", horizontal);
            animator?.SetFloat("PosY", vertical);
            animator?.SetBool("isRunning", horizontal != 0 || vertical != 0);

            // Animaciones de acción
            if (Input.GetButtonDown("Jump"))
                animator?.SetTrigger("Saltar");

            if (Input.GetButtonDown("Fire3"))
                animator?.SetTrigger("Atacar");

            if (Input.GetButtonDown("Fire2"))
                animator?.SetTrigger("capoeira");

            if (IsServer)
            {
                Position.Value = transform.position;
            }
        }
    }
}
