using System.Linq;
using UnityEngine;

//Dont want library in scene
public class ItemLibrary : MonoBehaviour
{
    public  LibrarySO ItemLibrarySos;
    
    public ActionItem GetGemFromLibrary(string gemID)
    {
        //TODO: If dictionary would be nice
        foreach (var gemSo in ItemLibrarySos.GemLibrary.Where(gemSo => gemSo.GetItemID() == gemID))
        {
            return gemSo;
        }
    
        Debug.Log("Warning: No gem was found with that ID in library. Returning Null");
        return null; 
    }
    
    
    
}
