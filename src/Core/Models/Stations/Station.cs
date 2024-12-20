namespace WorkFlowApp
{
    public class Station : UniqueIdNode
    {
        private int maxCapacity;
        private bool multiFlag;
        private bool fifoFlag;
        private UniqueIdList taskList;
        private List<double> plusMinusList;
        private int idleTime;
        private int runTime;
        private List<Event> executionQueue;
        private EventScheduler eventScheduler;
        private SpeedCalculator speedCalculator;

        public Station(string id, int maxCapacity, bool multiFlag, bool fifoFlag) : base(id)
        {
            this.maxCapacity = maxCapacity;
            this.multiFlag = multiFlag;
            this.fifoFlag = fifoFlag;
            this.taskList = new UniqueIdList();
            this.plusMinusList = new List<double>();
            this.idleTime = 0;
            this.runTime = 0;
            this.executionQueue = new List<Event>();
            this.eventScheduler = new EventScheduler(maxCapacity, multiFlag, fifoFlag, executionQueue);
            this.speedCalculator = new SpeedCalculator(taskList, plusMinusList);
        }

        public void RunEvents(List<Event> eventList)
        {
            eventScheduler.RunEvents(eventList, speedCalculator.RandomSpeed, log => Console.WriteLine(log));
        }

        public void AssignEvent(Event evt)
        {
            executionQueue.Add(evt);
        }

        public int GetMaxCapacity() => maxCapacity;

        public void SetMaxCapacity(int maxCapacity) => this.maxCapacity = maxCapacity;

        public bool IsMultiFlag() => multiFlag;

        public void SetMultiFlag(bool multiFlag) => this.multiFlag = multiFlag;

        public bool IsFifoFlag() => fifoFlag;

        public void SetFifoFlag(bool fifoFlag) => this.fifoFlag = fifoFlag;

        public UniqueIdList GetTaskList() => taskList;

        public void SetTaskList(UniqueIdList taskList) => this.taskList = taskList;

        public List<double> GetPlusMinusList() => plusMinusList;

        public void SetPlusMinusList(List<double> plusMinusList) => this.plusMinusList = plusMinusList;

        public int GetIdleTime() => idleTime;

        public void SetIdleTime(int idleTime) => this.idleTime = idleTime;

        public List<Event> GetExecutionQueue() => executionQueue;

        public void SetExecutionQueue(List<Event> executionQueue) => this.executionQueue = executionQueue;

        public override void Print()
        {
            Console.WriteLine("---------");
            Console.WriteLine("Station - " + GetId() + " max capacity: " + maxCapacity);
            Console.Write("multiFlag: " + (multiFlag ? "Y" : "N") + "\t");
            Console.WriteLine("fifoFlag: " + (fifoFlag ? "Y" : "N"));
            Console.WriteLine("---------");
            Console.WriteLine("Tasks Done by Station: ");
            for (int i = 0; i < taskList.GetSize(); i++)
            {
                Task t = (Task)taskList.GetElement(i);
                Console.Write("Task - " + t.GetId() + " speed " + t.GetSize());
                if (plusMinusList[i] > 0.0)
                {
                    Console.Write(" +- " + (plusMinusList[i] * 100) + "%");
                }
                Console.WriteLine(" units/min");
            }
        }

        public void StatusReport()
        {
            Console.Write(GetId() + ": ");
            if (executionQueue.Count == 0)
            {
                Console.Write("Idle");
            }
            else
            {
            }
        }
    }
}