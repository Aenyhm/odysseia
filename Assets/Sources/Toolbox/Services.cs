using System;
using System.Collections.Generic;

namespace Sources.Toolbox {
    // Service Locator : enregistrer une instance pour pouvoir y accéder de n'importe où.
    public static class Services {
        private static readonly Dictionary<Type, object> _items = new();
        
        public static void Register<T>(T item) {
            _items.Add(typeof(T), item);
        }
        
        public static T Get<T>() {
            return (T)_items[typeof(T)];
        }
    }
}
