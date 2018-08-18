using UnityEngine;
using System.Collections;

namespace USComics
{
    public class MoveByTime : MonoBehaviour
    {
        public float speed = 1.0f;
        public Vector3 direction = Vector3.zero;

        void Start()
        {
            Rigidbody r = GetComponent<Rigidbody>();

            if (Vector3.zero == direction)
            {
                direction = transform.forward;
            }
            r.velocity = direction * speed;
        }
    }
}
