using System;
using System.Diagnostics;
using System.Reflection;

namespace WSW.Performance
{
    public sealed class MetricTracker : IDisposable
    {
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;

        private MetricTracker(string methodName)
        {
            _methodName = methodName;
            _stopwatch = Stopwatch.StartNew();
        }

        void IDisposable.Dispose()
        {
            _stopwatch.Stop();
            LogMetrics();
        }

        private void LogMetrics() => Console.WriteLine(_methodName + @" : " + _stopwatch.Elapsed.TotalMilliseconds);

        public static MetricTracker Track(MethodBase mb)
        {
            return new MetricTracker(mb.Name);
        }
    }
}