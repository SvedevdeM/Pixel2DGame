using UnityEngine;

namespace Vices.Scripts
{
    public class DiaryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _diaryHolder;
        [SerializeField] private InventoryPage _inventoryPage;
        [SerializeField] private QuestPage _questPage;
        [SerializeField] private MapPage _mapPage;

        private void Start()
        {
            Close();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M)) OpenMap();
            if(Input.GetKeyDown(KeyCode.I)) OpenInventory();
            if(Input.GetKeyDown(KeyCode.Q)) OpenQuests();
        }

        public void OpenInventory()
        {
            _diaryHolder.SetActive(true);
            _inventoryPage.Open();
            _questPage.Close();
            _mapPage.Close();
        }

        public void OpenMap()
        {
            _diaryHolder.SetActive(true);
            _inventoryPage.Close();
            _questPage.Close();
            _mapPage.Open();
        }

        public void OpenQuests()
        {
            _diaryHolder.SetActive(true);
            _inventoryPage.Close();
            _questPage.Open();
            _mapPage.Close();
        }

        public void Close()
        {
            _diaryHolder.SetActive(false);
        }
    }
}
