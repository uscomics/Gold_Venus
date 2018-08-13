using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using USComics_User_Input;
using USComics_Vision;

namespace USComics_Debug
{

    public class DebugConsole : MonoBehaviour
    {
        public TextMeshProUGUI movementType;
        public TextMeshProUGUI currentMovementType;
        public TextMeshProUGUI speed;
        public TextMeshProUGUI direction;
        public TextMeshProUGUI other1;
        public TextMeshProUGUI other2;
        public TextMeshProUGUI other3;
        public TextMeshProUGUI other4;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCurrentMovementType(MovementType inMovementType) { currentMovementType.text = "Current Movement Type: " + inMovementType.ToString(); }
        public void SetMovementType(MovementType inMovementType) { movementType.text = "Movement Type: " + inMovementType.ToString(); }
        public void SetSpeed(float inSpeed) { speed.text = "Speed: " + inSpeed.ToString(); }
        public void SetDirection(Direction inDirection) { direction.text = "Direction: " + inDirection.ToString(); }
        public void SetOther1(string inOther) { other1.text = inOther.ToString(); }
        public void SetOther2(string inOther) { other2.text = inOther.ToString(); }
        public void SetOther3(string inOther) { other3.text = inOther.ToString(); }
        public void SetOther4(string inOther) { other4.text = inOther.ToString(); }
    }
}
