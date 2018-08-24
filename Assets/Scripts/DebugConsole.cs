using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using USComics_Movement;

namespace USComics_Debug {
    public class DebugConsole : MonoBehaviour {
        public TextMeshProUGUI currentMove;
        public TextMeshProUGUI previousMove;
        public TextMeshProUGUI speed;
        public TextMeshProUGUI other1;
        public TextMeshProUGUI other2;
        public TextMeshProUGUI other3;
        public TextMeshProUGUI other4;
        public TextMeshProUGUI other5;
        public TextMeshProUGUI other6;

        void Start() { }
        void Update() { }

        public void SetCurrentMove(Move inMovementType) { currentMove.text = "Current Speed: " + inMovementType.Speed + ", Direction: " + inMovementType.Direction; }
        public void SetPreviousMove(Move inMovementType) { previousMove.text = "Previous Speed: " + inMovementType.Speed + ", Direction: " + inMovementType.Direction; }
        public void SetCurrentMove(ClimbMove inMovementType) { currentMove.text = "Direction: " + inMovementType.Direction + ", Climb: " + inMovementType.Climb; }
        public void SetPreviousMove(ClimbMove inMovementType) { previousMove.text = "Direction: " + inMovementType.Direction + ", Climb: " + inMovementType.Climb; }
        public void SetOther1(string inOther) { other1.text = inOther.ToString(); }
        public void SetOther2(string inOther) { other2.text = inOther.ToString(); }
        public void SetOther3(string inOther) { other3.text = inOther.ToString(); }
        public void SetOther4(string inOther) { other4.text = inOther.ToString(); }
        public void SetOther5(string inOther) { other5.text = inOther.ToString(); }
        public void SetOther6(string inOther) { other6.text = inOther.ToString(); }
    }
}
