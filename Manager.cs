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




    }
}
