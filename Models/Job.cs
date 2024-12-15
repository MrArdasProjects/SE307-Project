using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE307_Project
{
    public class Job : UniqueIdNode
    {
        private UniqueIdList taskList;

        public Job(string id) : base(id)
        {
            this.taskList = new UniqueIdList();
        }

        public Job(string id, UniqueIdList taskList) : base(id)
        {
            this.taskList = taskList;
        }

        public UniqueIdList GetTaskList()
        {
            return taskList;
        }

        public void SetTaskList(UniqueIdList taskList)
        {
            this.taskList = taskList;
        }

        public override void Print()
        {
            Console.WriteLine("---------");
            Console.WriteLine("Job - " + GetId());
            Console.WriteLine("---------");
            Console.WriteLine("Tasks in Job: ");
            taskList.PrintList();
        }
    }

}
