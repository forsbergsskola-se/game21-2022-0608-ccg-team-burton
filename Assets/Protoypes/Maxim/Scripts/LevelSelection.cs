using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
   private LevelLocked _levelLocked;

   private void Awake() => _levelLocked = GetComponent<LevelLocked>();

   public void LoadLevel()
   {
      if (_levelLocked.Unlocked == true)
      {
         SceneManager.LoadScene(gameObject.name);
      }
   }
}
