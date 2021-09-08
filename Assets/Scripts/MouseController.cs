using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{   
    private Camera mainCamera;
    private PlayerInput playerInput;
    private PlayerMeeleController lastPlayerSelect;

    private void Awake()
    {
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerInput.Main.MouseButtons.performed += ctx => DetectPlayer();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void DetectPlayer()
    {
        // Get the  Coordinates where the Mouse is on the screen
        Ray ray = mainCamera.ScreenPointToRay(playerInput.Main.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);


        if (lastPlayerSelect != null)
        {
            lastPlayerSelect.DisableInput();
            lastPlayerSelect = null;
        }

        if (hits2D.collider != null)
        {
            if (hits2D.collider.gameObject.GetComponent<PlayerMeeleController>().GetCurrentState() == PlayerMeeleController.PlayerState.Ready)
            {
                hits2D.collider.gameObject.GetComponent<PlayerMeeleController>().EnableInput();
                lastPlayerSelect = hits2D.collider.gameObject.GetComponent<PlayerMeeleController>();
            }
        }
        
    }
}
