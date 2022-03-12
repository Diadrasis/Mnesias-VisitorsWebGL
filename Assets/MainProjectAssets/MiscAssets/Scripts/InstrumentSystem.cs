using UnityEngine;
//scriptable object with the info we want to show when we load the corresponding panel. (The middle ones that appear on MainPCManager)
[CreateAssetMenu(fileName = "New Instrument", menuName = "Instrument")]
public class InstrumentSystem :ScriptableObject
{
    [Tooltip("Name of the instrument")]
    public string nameInstrument;
    [Tooltip("Image of the instrument")]
    public Sprite imgInstrument;
    [Tooltip("Main text of the instrument")]
    public string mainText1,mainText2,mainText3;
    [Tooltip("Caption image of the instrument")]
    public string txtUnderImage;
}

