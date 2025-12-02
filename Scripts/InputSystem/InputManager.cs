using UnityEngine;
using UnityEngine.InputSystem;

namespace Proselyte.OldschoolOS
{
    public class InputManager : MonoBehaviour
    {
        private static bool isActive;

        [Header("Outgoing Variables")]
        [SerializeField] PlayerInputDataSO playerInputDataSO;

        [Header("References")]
        [SerializeField] InputActionAsset actions;
        [SerializeField] PlayerInput playerInput;

        private InputActionMap _playerMap;
        private InputActionMap _uiMap;

        private InputAction _interactAction;

        private void Awake()
        {
            if(!InputManager.isActive)
                InputManager.isActive = true;
            else
                return;

            // Set initial defaults
            playerInputDataSO.input_interact = false;
            playerInputDataSO.input_mouse_position = Vector2.zero;
            playerInputDataSO.input_mouse_button_left = false;
            playerInputDataSO.input_mouse_button_right = false;

            // Init action maps
            _playerMap = playerInput.actions.FindActionMap("Player", true);
            _uiMap = playerInput.actions.FindActionMap("UI", true);

            // Init player actions
            _interactAction = playerInput.actions["Interact"];
        }

        private void OnEnable()
        {
            if(!InputManager.isActive)
            {
                gameObject.SetActive(false);
                return;
            }

            if(_playerMap == null && _uiMap == null) return;
            _playerMap.Enable();
            _uiMap.Disable();
        }

        private void OnDisable()
        {
            if(InputManager.isActive) return;

            if(_playerMap == null && _uiMap == null) return;
            _uiMap.Disable();
            _playerMap.Disable();
        }

        private void Start()
        {
            if(InputManager.isActive) return;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            playerInputDataSO.input_interact = _interactAction.WasPressedThisFrame();

            playerInputDataSO.input_mouse_position = Mouse.current.position.ReadValue();
            playerInputDataSO.input_mouse_button_left = Mouse.current.leftButton.wasPressedThisFrame;
            playerInputDataSO.input_mouse_button_right = Mouse.current.rightButton.wasPressedThisFrame;
        }
    }
}
