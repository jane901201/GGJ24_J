using System;

public interface IButtonController
{
    public bool Pickable { get; }
    public Action OnClick { get; set; }
    public void SetVisible(bool isValid);
}
