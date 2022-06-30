using UnityEngine;
public interface IInteractable
{
    /// <summary>
    /// The texture to change the mouse to. Modify the get to return the value of a public field if you want it to change, or leave it as is if the mouse should be the default look.
    /// </summary>
    public Texture2D mouseTexture { get; }

    /// <summary>
    /// Implement your OnHover method. If the mouse sprite should change, access a sprite variable in the MouseControl script. LAST PART NOT IMPLEMENTED YET, MIGHT CHANGE!
    /// </summary>
    public void OnHoverEnter();

    public void OnHoverExit();

    /// <summary>
    /// Implement your OnClick method. This is called if your object is clicked, so implement any logic that should happen if a object is clicked here.
    /// </summary>
    public void OnClick(Vector3 mouseClickVector);
}