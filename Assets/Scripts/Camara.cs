using UnityEngine;
using Unity.Netcode;

public class AsignarCamaraLocal : NetworkBehaviour
{
    public Transform camaraPivot; // Lugar donde se posiciona la cámara (puede ser el jugador)

    private void Start()
    {
        // Solo queremos hacerlo si somos el jugador local
        if (!IsOwner) return;

        // Buscar la cámara principal
        Camera mainCam = Camera.main;
        if (mainCam != null && camaraPivot != null)
        {
            // Puedes ajustar el offset aquí o hacerlo con un script de seguimiento
            mainCam.transform.SetParent(camaraPivot);
            mainCam.transform.localPosition = new Vector3(0, 5, -7);
            mainCam.transform.localRotation = Quaternion.Euler(20f, 0f, 0f);
        }
        else
        {
            Debug.LogWarning("No se encontró la cámara principal o el camaraPivot no está asignado.");
        }
    }
}
