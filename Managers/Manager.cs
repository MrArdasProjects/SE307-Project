using SE307_Project.Models;
using Task = SE307_Project.Models.Task;

namespace SE307_Project
{
    public class Manager
    {
        private UniqueIdList taskList;
        private UniqueIdList jobList;
        private UniqueIdList stationList;
        private UniqueIdList executableJobs;

        public void ParseTaskList(string taskTypes)
        {
            taskList = new UniqueIdList();
            if (!taskTypes.StartsWith("(TASKTYPES ") || !taskTypes.EndsWith(")"))
            {
                throw new Exception("Task types are not formatted correctly!");
            }
            string innerTasks = taskTypes.Substring(11);
            innerTasks = innerTasks.Substring(0, innerTasks.Length - 1);
            string[] parts = innerTasks.Split(' ');
            ParseTaskString(parts);
        }

        private void ParseTaskString(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int plus = IsTaskWithSize(arr, i, false);
                if (plus > 0)
                {
                    double defaultSize = double.Parse(arr[i + plus]);
                    Task newTask = new Task(arr[i], defaultSize);
                    taskList.Add(newTask);
                }
                else
                {
                    Task newTask = new Task(arr[i]);
                    taskList.Add(newTask);
                }
                i += plus;
            }
        }

        private int IsTaskWithSize(string[] arr, int i, bool checkDouble)
        {
            if (i == arr.Length - 1)
            {
                return 0;
            }
            else
            {
                try
                {
                    double nextElem = double.Parse(arr[i + 1]);
                    if (checkDouble && i < arr.Length - 2)
                    {
                        double plusMinus = double.Parse(arr[i + 2]);
                        return 2;
                    }
                    return 1;
                }
                catch (FormatException)
                {
                    if (checkDouble)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public void ParseJobList(string jobTypes)
        {
            jobList = new UniqueIdList();
            if (!jobTypes.StartsWith("(JOBTYPES ") || !jobTypes.EndsWith(")"))
            {
                throw new Exception("Job types are not formatted correctly!");
            }
            string innerJobs = jobTypes.Substring(10);
            innerJobs = innerJobs.Substring(0, innerJobs.Length - 1);
            string jobStr = "";
            do
            {
                jobStr = GetContentInParentheses(innerJobs);
                if (jobStr == null)
                    break;
                string[] parts = jobStr.Split(' ');
                ParseJobString(parts);
                innerJobs = innerJobs.Substring(innerJobs.IndexOf("(") + jobStr.Length + 2);
            } while (jobStr != null);
        }

        private string GetContentInParentheses(string str)
        {
            if (str.Contains("(") && str.Contains(")"))
            {
                return str.Substring(str.IndexOf('(') + 1, str.IndexOf(')') - str.IndexOf('(') - 1);
            }
            return null;
        }

        private void ParseJobString(string[] arr)
        {
            Job j = new Job(arr[0]);
            jobList.Add(j);
            for (int i = 1; i < arr.Length; i++)
            {
                int plus = IsTaskWithSize(arr, i, false);
                if (plus > 0)
                {
                    double size = double.Parse(arr[i + plus]);
                    if (!this.taskList.ContainsId(arr[i]))
                    {
                        throw new Exception("The task " + arr[i] + " was not defined before!");
                    }
                    else
                    {
                        Task newTask = new Task(arr[i], size);
                        j.GetTaskList().Add(newTask);
                    }
                }
                else
                {
                    if (!this.taskList.ContainsId(arr[i]))
                    {
                        throw new Exception("The task " + arr[i] + " was not defined before!");
                    }
                    else
                    {
                        Task original = (Task)this.taskList.FindId(arr[i]);
                        if (original.GetSize() <= 0.0)
                        {
                            throw new Exception("The task " + arr[i] + " does not have a default size!");
                        }
                        else
                        {
                            Task newTask = new Task(arr[i], original.GetSize());
                            j.GetTaskList().Add(newTask);
                        }
                    }
                }
                i += plus;
            }
        }

        public void ParseStations(string stations)
        {
            stationList = new UniqueIdList();
            if (!stations.StartsWith("(STATIONS ") || !stations.EndsWith(")"))
            {
                throw new Exception("Station types are not formatted correctly!");
            }
            string innerStations = stations.Substring(10);
            innerStations = innerStations.Substring(0, innerStations.Length - 1);
            string stationStr = "";
            do
            {
                stationStr = GetContentInParentheses(innerStations);
                if (stationStr == null)
                    break;
                string[] parts = stationStr.Split(' ');
                ParseStationString(parts);
                innerStations = innerStations.Substring(innerStations.IndexOf("(") + stationStr.Length + 2);
            } while (stationStr != null);
        }

        private void ParseStationString(string[] arr)
        {
            int maxCapacity = 1;
            Station s;
            bool multiFlag, fifoFlag;
            int start;
            try
            {
                maxCapacity = int.Parse(arr[1]);
                multiFlag = arr[2].Equals("Y");
                fifoFlag = arr[3].Equals("Y");
                start = 4;
            }
            catch (FormatException e)
            {
                maxCapacity = 1;
                multiFlag = arr[1].Equals("Y");
                fifoFlag = arr[2].Equals("Y");
                start = 3;
            }
            s = new Station(arr[0], maxCapacity, multiFlag, fifoFlag);
            stationList.Add(s);
            for (int i = start; i < arr.Length; i++)
            {
                int plus = IsTaskWithSize(arr, i, true);
                if (plus == 2)
                {
                    double speed = double.Parse(arr[i + 1]);
                    double plusMinus = double.Parse(arr[i + plus]);
                    if (!this.taskList.ContainsId(arr[i]))
                    {
                        throw new Exception("The task " + arr[i] + " was not defined before!");
                    }
                    else
                    {
                        Task newTask = new Task(arr[i], speed);
                        s.GetTaskList().Add(newTask);
                        s.GetPlusMinusList().Add(plusMinus);
                    }
                }
                else
                {
                    double speed = double.Parse(arr[i + 1]);
                    if (!this.taskList.ContainsId(arr[i]))
                    {
                        throw new Exception("The task " + arr[i] + " was not defined before!");
                    }
                    else
                    {
                        Task newTask = new Task(arr[i], speed);
                        s.GetTaskList().Add(newTask);
                        s.GetPlusMinusList().Add(0.0);
                    }
                }
                i += plus;
            }
        }

        public void ConfirmTaskCoverage()
        {
            for (int i = 0; i < jobList.GetSize(); i++)
            {
                Job job = (Job)jobList.GetElement(i);
                for (int j = 0; j < job.GetTaskList().GetSize(); j++)
                {
                    bool found = false;
                    for (int k = 0; k < stationList.GetSize(); k++)
                    {
                        Station station = (Station)stationList.GetElement(k);
                        if (station.GetTaskList().ContainsId(job.GetTaskList().GetIdAt(j)))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception("There are no stations to execute Task: " + job.GetTaskList().GetIdAt(j) + ", which is part of a job");
                    }
                }
            }
        }

        public void ParseExecutableJobs(List<string> executableJobStrings)
        {
            executableJobs = new UniqueIdList();
            for (int i = 0; i < executableJobStrings.Count; i++)
            {
                string[] parts = executableJobStrings[i].Split('\t');
                ParseExecutableJobString(parts);
            }
        }

        private void ParseExecutableJobString(string[] arr)
        {
            Job innerJob = (Job)jobList.FindId(arr[1]);
            int startTime = int.Parse(arr[2]);
            int duration = int.Parse(arr[3]);
            ExecutableJob ex = new ExecutableJob(arr[0], innerJob, startTime, duration);
            executableJobs.Add(ex);
        }

        public void Print()
        {
            Console.WriteLine("****************** TASK LIST  ******************");
            taskList.PrintList();
            Console.WriteLine("******************  JOB LIST  ******************");
            jobList.PrintList();
            Console.WriteLine("******************  STATIONS  ******************");
            stationList.PrintList();
            Console.WriteLine();
            Console.WriteLine();
        }

        public void StartJobs()
        {
            if (stationList == null || stationList.GetSize() == 0)
            {
                throw new Exception("Station list is not initialized or empty.");
            }

            Console.WriteLine("****************** STARTING JOBS ***************");
            List<Event> eventList = ExtractEvents();
            while (eventList.Count > 0)
            {
                for (int i = 0; i < stationList.GetSize(); i++)
                {
                    Station s = (Station)stationList.GetElement(i);
                    s.RunEvents(eventList);
                }
            }
        }

        private List<Event> ExtractEvents()
        {
            List<Event> eventList = new List<Event>();
            int[] previousIndexes = new int[executableJobs.GetSize()];
            for (int i = 0; i < previousIndexes.Length; i++)
            {
                previousIndexes[i] = -1;
            }
            for (int i = 0; i < executableJobs.GetSize(); i++)
            {
                ExecutableJob ex = (ExecutableJob)executableJobs.GetElement(i);
                Task t = ex.GetNextTask();
                List<Station> stations = FindAvailableStationsForTask(t);
                while (stations != null)
                {
                    Station s = FindSmallestQueue(stations);
                    Event eventObj;
                    if (previousIndexes[i] == -1)
                    {
                        eventObj = new Event(ex, s, t, null);
                    }
                    else
                    {
                        eventObj = new Event(ex, s, t, eventList[previousIndexes[i]]);
                    }
                    eventList.Add(eventObj);
                    s.AssignEvent(eventObj);
                    previousIndexes[i] = eventList.Count - 1;
                    Console.WriteLine("Assigned task " + t.GetId() + " from " + ex.GetId() + " (" + ex.GetInnerJob().GetId() + ") to station " + s.GetId());
                    t = ex.CalculateNextTask();
                    stations = FindAvailableStationsForTask(t);
                }
            }
            return eventList;
        }

        private Station FindSmallestQueue(List<Station> stations)
        {
            int min = 0;
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].GetExecutionQueue().Count < stations[min].GetExecutionQueue().Count)
                {
                    min = i;
                }
            }
            return stations[min];
        }

        private List<Station> FindAvailableStationsForTask(Task t)
        {
            if (t == null)
            {
                return null;
            }
            List<Station> availableList = new List<Station>();
            for (int i = 0; i < stationList.GetSize(); i++)
            {
                Station s = (Station)stationList.GetElement(i);
                if (s.GetTaskList().ContainsId(t.GetId()))
                {
                    availableList.Add(s);
                }
            }
            return availableList;
        }
    }
}