using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{

    public interface ITimerInterval
    {
        double? GetInterval();
    }

    public class TimeSpanInterval : ITimerInterval
    {
        TimeSpan _interval;
        public TimeSpanInterval(TimeSpan interval)
        {
            _interval = interval;
        }

        public double? GetInterval()
        {
            return _interval.TotalMilliseconds;
        }
    }

    public class FunctionInterval : ITimerInterval
    {
        private int _index = 0;
        private readonly Func<int, double> _function;
        public FunctionInterval(Func<int, double> function)
        {
            _function = function;
        }
        public double? GetInterval()
        {
            return _function(_index++);
        }
    }

    public class LoopingListTimeSpanInterval : ITimerInterval
    {
        private int index = 0;
        private readonly IEnumerable<TimeSpan> _timespans;

        public LoopingListTimeSpanInterval(IEnumerable<TimeSpan> timespans)
        {
            this._timespans = timespans;
        }

        public double? GetInterval()
        {
            index %= _timespans.Count();
            return _timespans.ElementAt(index++).TotalMilliseconds;
        }
    }

    public class ListDateTimeInterval : ITimerInterval
    {
        private int _index = 0;
        private readonly IEnumerable<DateTime> _dateTimes;

        public ListDateTimeInterval(IEnumerable<DateTime> dateTimes)
        {
            var temp = dateTimes.ToList();
            temp.Sort();
            _dateTimes = temp;
        }

        public double? GetInterval()
        {
            var nextDateTime = GetNextDatetime();
            //Make sure it is a future Value, if not go to the next one. 
            while (nextDateTime.HasValue)
            {
                var now = DateTime.Now;
                if (nextDateTime > now) return (nextDateTime - now).Value.TotalMilliseconds;
                nextDateTime = GetNextDatetime();
            }
            //If there are no more usages return null
            return null;


        }

        private DateTime? GetNextDatetime()
        {
            if (_index >= _dateTimes.Count()) return null;
            return _dateTimes.ElementAt(_index++);
        }
    }

}
