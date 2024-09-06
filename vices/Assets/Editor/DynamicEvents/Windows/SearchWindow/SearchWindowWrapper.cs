using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;

using JabberwockyWorld.DynamicEvents.Editor.Utilites;


namespace JabberwockyWorld.DynamicEvents.Editor.Windows
{
    public class SearchWindowWrapper : EditorWindow
    {
        public object Implementation = null;
        public Type _type = null;
        public Type Type
        {
            get { return _type.GetGenericArguments()[0]; }
            set
            {
                _type = typeof(SearchWindow<>).MakeGenericType(value);
                Implementation = System.Activator.CreateInstance(_type);
            }
        }

        public static SearchWindowWrapper Open<T>(T type) where T : System.Type
        {
            var window = EditorWindow.GetWindow<SearchWindowWrapper>($"{type.GetType()} Search window");
            window.DrawAsDropDown();
            window.Type = (T)type;

            return window;
        }

        public virtual void SetValues<T>(List<T> list, Action<T> action)
        {
            MethodInfo[] methodInfos = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var setValuesMethod = methodInfos.FirstOrDefault(m => m.Name == "SetValues");
            setValuesMethod.Invoke(Implementation, new object[] { list, action });
        }

        private void OnGUI()
        {
            (Implementation as ISearchWindow).OnGUI();
        }
    }
}