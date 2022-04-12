using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace HomeWork_25
{
    class LoadSave
    {

        public static ObservableCollection<IClient> LoadDB(string Path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            ObservableCollection<IClient> clients = new ObservableCollection<IClient>();

            using (StreamReader streamReader = new StreamReader(Path))
            {
                string text = streamReader.ReadToEnd();
                clients = JsonConvert.DeserializeObject<ObservableCollection<IClient>>(text, settings);
            }

            return clients;
        }

        public static void SaveDB(string Path, ObservableCollection<IClient> clients)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
                
            };
            
            using (StreamWriter streamWriter = new StreamWriter(Path))
            {
                string text = JsonConvert.SerializeObject(clients, Formatting.Indented, settings);
                streamWriter.WriteLine(text);
            }
        }
    }
}
