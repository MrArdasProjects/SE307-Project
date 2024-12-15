using System;

namespace SE307_Project
{
    public class TaskParser : ParserBase
    {
        private UniqueIdList taskList;

        public UniqueIdList ParseTaskList(string taskTypes)
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
            return taskList;
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
    }
}
