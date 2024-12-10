using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE307_Project.Models
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
        }

        public void RunEvents(List<Event> eventList)
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
                                double speed = RandomSpeed(currentEvent.Task);
                                int finish = (int)Math.Ceiling(currentEvent.StartTime + (currentEvent.Task.GetSize() / speed));
                                currentEvent.FinishTime = finish;
                                concurrent[runCount++] = currentEvent;

                                Console.WriteLine("Station " + GetId() + " started working on task "
                                    + currentEvent.Task.GetId() + " from "
                                    + currentEvent.Job.GetId() + " (" +
                                    ((UniqueIdNode)currentEvent.Job.InnerJob)?.GetId() + ") at time: "
                                    + currentEvent.StartTime + " and finishes it at time: " + finish);

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
                    double speed = RandomSpeed(e.Task);
                    int finish = (int)Math.Ceiling(e.StartTime + (e.Task.GetSize() / speed));
                    e.FinishTime = finish;
                    concurrent[runCount++] = e;

                    Console.WriteLine("Station " + GetId() + " started working on task " + e.Task.GetId() + " from "
                        + e.Job.GetId() + ((UniqueIdNode)e.Job.InnerJob)?.GetId() + ") at time: "
                        + e.StartTime + " and finishes it at time: " + finish);

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

        public double RandomSpeed(Task t)
        {
            Random rand = new Random();
            double speed = 0;

            for (int i = 0; i < taskList.GetSize(); i++)
            {
                Task task = (Task)taskList.GetElement(i);
                if (task.GetId() == t.GetId())
                {
                    speed = task.GetSize();
                    int randomLimits = (int)(plusMinusList[i] * 100) * 2;
                    if (randomLimits != 0)
                    {
                        int randomPercent = 100 + rand.Next(randomLimits);
                        randomPercent -= (int)(plusMinusList[i] * 100);
                        speed = (speed * randomPercent) / 100.0;
                    }
                    break;
                }
            }

            return speed;
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
                // Additional status reporting logic can be added here
            }
        }
    }
}