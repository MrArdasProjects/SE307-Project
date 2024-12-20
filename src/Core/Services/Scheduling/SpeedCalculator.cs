namespace WorkFlowApp
{
    public class SpeedCalculator
    {
        private UniqueIdList taskList;
        private List<double> plusMinusList;

        public SpeedCalculator(UniqueIdList taskList, List<double> plusMinusList)
        {
            this.taskList = taskList;
            this.plusMinusList = plusMinusList;
        }

        public double RandomSpeed(Task t)
        {
            Random rand = new Random();
            double speed = 0;

            for (int i = 0; i < taskList.GetSize(); i++)
            {
                Task task = (Task)taskList.GetElement(i);
                if (task.GetId() == t.GetId())
                {
                    speed = task.GetSize();
                    int randomLimits = (int)(plusMinusList[i] * 100) * 2;
                    if (randomLimits != 0)
                    {
                        int randomPercent = 100 + rand.Next(randomLimits);
                        randomPercent -= (int)(plusMinusList[i] * 100);
                        speed = (speed * randomPercent) / 100.0;
                    }
                    break;
                }
            }
            return speed;
        }
    }
}