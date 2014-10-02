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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mowali {
    public sealed class SettingsWrapper : INotifyPropertyChanged {
        private const string DATA_FILE_NAME = "data.mwl";
        
        public event PropertyChangedEventHandler PropertyChanged;

        #region Class (de)construction
        private ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private SettingsWrapper() {
            LoadDataFile();
        }

        private async void LoadDataFile() {
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.OpenIfExists);
            var json = await FileIO.ReadTextAsync(dataFile);
            JObject obj = JObject.Parse(json);
            toWatchList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["ToWatch"]);
            watchedList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["Watched"]);
        }

        public async void saveDataFile() {
            string dataJson = await JsonConvert.SerializeObjectAsync(new { ToWatch = toWatchList, Watched = watchedList });
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(dataFile, dataJson);
        }
        #endregion

        #region Instance accessor
        private static SettingsWrapper instance;

        private static object lockerObject = new object();
        public static SettingsWrapper Instance {
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
        #endregion

        #region Data accessors
        private ObservableCollection<Movie> toWatchList;
        public ObservableCollection<Movie> ToWatchList {
            get {
                return toWatchList;
            }
        }

        private ObservableCollection<Movie> watchedList;
        public ObservableCollection<Movie> WatchedList {
            get {
                return watchedList;
            }
        }
        #endregion

        private void OnPropertyChanged([CallerMemberName] string caller = "") {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
