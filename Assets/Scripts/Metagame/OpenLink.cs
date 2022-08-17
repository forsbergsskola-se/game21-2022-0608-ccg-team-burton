using UnityEngine;

namespace Metagame
{
    public class OpenLink : MonoBehaviour
    {
        [SerializeField] string url;

        public void OpenButtonLink(){
            Application.OpenURL($"{url}");
        }
    }
}