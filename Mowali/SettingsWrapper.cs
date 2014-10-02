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
        private const string APP_NAME = "mowali";
        private const int APP_COLOR_HEX = 900020;
        private const int APP_COLOR_R = 144;
        private const int APP_COLOR_G = 0;
        private const int APP_COLOR_B = 32;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Class (de)construction
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private SettingsWrapper() {
            LoadDataFile();
        }

        private async void LoadDataFile() {
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.OpenIfExists);
            var json = await FileIO.ReadTextAsync(dataFile);
            if(!string.IsNullOrWhiteSpace(json)) {
                JObject obj = JObject.Parse(json);
                toWatchList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["ToWatch"]);
                watchedList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["Watched"]);
            } else {
                toWatchList = new ObservableCollection<Movie>();
                watchedList = new ObservableCollection<Movie>();
            }
        }

        public async void saveDataFile() {
            string dataJson = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(new { ToWatch = toWatchList, Watched = watchedList }));
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

        #region Settings accessors
        private ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;


        #endregion

        #region Constant accessors
        public string AppName { get { return APP_NAME; } }
        public int AppColorHex { get { return APP_COLOR_HEX; } }
        public int AppColorR { get { return APP_COLOR_R; } }
        public int AppColorG { get { return APP_COLOR_G; } }
        public int AppColorB { get { return APP_COLOR_B; } }
        #endregion

        private void OnPropertyChanged([CallerMemberName] string caller = "") {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
