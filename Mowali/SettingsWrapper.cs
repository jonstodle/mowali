﻿using System;
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

        static ApplicationDataContainer roamingSettings;
        static StorageFolder localFolder;

        static ObservableCollection<Movie> toWatchList;
        static ObservableCollection<Movie> watchedList;

        #region Class initialization
        static SettingsWrapper() {
            roamingSettings = ApplicationData.Current.RoamingSettings;
            localFolder = ApplicationData.Current.LocalFolder;

            LoadDataFile();
        }

        async static void LoadDataFile() {
            var dataFile = await localFolder.GetFileAsync(DATA_FILE_NAME);
            var json = await FileIO.ReadTextAsync(dataFile);
            JObject obj = JObject.Parse(json);
            toWatchList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["ToWatch"]);
            watchedList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Movie>>((string)obj["Watched"]);
        }

        public async static void saveDataFile(){
            string dataJson = await JsonConvert.SerializeObjectAsync(new { ToWatch = toWatchList, Watched = watchedList });
            var dataFile = await localFolder.CreateFileAsync(DATA_FILE_NAME, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(dataFile, dataJson);
        }
        #endregion

        public static ObservableCollection<Movie> ToWatchList {
            get {
                return toWatchList;
            }
        }

        public static ObservableCollection<Movie> WatchedList {
            get {
                return watchedList;
            }
        }
    }
}
