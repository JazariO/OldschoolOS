using System.Collections.Generic;
using UnityEngine;

namespace Proselyte.OldschoolOS
{
    [CreateAssetMenu(fileName = "PlayerSaveDataSO", menuName = "Proselyte / OldschoolOS / Player Save Data SO")]
    public class PlayerSaveDataSO : ScriptableObject
    {
        public Vector3 position;
        public Vector3 cameraPosition;
        public Quaternion cameraRotation;
        public bool isInspecting;

        public byte[] photo_taken_bytes;

        public Vector2 desktop_canvas_mouse_position;
    }
}
