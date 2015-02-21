using System.Timers;

namespace Extensions
{
    public static class TimerExtension
    {
        public static void Stop(this Timer timer)
        {
            timer.Enabled = false;
            timer.Stop();
        }

        public static void Start(this Timer timer)
        {
            timer.Enabled = true;
            timer.Start();
        }

        public static void Reset(this Timer timer)
        {
            if (timer.Enabled)
                timer.Start();
        }
    }
}
