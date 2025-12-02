using UnityEngine;

namespace Proselyte.OldschoolOS
{
    public class DesktopCanvasController : MonoBehaviour
    {
        [SerializeField] Canvas laptop_canvas;
        [SerializeField] PlayerSaveDataSO playerSaveDataSO;
        [SerializeField] PlayerInputDataSO playerInputDataSO;

        private RectTransform canvas_rect_transform;

        private void Start()
        {
            canvas_rect_transform = laptop_canvas.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Convert screen position to local point in canvas space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_rect_transform, playerInputDataSO.input_mouse_position, Camera.main, out Vector2 point_out);

            // Clamp to canvas rect bounds
            Rect rect = canvas_rect_transform.rect;
            float clampedX = Mathf.Clamp(point_out.x, rect.xMin, rect.xMax);
            float clampedY = Mathf.Clamp(point_out.y, rect.yMin, rect.yMax);

            Vector2 clampedPoint = new Vector2(clampedX, clampedY);

            // Save clamped position
            playerSaveDataSO.desktop_canvas_mouse_position = clampedPoint;
        }
    }
}
