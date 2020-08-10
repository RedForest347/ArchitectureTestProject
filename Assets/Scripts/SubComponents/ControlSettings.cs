using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ControlSettings", menuName = "Subcomponents/ControlSettings", order = 51)]
public class ControlSettings : ScriptableObject
{
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Rigt;
    public KeyCode Boost;
    public KeyCode RollLeft;
    public KeyCode RollRigt;
}
