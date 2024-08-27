using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    InputAction move, rotate, shoot;
    public Animator animator;
    public GameObject bulletPrefab;  // Reference to the bullet prefab
    public Transform bulletSpawnPoint;  // Point from where the bullet will be fired
    private void Awake()
    {
        InputActionsGojo inputMap = new InputActionsGojo();
        move = inputMap.Gojo.Move;
        rotate = inputMap.Gojo.Rotate;
        shoot = inputMap.Gojo.Shoot;
    }

    private void OnEnable()
    {
        move.Enable();
        rotate.Enable();
        shoot.Enable();

        shoot.performed += OnShoot;  // Listen for the shoot action
    }

    private void OnDisable()
    {
        move.Disable();
        rotate.Disable();

        shoot.performed -= OnShoot;  // Unsubscribe to avoid memory leaks
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        
        transform.Translate(0, 0,
        move.ReadValue<float>() * 5 * Time.deltaTime);
        transform.Rotate(0,
            rotate.ReadValue<float>() * 180 * Time.deltaTime, 0);

        if (move.ReadValue<float>() != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        
        
        
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        ShootBulletServerRpc();  // Trigger the bullet spawn on the server
    }
    [ClientRpc]
    public void PontuarClientRpc()
    {
        if(IsOwner)
        {
            UIManager.instance.RaiseScore();
        }
    }
    

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        NetworkObject networkObject = bulletInstance.GetComponent<NetworkObject>();

        //if (networkObject != null)
        {
            bulletInstance.GetComponent<Bullet>().playerGameObject = gameObject;
            networkObject.Spawn();  // Spawn bullet across the network
        }
    }
    }
