namespace WorkFlowApp
{
    public class FileIO
    {
        private string workFlowFileName;
        private string jobFileName;
        private string taskTypes;
        private string jobTypes;
        private string stations;
        private List<string> executableJobs;

        public FileIO(string workFlowFileName, string jobFileName)
        {
            this.workFlowFileName = workFlowFileName;
            this.jobFileName = jobFileName;
            taskTypes = "";
            jobTypes = "";
            stations = "";
            executableJobs = new List<string>();
        }

        public void ReadWorkFlowFile()
        {
            try
            {
                int readMode = 1; // 1: taskType, 2: jobType, 3: station
                using (StreamReader sr = new StreamReader(workFlowFileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        readMode = DetermineMode(line, readMode);
                        AddString(line, readMode);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Could not find the workflow file at :" + workFlowFileName);
            }
            catch (IOException)
            {
                throw new IOException("Error while reading workflow file");
            }
        }

        public void ReadJobFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(jobFileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        executableJobs.Add(line.Trim());
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Could not find the job file at :" + jobFileName);
            }
            catch (IOException)
            {
                throw new IOException("Error while reading job file");
            }
        }

        private int DetermineMode(string line, int currentMode)
        {
            if (line.Contains("(TASKTYPES"))
            {
                return 1;
            }
            else if (line.Contains("(JOBTYPES"))
            {
                return 2;
            }
            else if (line.Contains("(STATIONS"))
            {
                return 3;
            }
            else
            {
                return currentMode;
            }
        }

        private void AddString(string line, int readMode)
        {
            if (readMode == 1)
            {
                taskTypes += line.Trim() + " ";
            }
            else if (readMode == 2)
            {
                jobTypes += line.Trim() + " ";
            }
            else if (readMode == 3)
            {
                stations += line.Trim() + " ";
            }
        }

        public void CheckParanthesis()
        {
            string str = "";
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    str = taskTypes;
                }
                else if (i == 1)
                {
                    str = jobTypes;
                }
                else
                {
                    str = stations;
                }
                int paranthesis = 0;
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '(')
                    {
                        paranthesis++;
                    }
                    else if (str[j] == ')')
                    {
                        paranthesis--;
                    }
                }
                if (paranthesis != 0)
                {
                    string errorMessage = "Missing paranthesis at ";
                    if (i == 0)
                    {
                        errorMessage += "TASKTYPES";
                    }
                    else if (i == 1)
                    {
                        errorMessage += "JOBTYPES";
                    }
                    else
                    {
                        errorMessage += "STATIONS";
                    }
                    throw new Exception(errorMessage);
                }
            }
        }

        public string GetTaskTypes()
        {
            return taskTypes.Trim();
        }

        public string GetJobTypes()
        {
            return jobTypes.Trim();
        }

        public string GetStations()
        {
            return stations.Trim();
        }

        public List<string> GetExecutableJobs()
        {
            return executableJobs;
        }
    }
}