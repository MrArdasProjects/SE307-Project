using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE307_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string workflowFilePath = "resources/workflowFile.txt";
                string jobFilePath = "resources/jobFile.txt";

                FileIO reader = new FileIO(workflowFilePath, jobFilePath);

                reader.ReadWorkFlowFile();
                reader.ReadJobFile();

                reader.CheckParanthesis();

                Manager manager = new Manager();
                manager.ParseTaskList(reader.GetTaskTypes());
                manager.ParseJobList(reader.GetJobTypes());
                manager.ParseStations(reader.GetStations());
                manager.ConfirmTaskCoverage();
                manager.ParseExecutableJobs(reader.GetExecutableJobs());

                manager.Print();
                manager.StartJobs();
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an error: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
