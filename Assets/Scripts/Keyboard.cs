using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USComics_Vision;

namespace USComics_User_Input
{
    public class Keyboard
    {
        public static bool forceStop = false;

        public static Direction GetDirection()
        {
            if (Keyboard.forceStop)
            {
                Keyboard.forceStop = false;
                return Direction.Stop;
            }
            // W v: +, h: 0 (UP)
            // A v: 0, h: - (LEFT)
            // S v: -, h: 0 (DOWN)
            // D v: 0, h: + (RIGHT)
            float vert = Input.GetAxis("Vertical");
            float horz = Input.GetAxis("Horizontal");
            if (0 == vert && 0 == horz)
            {
                if (Input.GetKeyDown(KeyCode.Backspace)) return Direction.Stop;
                if (Input.GetKeyDown(KeyCode.Delete)) return Direction.Stop;
                return Direction.None;
            }
            if (vert < 0) return Direction.South;
            if (vert > 0) return Direction.North;
            if (horz < 0) return Direction.West;
            return Direction.East;
        }
        public static MovementType GetMovementType()
        {
            // ESC = Cancel (WALKING)
            // ENTER/RETURN = Submit (RUNNING)
            // SHIFT = (SNEAK)
            if (0 < Input.GetAxis("Cancel")) return MovementType.Walking;
            if (0 < Input.GetAxis("Submit")) return MovementType.Running;
            if (Input.GetKeyDown(KeyCode.RightShift)) return MovementType.Sneaking;
            if (Input.GetKeyDown(KeyCode.LeftShift)) return MovementType.Sneaking;
            if (Input.GetKeyDown(KeyCode.C)) return MovementType.Climbing;
            return MovementType.None;
        }
    }
}
