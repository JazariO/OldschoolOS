using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Proselyte.OldschoolOS
{
    public class ProgramPanelController : MonoBehaviour
    {
        [SerializeField] PlayerSaveDataSO playerSaveDataSO;
        [SerializeField] PlayerInputDataSO playerInputDataSO;
        [SerializeField] RectTransform program_panel_main_rect_transform;

        [SerializeField] Button program_panel_titlebar_handle_button;
        [SerializeField] Button program_panel_quit_button;

        private bool dragging_program_panel;
        private Vector2 drag_offset;

        private void Start()
        {
            /* NOTE(Jazz): Unity assumes I'm going to attach a monobehaviour script to each 
             * gameobject with a Button component. So we need to instead attach their EventTrigger component and
             * programmatically add callbacks from this centralized script in order to avoid disseminating monobehaviour 
             * components down through the hierarchy - which would obfuscate all the functionality deeply into the hierarchy.
             */

            // --- Titlebar Events ---
            EventTrigger titlebarEventTrigger = program_panel_titlebar_handle_button.gameObject.GetComponent<EventTrigger>();

            // PointerDown
            var downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            downEntry.callback.AddListener(HandleTitlebarPointerDown);
            titlebarEventTrigger.triggers.Add(downEntry);

            program_panel_quit_button.onClick.AddListener(HandleQuitButtonClick);
        }

        // While titlebar handle is clicked -> drag program panel around
        private void Update()
        {
            if(Mouse.current.leftButton.wasReleasedThisFrame)
            {
                HandlePointerUp();
            }


            if(dragging_program_panel)
            {
                // get canvas space mouse coordinates from input scriptable object
                program_panel_main_rect_transform.localPosition = playerSaveDataSO.desktop_canvas_mouse_position + drag_offset;
            }
        }

        private void HandleTitlebarPointerDown(BaseEventData _)
        {
            // Record offset at the moment dragging starts
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                program_panel_main_rect_transform.parent as RectTransform,
                Mouse.current.position.ReadValue(),
                Camera.main,
                out Vector2 mousePos);

            drag_offset = program_panel_main_rect_transform.localPosition - (Vector3)mousePos;
            dragging_program_panel = true;
        }

        private void HandlePointerUp()
        {
            dragging_program_panel = false;
        }

        private void HandleQuitButtonClick()
        {
            Debug.Log("Quit Button Clicked");
        }
    }
}
