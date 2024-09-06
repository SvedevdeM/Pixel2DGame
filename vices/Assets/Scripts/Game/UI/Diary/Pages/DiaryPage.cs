using UnityEngine;

namespace Vices.Scripts
{
    public abstract class DiaryPage : MonoBehaviour
    {
        protected abstract void OpenPage();
        protected abstract void ClosePage();

        public void Open()
        {
            gameObject.SetActive(true);
            OpenPage();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            ClosePage();
        }
    }
}
