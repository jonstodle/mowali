using System;

namespace Mowali.Services.Telemetry
{
	public interface ITelemetryService
	{
		void LogLifecycleEvent(string eventType);
		void LogUserAction(string actionType);
		void LogPageView(string pageName);
		void LogException(string parentContainer, string failedAction, Exception exception);
	}
}