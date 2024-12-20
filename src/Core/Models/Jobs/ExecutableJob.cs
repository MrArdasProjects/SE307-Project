namespace WorkFlowApp
{
    public class ExecutableJob : UniqueIdNode
    {
        private Job innerJob;
        private int startTime;
        private int deadline;
        private int finishTime;
        private int expectedFinishTime;
        private string status;
        private Task nextTask;
        private int nextTaskIndex;

        public object InnerJob { get; internal set; }
        public object ExpectedFinishTime { get; internal set; }

        public ExecutableJob(string id, Job innerJob, int startTime, int deadline) : base(id)
        {
            this.innerJob = innerJob;
            SetStartTime(startTime);
            SetDeadline(deadline);
            this.expectedFinishTime = startTime + deadline;
            this.status = "Waiting";
            this.nextTaskIndex = 0;
            this.nextTask = CalculateNextTask();
        }

        public Task CalculateNextTask()
        {
            if (innerJob.GetTaskList().GetSize() == nextTaskIndex)
            {
                this.status = "Finished";
                return null;
            }
            else
            {
                Task t = (Task)innerJob.GetTaskList().GetElement(nextTaskIndex++);
                return t;
            }
        }

        public Job GetInnerJob()
        {
            return innerJob;
        }

        public void SetInnerJob(Job innerJob)
        {
            this.innerJob = innerJob;
        }

        public int GetStartTime()
        {
            return startTime;
        }

        public void SetStartTime(int startTime)
        {
            if (startTime < 0)
            {
                throw new Exception("The job " + GetId() + " starts before minute 0");
            }
            this.startTime = startTime;
        }

        public int GetDeadline()
        {
            return deadline;
        }

        public void SetDeadline(int deadline)
        {
            if (deadline <= 0)
            {
                throw new Exception("The job " + GetId() + " has an invalid deadline");
            }
            this.deadline = deadline;
        }

        public int GetFinishTime()
        {
            return finishTime;
        }

        public void SetFinishTime(int finishTime)
        {
            if (finishTime < startTime)
            {
                throw new Exception("The job " + GetId() + " has a finish time earlier than its start time");
            }
            this.finishTime = finishTime;
        }


        public int GetExpectedFinishTime()
        {
            return expectedFinishTime;
        }

        public void SetExpectedFinishTime(int expectedFinishTime)
        {
            this.expectedFinishTime = expectedFinishTime;
        }

        public string GetStatus()
        {
            return status;
        }

        public void SetStatus(string status)
        {
            this.status = status;
        }

        public Task GetNextTask()
        {
            return nextTask;
        }

        public void SetNextTask(Task nextTask)
        {
            this.nextTask = nextTask;
        }

        public override void Print()
        {
        }

        public void StatusReport()
        {
            Console.Write(GetId() + ": " + status);
        }
    }
}