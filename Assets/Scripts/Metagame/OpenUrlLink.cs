using UnityEngine;

namespace Metagame
{
    public class OpenUrlLink : MonoBehaviour
    {
        [SerializeField] string url;

        public void OpenButtonLink(){
            Application.OpenURL($"{url}");
        }
    }
}