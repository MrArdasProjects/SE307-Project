using System;

namespace SE307_Project
{
    public class StationParser : ParserBase
    {
        private UniqueIdList stationList;
        private UniqueIdList taskList;

        public StationParser(UniqueIdList taskList)
        {
            this.taskList = taskList;
        }

        public UniqueIdList ParseStations(string stations)
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
            return stationList;
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
    }
}
