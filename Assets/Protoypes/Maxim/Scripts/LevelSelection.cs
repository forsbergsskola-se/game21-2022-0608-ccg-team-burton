using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{

   private UnlockLevel _levelLocked;
    SavingWrapper _savingWrapper;

   private void Awake()
    {
        _levelLocked = GetComponent<UnlockLevel>();
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
