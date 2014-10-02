using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Mowali {
    public static class Extensions {
        public async static void SetAppTitle(this StatusBar sb) {
            sb.BackgroundColor = Color.FromArgb(255, (byte)SettingsWrapper.Instance.AppColorR, (byte)SettingsWrapper.Instance.AppColorG, (byte)SettingsWrapper.Instance.AppColorB);
            sb.BackgroundOpacity = 1;
            var progInd = sb.ProgressIndicator;
            progInd.Text = SettingsWrapper.Instance.AppName;
            progInd.ProgressValue = 0;
            await progInd.ShowAsync();
        }
    }
}
