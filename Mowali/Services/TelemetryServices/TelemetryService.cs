using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mowali.Services.TelemetryServices
{
	public class TelemetryService : ITelemetryService
	{
		public static TelemetryService Instance { get; }
		static TelemetryService()
		{
			Instance = Instance ?? new TelemetryService();
		}



		private TelemetryClient client = new TelemetryClient();

		public void LogLifecycleEvent(string eventType)
		{
			var properties = new Dictionary<string, string>
			{
				{"EventType", eventType }
			};

			client.TrackEvent("LifecycleEvent", properties);
		}

		public void LogUserAction(string actionType)
		{
			var properties = new Dictionary<string, string>
			{
				{"ActionType", actionType }
			};

			client.TrackEvent("UserAction", properties);
		}

		public void LogPageView(string pageName)
		{
			client.TrackPageView(pageName);
		}

		public void LogException(string parentContainer, string failedAction, Exception exception)
		{
			var properties = new Dictionary<string, string>
			{
				{"ParentContainer", parentContainer },
				{"FailedAction", failedAction }
			};

			client.TrackException(exception, properties);
		}
	}
}
