using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_User_Input
{

    [System.Serializable]
    public enum Direction { None, North, East, South, West, NW, NE, SW, SE, Stop };

    public class MovementPad
    {
        // private static Rect padRect = new Rect(501.0f, 272.0f, 256.0f, 256.0f);
        private static Rect padRect = new Rect(501.0f, 20.0f, 256.0f, 256.0f);
        private static Rect attackRect = new Rect(565.0f, 100.0f, 109.0f, 109.0f);
        private static Rect northRect = new Rect(584.0f, 212.0f, 69.0f, 60.0f);
        private static Rect southRect = new Rect(584.0f, 31.0f, 69.0f, 60.0f);
        private static Rect eastRect = new Rect(675.0f, 120.0f, 61.0f, 64.0f);
        private static Rect westRect = new Rect(501.0f, 120.0f, 61.0f, 64.0f);

        public static Direction GetDirection()
        {
            if (!Input.GetMouseButtonDown(0)) { return Direction.None; }
            Vector2 mousePosition = Input.mousePosition;
            if (!padRect.Contains(mousePosition)) return Direction.None;
            Direction direction = Direction.None;
            if (MovementPad.attackRect.Contains(mousePosition)) { direction = Direction.Stop; }
            else if (MovementPad.northRect.Contains(mousePosition)) { direction = Direction.North; }
            else if (MovementPad.southRect.Contains(mousePosition)) { direction = Direction.South; }
            else if (MovementPad.eastRect.Contains(mousePosition)) { direction = Direction.East; }
            else if (MovementPad.westRect.Contains(mousePosition)) { direction = Direction.West; }
            else
            {
                float diameterX = MovementPad.padRect.width / 2;
                float diameterY = MovementPad.padRect.height / 2;
                if (mousePosition.x < MovementPad.padRect.x + diameterX)
                {
                    if (mousePosition.y < MovementPad.padRect.y + diameterY) { direction = Direction.SW; }
                    else { direction = Direction.NW; }
                }
                else
                {
                    if (mousePosition.y < MovementPad.padRect.y + diameterY) { direction = Direction.SE; }
                    else { direction = Direction.NE; }
                }
            }
            return direction;
        }

        public static Vector3 GetIndicatorPosition(Direction inDirection, Vector2 inMovementPadIndicatorOriginalPosition)
        {
            Vector2 position = inMovementPadIndicatorOriginalPosition;
            if (Direction.North == inDirection) { position.y += 120; }
            else if (Direction.South == inDirection) { position.y -= 120; }
            else if (Direction.East == inDirection) { position.x += 120; }
            else if (Direction.West == inDirection) { position.x -= 120; }
            else if (Direction.NE == inDirection) { position.x += 90; position.y += 90; }
            else if (Direction.NW == inDirection) { position.x -= 90; position.y += 90; }
            else if (Direction.SE == inDirection) { position.x += 90; position.y -= 90; }
            else if (Direction.SW == inDirection) { position.x -= 90; position.y -= 90; }
            return new Vector3(position.x, position.y, 0);
        }

        public static Vector3 ConvertDirectionToVector(Direction direction, Vector3 previousVector)
        {
            if (Direction.Stop == direction) return Vector3.zero;
            if (Direction.North == direction) return Vector3.forward;
            if (Direction.South == direction) return -Vector3.forward;
            if (Direction.East == direction) return -Vector3.left;
            if (Direction.West == direction) return Vector3.left;
            if (Direction.NW == direction) return new Vector3(-0.66f, 0.0f, 0.66f);
            if (Direction.NE == direction) return new Vector3(0.66f, 0.0f, 0.66f);
            if (Direction.SW == direction) return new Vector3(-0.66f, 0.0f, -0.66f);
            if (Direction.SE == direction) return new Vector3(0.66f, 0.0f, -0.66f);
            if (Direction.None == direction) return previousVector;
            return Vector3.zero;
        }

    }
}
