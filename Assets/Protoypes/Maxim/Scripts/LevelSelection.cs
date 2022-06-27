using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
   private UnlockLevel _levelLocked;

   private void Awake() => _levelLocked = GetComponent<UnlockLevel>();

   public void LoadLevel()
   {
      if (_levelLocked.Unlocked == true)
      {
         SceneManager.LoadScene(gameObject.name);
      }
   }
}
