using UnityEngine;


namespace JabberwockyWorld.DynamicEvents.Scripts.Data
{
    public class test : MonoBehaviour
    {
        public EventDatabase data;
        private void Start()
        {
            InitializeEntries();
        }
        public void InitializeEntries()
        {
            for (int i = 0; i < data.EventTables.Count; i++)
            {
                var eventTable = data.EventTables[i];

                for (int j = 0; j < eventTable.Events.Count; j++)
                {
                  //  eventTable.Events[j].Initialize(this);
                         eventTable.Events[j].Usages = 0;
                }

                for (int j = 0; j < eventTable.Rules.Count; j++)
                {
                   // eventTable.Rules[j].Initialize(this);
                         eventTable.Rules[j].Usages = 0;
                }

                //TODO: Think about what actually facts need have 
                for (int j = 0; j < eventTable.Facts.Count; j++)
                {
                   // eventTable.Facts[j].Initialize(this);
                            eventTable.Facts[j].Value = 0;
                }
            }
        }
    }
}