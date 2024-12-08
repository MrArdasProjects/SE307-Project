namespace SE307_Project.Models
{
    public class ExecutableTask
    {
        private Task innerTask;
        private int startTime;
        private int deadline;
        private int finishTime;
        private string executableJobName;

        public ExecutableTask(Task innerTask, int startTime, int deadline, string executableJobName)
        {
            this.innerTask = innerTask;
            this.startTime = startTime;
            this.deadline = deadline;
            this.executableJobName = executableJobName;
        }

        public Task InnerTask
        {
            get { return innerTask; }
            set { innerTask = value; }
        }

        public int StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public int Deadline
        {
            get { return deadline; }
            set { deadline = value; }
        }

        public int FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }

        public string ExecutableJobName
        {
            get { return executableJobName; }
            set { executableJobName = value; }
        }
    }
}