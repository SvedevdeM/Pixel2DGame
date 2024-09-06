using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;

using JabberwockyWorld.DynamicEvents.Scripts.Entries;
using JabberwockyWorld.DynamicEvents.Editor.Utilites;


namespace JabberwockyWorld.DynamicEvents.Editor.Windows
{
    public class SearchWindowBaseEntryWrapper : EditorWindow
    {
        public object Implementation = null;
        public Type _type = null;
        public Type Type
        {
            get { return _type.GetGenericArguments()[0]; }
            set
            {
                _type = typeof(SearchWindowBaseEntry);
                Implementation = System.Activator.CreateInstance(_type);
            }
        }

        public static SearchWindowBaseEntryWrapper Open<T>(T type) where T : Type
        {
            var window = GetWindow<SearchWindowBaseEntryWrapper>($"{type.GetType()} Search window");
            window.DrawAsDropDown();
            window.Type = (T)type;

            return window;
        }

        public virtual void SetValues<T>(List<T> list, Action<BaseEntry> action)
        {
            //Convert T into BaseEntry. For cases in which given FactEntry/EventEntry/RuleEntry
            var baseEntries = new List<BaseEntry>();
            foreach (var item in list)
            {
                var baseEntry = item as BaseEntry;
                baseEntries.Add(baseEntry);
            }

            MethodInfo[] methodInfos = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var setValuesMethod = methodInfos.FirstOrDefault(m => m.Name == "SetValues");
            setValuesMethod.Invoke(Implementation, new object[] { baseEntries, action });
        }

        private void OnGUI()
        {
            (Implementation as ISearchWindow).OnGUI();
        }
    }
}