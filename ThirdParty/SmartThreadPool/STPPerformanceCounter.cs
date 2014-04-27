using System;
using System.Diagnostics;

namespace Amib.Threading
{
    public interface ISTPPerformanceCountersReader
    {
        long ActiveThreads { get; }

        long InUseThreads { get; }
        long WorkItemsProcessed { get; }

        long WorkItemsQueued { get; }
    }
}

namespace Amib.Threading.Internal
{
    internal enum STPPerformanceCounterType
    {
        // Fields
        ActiveThreads = 0,

        InUseThreads = 1,
        OverheadThreads = 2,
        OverheadThreadsPercent = 3,
        OverheadThreadsPercentBase = 4,

        WorkItems = 5,
        WorkItemsInQueue = 6,
        WorkItemsProcessed = 7,

        WorkItemsQueuedPerSecond = 8,
        WorkItemsProcessedPerSecond = 9,

        AvgWorkItemWaitTime = 10,
        AvgWorkItemWaitTimeBase = 11,

        AvgWorkItemProcessTime = 12,
        AvgWorkItemProcessTimeBase = 13,

        WorkItemsGroups = 14,

        LastCounter = 14,
    }

    internal interface ISTPInstancePerformanceCounters : IDisposable
    {
        void Close();

        void SampleThreads(long activeThreads, long inUseThreads);

        void SampleWorkItems(long workItemsQueued, long workItemsProcessed);

        void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime);

        void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime);
    }

#if !(_WINDOWS_CE) && !(_SILVERLIGHT) && !(WINDOWS_PHONE)
    internal class LocalSTPInstancePerformanceCounters : ISTPInstancePerformanceCounters, ISTPPerformanceCountersReader
    {
        private long _activeThreads;

        private long _inUseThreads;

        private long _workItemsProcessed;

        private long _workItemsQueued;

        public long ActiveThreads
        {
            get { return _activeThreads; }
        }

        public long InUseThreads
        {
            get { return _inUseThreads; }
        }

        public long WorkItemsProcessed
        {
            get { return _workItemsProcessed; }
        }

        public long WorkItemsQueued
        {
            get { return _workItemsQueued; }
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }
        public void SampleThreads(long activeThreads, long inUseThreads)
        {
            _activeThreads = activeThreads;
            _inUseThreads = inUseThreads;
        }

        public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
        {
            _workItemsQueued = workItemsQueued;
            _workItemsProcessed = workItemsProcessed;
        }

        public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
        {
            // Not supported
        }

        public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
        {
            // Not supported
        }
    }

    internal class NullSTPInstancePerformanceCounters : ISTPInstancePerformanceCounters, ISTPPerformanceCountersReader
    {
        private static readonly NullSTPInstancePerformanceCounters _instance = new NullSTPInstancePerformanceCounters();

        public static NullSTPInstancePerformanceCounters Instance
        {
            get { return _instance; }
        }

        public long ActiveThreads
        {
            get { return 0; }
        }

        public long InUseThreads
        {
            get { return 0; }
        }

        public long WorkItemsProcessed
        {
            get { return 0; }
        }

        public long WorkItemsQueued
        {
            get { return 0; }
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        public void SampleThreads(long activeThreads, long inUseThreads)
        {
        }

        public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
        {
        }

        public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
        {
        }

        public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
        {
        }
    }

    internal class STPInstanceNullPerformanceCounter : STPInstancePerformanceCounter
    {
        // Methods
        public override void Increment()
        {
        }

        public override void IncrementBy(long value)
        {
        }

        public override void Set(long val)
        {
        }
    }

    internal class STPInstancePerformanceCounter : IDisposable
    {
        // Fields
        private bool _isDisposed;

        private PerformanceCounter _pcs;

        public STPInstancePerformanceCounter(
                    string instance,
                    STPPerformanceCounterType spcType)
            : this()
        {
            STPPerformanceCounters counters = STPPerformanceCounters.Instance;
            _pcs = new PerformanceCounter(
                STPPerformanceCounters._stpCategoryName,
                counters._stpPerformanceCounters[(int)spcType].Name,
                instance,
                false);
            _pcs.RawValue = _pcs.RawValue;
        }

        // Methods
        protected STPInstancePerformanceCounter()
        {
            _isDisposed = false;
        }
        public void Close()
        {
            if (_pcs != null)
            {
                _pcs.RemoveInstance();
                _pcs.Close();
                _pcs = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Close();
                }
            }
            _isDisposed = true;
        }

        public virtual void Increment()
        {
            _pcs.Increment();
        }

        public virtual void IncrementBy(long val)
        {
            _pcs.IncrementBy(val);
        }

        public virtual void Set(long val)
        {
            _pcs.RawValue = val;
        }
    }

    internal class STPInstancePerformanceCounters : ISTPInstancePerformanceCounters
    {
        private static readonly STPInstancePerformanceCounter _stpInstanceNullPerformanceCounter;
        private bool _isDisposed;

        // Fields
        private STPInstancePerformanceCounter[] _pcs;
        // Methods
        static STPInstancePerformanceCounters()
        {
            _stpInstanceNullPerformanceCounter = new STPInstanceNullPerformanceCounter();
        }

        public STPInstancePerformanceCounters(string instance)
        {
            _isDisposed = false;
            _pcs = new STPInstancePerformanceCounter[(int)STPPerformanceCounterType.LastCounter];

            // Call the STPPerformanceCounters.Instance so the static constructor will
            // intialize the STPPerformanceCounters singleton.
            STPPerformanceCounters.Instance.GetHashCode();

            for (int i = 0; i < _pcs.Length; i++)
            {
                if (instance != null)
                {
                    _pcs[i] = new STPInstancePerformanceCounter(
                        instance,
                        (STPPerformanceCounterType)i);
                }
                else
                {
                    _pcs[i] = _stpInstanceNullPerformanceCounter;
                }
            }
        }

        public void Close()
        {
            if (null != _pcs)
            {
                for (int i = 0; i < _pcs.Length; i++)
                {
                    if (null != _pcs[i])
                    {
                        _pcs[i].Dispose();
                    }
                }
                _pcs = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Close();
                }
            }
            _isDisposed = true;
        }

        public void SampleThreads(long activeThreads, long inUseThreads)
        {
            GetCounter(STPPerformanceCounterType.ActiveThreads).Set(activeThreads);
            GetCounter(STPPerformanceCounterType.InUseThreads).Set(inUseThreads);
            GetCounter(STPPerformanceCounterType.OverheadThreads).Set(activeThreads - inUseThreads);

            GetCounter(STPPerformanceCounterType.OverheadThreadsPercentBase).Set(activeThreads - inUseThreads);
            GetCounter(STPPerformanceCounterType.OverheadThreadsPercent).Set(inUseThreads);
        }

        public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
        {
            GetCounter(STPPerformanceCounterType.WorkItems).Set(workItemsQueued + workItemsProcessed);
            GetCounter(STPPerformanceCounterType.WorkItemsInQueue).Set(workItemsQueued);
            GetCounter(STPPerformanceCounterType.WorkItemsProcessed).Set(workItemsProcessed);

            GetCounter(STPPerformanceCounterType.WorkItemsQueuedPerSecond).Set(workItemsQueued);
            GetCounter(STPPerformanceCounterType.WorkItemsProcessedPerSecond).Set(workItemsProcessed);
        }

        public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
        {
            GetCounter(STPPerformanceCounterType.AvgWorkItemProcessTime).IncrementBy((long)workItemProcessTime.TotalMilliseconds);
            GetCounter(STPPerformanceCounterType.AvgWorkItemProcessTimeBase).Increment();
        }

        public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
        {
            GetCounter(STPPerformanceCounterType.AvgWorkItemWaitTime).IncrementBy((long)workItemWaitTime.TotalMilliseconds);
            GetCounter(STPPerformanceCounterType.AvgWorkItemWaitTimeBase).Increment();
        }

        private STPInstancePerformanceCounter GetCounter(STPPerformanceCounterType spcType)
        {
            return _pcs[(int)spcType];
        }
    }

    /// <summary>
    /// Summary description for STPPerformanceCounter.
    /// </summary>
    internal class STPPerformanceCounter
    {
        protected string _counterHelp;

        protected string _counterName;

        // Fields
        private readonly PerformanceCounterType _pcType;
        // Methods
        public STPPerformanceCounter(
            string counterName,
            string counterHelp,
            PerformanceCounterType pcType)
        {
            _counterName = counterName;
            _counterHelp = counterHelp;
            _pcType = pcType;
        }

        // Properties
        public string Name
        {
            get
            {
                return _counterName;
            }
        }

        public void AddCounterToCollection(CounterCreationDataCollection counterData)
        {
            CounterCreationData counterCreationData = new CounterCreationData(
                _counterName,
                _counterHelp,
                _pcType);

            counterData.Add(counterCreationData);
        }
    }

    internal class STPPerformanceCounters
    {
        internal const string _stpCategoryHelp = "SmartThreadPool performance counters";

        internal const string _stpCategoryName = "SmartThreadPool";

        // Fields
        internal STPPerformanceCounter[] _stpPerformanceCounters;

        private static readonly STPPerformanceCounters _instance;
        // Methods
        static STPPerformanceCounters()
        {
            _instance = new STPPerformanceCounters();
        }

        private STPPerformanceCounters()
        {
            STPPerformanceCounter[] stpPerformanceCounters = new STPPerformanceCounter[]
				{
					new STPPerformanceCounter("Active threads", "The current number of available in the thread pool.", PerformanceCounterType.NumberOfItems32),
					new STPPerformanceCounter("In use threads", "The current number of threads that execute a work item.", PerformanceCounterType.NumberOfItems32),
					new STPPerformanceCounter("Overhead threads", "The current number of threads that are active, but are not in use.", PerformanceCounterType.NumberOfItems32),
					new STPPerformanceCounter("% overhead threads", "The current number of threads that are active, but are not in use in percents.", PerformanceCounterType.RawFraction),
					new STPPerformanceCounter("% overhead threads base", "The current number of threads that are active, but are not in use in percents.", PerformanceCounterType.RawBase),

					new STPPerformanceCounter("Work Items", "The number of work items in the Smart Thread Pool. Both queued and processed.", PerformanceCounterType.NumberOfItems32),
					new STPPerformanceCounter("Work Items in queue", "The current number of work items in the queue", PerformanceCounterType.NumberOfItems32),
					new STPPerformanceCounter("Work Items processed", "The number of work items already processed", PerformanceCounterType.NumberOfItems32),

					new STPPerformanceCounter("Work Items queued/sec", "The number of work items queued per second", PerformanceCounterType.RateOfCountsPerSecond32),
					new STPPerformanceCounter("Work Items processed/sec", "The number of work items processed per second", PerformanceCounterType.RateOfCountsPerSecond32),

					new STPPerformanceCounter("Avg. Work Item wait time/sec", "The average time a work item supends in the queue waiting for its turn to execute.", PerformanceCounterType.AverageCount64),
					new STPPerformanceCounter("Avg. Work Item wait time base", "The average time a work item supends in the queue waiting for its turn to execute.", PerformanceCounterType.AverageBase),

					new STPPerformanceCounter("Avg. Work Item process time/sec", "The average time it takes to process a work item.", PerformanceCounterType.AverageCount64),
					new STPPerformanceCounter("Avg. Work Item process time base", "The average time it takes to process a work item.", PerformanceCounterType.AverageBase),

					new STPPerformanceCounter("Work Items Groups", "The current number of work item groups associated with the Smart Thread Pool.", PerformanceCounterType.NumberOfItems32),
				};

            _stpPerformanceCounters = stpPerformanceCounters;
            SetupCategory();
        }

        // Properties
        public static STPPerformanceCounters Instance
        {
            get
            {
                return _instance;
            }
        }

        private void SetupCategory()
        {
            if (!PerformanceCounterCategory.Exists(_stpCategoryName))
            {
                CounterCreationDataCollection counters = new CounterCreationDataCollection();

                for (int i = 0; i < _stpPerformanceCounters.Length; i++)
                {
                    _stpPerformanceCounters[i].AddCounterToCollection(counters);
                }

                PerformanceCounterCategory.Create(
                    _stpCategoryName,
                    _stpCategoryHelp,
                    PerformanceCounterCategoryType.MultiInstance,
                    counters);
            }
        }
    }
#endif
}