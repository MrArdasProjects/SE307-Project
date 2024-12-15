using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
            TaskParser taskParser = new TaskParser();
            taskList = taskParser.ParseTaskList(taskTypes);
        }

        public void ParseJobList(string jobTypes)
        {
            JobParser jobParser = new JobParser(taskList);
            jobList = jobParser.ParseJobList(jobTypes);
        }

        public void ParseStations(string stations)
        {
            StationParser stationParser = new StationParser(taskList);
            stationList = stationParser.ParseStations(stations);
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
