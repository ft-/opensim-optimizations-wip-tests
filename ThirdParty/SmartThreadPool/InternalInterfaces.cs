namespace Amib.Threading.Internal
{
    /// <summary>
    /// An internal delegate to call when the WorkItem starts or completes
    /// </summary>
    internal delegate void WorkItemStateCallback(WorkItem workItem);

    public interface IHasWorkItemPriority
    {
        WorkItemPriority WorkItemPriority { get; }
    }

    internal interface IInternalWaitableResult
    {
        /// <summary>
        /// This method is intent for internal use.
        /// </summary>
        IWorkItemResult GetWorkItemResult();
    }

    internal interface IInternalWorkItemResult
    {
        event WorkItemStateCallback OnWorkItemCompleted;

        event WorkItemStateCallback OnWorkItemStarted;
    }
}