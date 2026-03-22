using UnityEngine;

public enum UIType
{
    Text,
    Image,
    TextWithImage
}

[CreateAssetMenu(menuName = "UI/Item Info")]
public class ItemInfoData : ScriptableObject
{
    public UIType uiType;

    [TextArea]
    public string text;

    public Sprite image;
}