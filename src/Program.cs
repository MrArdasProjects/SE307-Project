namespace WorkFlowApp
{
    public class Program
    {
        private static string logFilePath = "run_log.txt";

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: WorkFlowApp <workflowFilePath> <jobFilePath>");
                    return;
                }

                string workflowFilePath = args[0];
                string jobFilePath = args[1];

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

                LogRun("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an error: " + e.Message);
                Console.WriteLine(e.StackTrace);
                LogRun("Failure");
            }
        }

        private static void LogRun(string status)
        {
            int runCount = GetRunCount();
            string projectName = "WorkFlowApp";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{runCount}; {dateTime}; {projectName}[{runCount}] - {status}";

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logEntry);
            }
        }

        private static int GetRunCount()
        {
            if (!File.Exists(logFilePath))
            {
                return 1;
            }

            string[] logEntries = File.ReadAllLines(logFilePath);
            if (logEntries.Length == 0)
            {
                return 1;
            }

            string lastEntry = logEntries[logEntries.Length - 1];
            string[] parts = lastEntry.Split(';');
            if (parts.Length > 0 && int.TryParse(parts[0].Trim(), out int lastRunCount))
            {
                return lastRunCount + 1;
            }

            return 1;
        }
    }
}