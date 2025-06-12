using System.Collections.Generic;
using System.IO;
using Sources.Toolbox;

namespace Sources {
    // Sérialisation + écriture et lecture + désérialisation des données persistantes.
    public static class FileStorage {

        public static void Save(in object obj, string fileName, bool append) {
            var platform = Services.Get<IPlatform>();
            
            var content = platform.Serialize(obj);
            var path = Path.Join(platform.PersistentPath, fileName);

            using (var stream = new StreamWriter(path, append)) {
                stream.WriteLine(content);
            }
        }

        public static T Load<T>(string fileName) {
            var result = default(T);
            
            var platform = Services.Get<IPlatform>();
            
            var path = Path.Join(platform.PersistentPath, fileName);
            if (File.Exists(path)) {
                using (var reader = new StreamReader(path)) {
                    var content = reader.ReadToEnd();
                    result = platform.Deserialize<T>(content);
                }
            }
            
            return result;
        }
        
        public static List<T> LoadList<T>(string fileName) {
            var result = new List<T>();
            
            var platform = Services.Get<IPlatform>();
            
            var path = Path.Join(platform.PersistentPath, fileName);
            if (File.Exists(path)) {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines) {
                    result.Add(platform.Deserialize<T>(line));
                }
            }
            
            return result;
        }
    }
}
