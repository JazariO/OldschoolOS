using UnityEngine;

namespace Proselyte.OldschoolOS
{
    [CreateAssetMenu(fileName = "PlayerInputDataSO", menuName = "Proselyte / OldschoolOS / Player Input Data SO")]
    public class PlayerInputDataSO : ScriptableObject
    {
        public Vector2 input_move;
        public Vector2 input_look;
        public bool input_interact;
        public bool input_change_view;

        public Vector2 input_mouse_position;
        public bool input_mouse_button_left;
        public bool input_mouse_button_right;
    }
}
