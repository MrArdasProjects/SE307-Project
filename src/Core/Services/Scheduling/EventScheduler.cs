namespace WorkFlowApp
{
    public class EventScheduler
    {
        private int maxCapacity;
        private bool multiFlag;
        private bool fifoFlag;
        private List<Event> executionQueue;

        public EventScheduler(int maxCapacity, bool multiFlag, bool fifoFlag, List<Event> executionQueue)
        {
            this.maxCapacity = maxCapacity;
            this.multiFlag = multiFlag;
            this.fifoFlag = fifoFlag;
            this.executionQueue = executionQueue;
        }

        public void RunEvents(List<Event> eventList, Func<Task, double> randomSpeed, Action<string> log)
        {
            if (eventList == null)
            {
                throw new ArgumentNullException(nameof(eventList), "The event list cannot be null.");
            }
            if (executionQueue == null)
            {
                throw new InvalidOperationException("Execution queue is not initialized.");
            }

            Event[] concurrent = new Event[maxCapacity];
            int runCount = 0;

            if (fifoFlag)
            {
                for (int i = 0; i < executionQueue.Count; i++)
                {
                    if (runCount == maxCapacity)
                        break;

                    var currentEvent = executionQueue[i];
                    if (currentEvent.FinishTime == -1)
                    {
                        if (currentEvent.PreviousEvent == null || currentEvent.PreviousEvent.FinishTime != -1)
                        {
                            if (currentEvent.PreviousEvent != null && currentEvent.StartTime < currentEvent.PreviousEvent.FinishTime)
                            {
                                currentEvent.StartTime = currentEvent.PreviousEvent.FinishTime;
                            }
                            if (multiFlag || concurrent[0] == null || currentEvent.Task.GetId().Equals(concurrent[0].Task.GetId()))
                            {
                                double speed = randomSpeed(currentEvent.Task);
                                int finish = (int)Math.Ceiling(currentEvent.StartTime + (currentEvent.Task.GetSize() / speed));
                                currentEvent.FinishTime = finish;
                                concurrent[runCount++] = currentEvent;

                                log($"Station started working on task {currentEvent.Task.GetId()} from {currentEvent.Job.GetId()} at time: {currentEvent.StartTime} and finishes it at time: {finish}");

                                eventList.Remove(currentEvent);
                            }
                        }
                    }
                }
            }
            else
            {
                while (runCount < maxCapacity)
                {
                    Event e = FindClosestDeadline();
                    if (e == null)
                        break;

                    if (e.PreviousEvent != null && e.StartTime < e.PreviousEvent.FinishTime)
                    {
                        e.StartTime = e.PreviousEvent.FinishTime;
                    }
                    double speed = randomSpeed(e.Task);
                    int finish = (int)Math.Ceiling(e.StartTime + (e.Task.GetSize() / speed));
                    e.FinishTime = finish;
                    concurrent[runCount++] = e;

                    log($"Station started working on task {e.Task.GetId()} from {e.Job.GetId()} at time: {e.StartTime} and finishes it at time: {finish}");

                    eventList.Remove(e);
                }
            }
        }

        private Event FindClosestDeadline()
        {
            int minIndex = -1;
            for (int i = 0; i < executionQueue.Count; i++)
            {
                if (executionQueue[i].FinishTime == -1)
                {
                    minIndex = i;
                    break;
                }
            }
            if (minIndex == -1)
            {
                return null;
            }
            for (int i = 0; i < executionQueue.Count; i++)
            {
                if (executionQueue[i].FinishTime == -1)
                {
                    if (executionQueue[i].PreviousEvent == null || executionQueue[i].PreviousEvent.FinishTime != -1)
                    {
                        if (executionQueue[i].Job.ExpectedFinishTime != null &&
                            executionQueue[minIndex].Job.ExpectedFinishTime != null &&
                            (int)executionQueue[i].Job.ExpectedFinishTime < (int)executionQueue[minIndex].Job.ExpectedFinishTime)
                        {
                            minIndex = i;
                        }
                    }
                }
            }
            return executionQueue[minIndex];
        }
    }
}