using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USComics_Movement
{
    abstract public class AbstractMovementModule : MonoBehaviour
    {
        void Start() { }
        void Update() { }
        abstract public ModuleTypes ModuleType();
        abstract public string StartAnimation();
        abstract public string StopAnimation();
        abstract public void StartModule();
        abstract public void StopModule();
        abstract public bool IsRunning();
    }

    [System.Serializable]
    public enum ModuleTypes
    {
        None,
        Simple,
        Climbing,
        Combat
    }
}
