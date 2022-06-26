using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
   private LevelLocked _levelLocked;
    SavingWrapper _savingWrapper;

   private void Awake()
    {
        _levelLocked = GetComponent<LevelLocked>();
        _savingWrapper = GetComponent<SavingWrapper>();
    }

   public void LoadLevel()
   {
      if (_levelLocked.Unlocked == true)
      {
         SceneManager.LoadScene(gameObject.name);
         _savingWrapper.Load();
      }
   }
}
