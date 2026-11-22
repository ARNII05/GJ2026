using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputHandler : MonoBehaviour
{
    private GameControls controls;
    private Player player;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDestroy()
    {
        controls.Dispose();
    }

    private void Awake()
    {
        controls = new GameControls();
        player = GetComponent<Player>();
    }

    public void HandleInput()
    {
        if (player.NearbyObjects.actualBox != null &&
            controls.Gameplay.Interact.WasPressedThisFrame())
        {
            player.SaveBox();
            return;
        }

        if (player.NearbyObjects.truck != null &&
            controls.Gameplay.Interact.WasPressedThisFrame())
        {
            player.GiveBoxToTruck();
            return;
        }
    }
}
