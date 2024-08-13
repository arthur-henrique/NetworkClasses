using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction move, rotate;

    private void Awake()
    {
        InputActionsGojo inputMap = new InputActionsGojo();
        move = inputMap.Gojo.Move;
        rotate = inputMap.Gojo.Rotate;
    }

    private void OnEnable()
    {
        move.Enable();
        rotate.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        rotate.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0,
            move.ReadValue<float>() * 5 * Time.deltaTime);
        transform.Rotate(0,
            rotate.ReadValue<float>() * 180 * Time.deltaTime, 0);
    }
}
