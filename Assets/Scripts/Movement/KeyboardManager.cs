using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement
{
    public class KeyboardManager : MonoBehaviour {
        public DirectionQueue directionBuffer = new DirectionQueue();
        public MovementQueue movementBuffer = new MovementQueue();

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public DirectionType GetDirection()
        {
            if (0 < directionBuffer.queue.Count)
            {
                BufferedDirection bufferedDirection = directionBuffer.queue.Peek() as BufferedDirection;
                if ((Time.realtimeSinceStartup >= directionBuffer.lastEventTime + bufferedDirection.delay)
                || (0 == bufferedDirection.delay))
                {
                    directionBuffer.queue.Dequeue();
                    directionBuffer.lastEventTime = Time.realtimeSinceStartup;
                    return bufferedDirection.direction;
                }
            }
            // W v: +, h: 0 (UP)
            // A v: 0, h: - (LEFT)
            // S v: -, h: 0 (DOWN)
            // D v: 0, h: + (RIGHT)
            float vert = Input.GetAxis("Vertical");
            float horz = Input.GetAxis("Horizontal");
            if (0 == vert && 0 == horz)
            {
                if (Input.GetKeyDown(KeyCode.Backspace)) return DirectionType.Stop;
                if (Input.GetKeyDown(KeyCode.Delete)) return DirectionType.Stop;
                return DirectionType.None;
            }
            if (vert < 0) return DirectionType.South;
            if (vert > 0) return DirectionType.North;
            if (horz < 0) return DirectionType.West;
            return DirectionType.East;
        }
        public MovementType GetMovementType()
        {
            if (0 < movementBuffer.queue.Count)
            {
                BufferedMovement bufferedMovement = movementBuffer.queue.Peek() as BufferedMovement;
                if ((Time.realtimeSinceStartup >= directionBuffer.lastEventTime + bufferedMovement.delay)
                || (0 == bufferedMovement.delay))
                {
                    directionBuffer.queue.Dequeue();
                    directionBuffer.lastEventTime = Time.realtimeSinceStartup;
                    return bufferedMovement.movement;
                }
            }
            // ESC = Cancel (WALKING)
            // ENTER/RETURN = Submit (RUNNING)
            // SHIFT = (SNEAK)
            if (0 < Input.GetAxis("Cancel")) return MovementType.Walking;
            if (0 < Input.GetAxis("Submit")) return MovementType.Running;
            if (Input.GetKeyDown(KeyCode.RightShift)) return MovementType.Sneaking;
            if (Input.GetKeyDown(KeyCode.LeftShift)) return MovementType.Sneaking;
            if (Input.GetKeyDown(KeyCode.RightAlt)) return MovementType.Standing;
            if (Input.GetKeyDown(KeyCode.LeftAlt)) return MovementType.Standing;
            return MovementType.None;
        }
    }
    [System.Serializable]
    public class BufferedDirection
    {
        public BufferedDirection(DirectionType inDirection, float inDelay = 0.0f)
        {
            direction = inDirection;
            delay = inDelay;
        }
        public DirectionType direction;
        public float delay = 0.0f;
    }

    [System.Serializable]
    public class BufferedMovement
    {
        public BufferedMovement(MovementType inMovement, float inDelay = 0.0f)
        {
            movement = inMovement;
            delay = inDelay;
        }
        public MovementType movement;
        public float delay = 0.0f;
    }

    [System.Serializable]
    public class DirectionQueue
    {
        public Queue queue = new Queue();
        public float lastEventTime = 0.0f;
    }

    [System.Serializable]
    public class MovementQueue
    {
        public Queue queue = new Queue();
        public float lastEventTime = 0.0f;
    }
}

