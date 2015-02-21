using System;
using System.Timers;

namespace Extensions
{
    public class NextTimeToRun
    {
        /*This class should work on the idea that you want it to fire at particular times
         * These Times can represented in many ways
         * set time intervals,
         * a function of time/current iteration,
         * and potentially more
         */
        public event EventHandler TimeElapsed;
        private readonly ITimerInterval _timerInterval;
        private readonly Timer _timer;

        public NextTimeToRun(ITimerInterval timerConfig)
        {
            _timer = new Timer { AutoReset = false, Enabled = false };
            _timer.Elapsed += HandleTimerElapsed;
            _timerInterval = timerConfig;
        }

        public void Start()
        {
            _timer.Start();
        }

        private void Stop()
        {
            _timer.Stop();
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (TimeElapsed != null) TimeElapsed(this, null);

            SetTimerInterval();
            
            _timer.Reset();
        }

        private void SetTimerInterval()
        {
            var interval = _timerInterval.GetInterval();
            if (interval.HasValue)
                _timer.Interval = Math.Max(1,interval.Value);
            //TODO::stops running and sits here?
        }
    }
}
