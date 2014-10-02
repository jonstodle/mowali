using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using TmdbWrapper;
using TmdbWrapper.Movies;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mowali {
    public sealed class SettingsWrapper {
        const string DATA_FILE_NAME = "data.mwl";

        static SettingsWrapper instance;

        ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        ObservableCollection<Movie> toWatchList;
        ObservableCollection<Movie> watchedList;

        static object lockerObject = new object();
        public SettingsWrapper Instance {
            get {
                if(instance == null) {
                    lock(lockerObject) {
                        if(instance == null) {
                            instance = new SettingsWrapper();
                        }
                    }
                }
                return instance;
            }
        }

        #region Class initialization
        SettingsWrapper() {
            LoadDataFile();
        }

        async void LoadDataFile() {
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.OpenIfExists);
            var json = await FileIO.ReadTextAsync(dataFile);
            JObject obj = JObject.Parse(json);
            toWatchList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["ToWatch"]);
            watchedList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["Watched"]);
        }

        public async void saveDataFile(){
            string dataJson = await JsonConvert.SerializeObjectAsync(new { ToWatch = toWatchList, Watched = watchedList });
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(dataFile, dataJson);
        }
        #endregion

        public ObservableCollection<Movie> ToWatchList {
            get {
                return toWatchList;
            }
        }

        public ObservableCollection<Movie> WatchedList {
            get {
                return watchedList;
            }
        }
    }
}
