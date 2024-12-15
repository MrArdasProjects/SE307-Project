using System;

namespace SE307_Project
{
    public class JobParser : ParserBase
    {
        private UniqueIdList jobList;
        private UniqueIdList taskList;

        public JobParser(UniqueIdList taskList)
        {
            this.taskList = taskList;
        }

        public UniqueIdList ParseJobList(string jobTypes)
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
            return jobList;
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
