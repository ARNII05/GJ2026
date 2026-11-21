using UnityEngine;

public class Box : MonoBehaviour
{
    // BoxType boxType;
    public BoxType BoxType;
}

public enum BoxType
{
    Normal,
    Red,
    Green,
    Blue,
    None,
    DragonBallNormalBox,
    DragonBallGreenBox,
    DragonBallRedBox,
    DragonBallBlueBox,
    NormalDuckBox,
    DuckGreenBox,
    DuckRedBox,
    DuckBlueBox
}
