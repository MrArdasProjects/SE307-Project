using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SE307_Project.Models
{
    public class Event
    {
        private ExecutableJob job;
        private Station station;
        private Task task;
        private Event previousEvent;
        private int startTime;
        private int finishTime;

        public Event(ExecutableJob job, Station station, Task task, Event previousEvent)
        {
            this.job = job;
            this.station = station;
            this.task = task;
            this.previousEvent = previousEvent;
            this.startTime = job.GetStartTime();
            this.finishTime = -1;
        }

        public ExecutableJob Job
        {
            get { return job; }
            set { job = value; }
        }

        public Station Station
        {
            get { return station; }
            set { station = value; }
        }

        public Task Task
        {
            get { return task; }
            set { task = value; }
        }

        public Event PreviousEvent
        {
            get { return previousEvent; }
            set { previousEvent = value; }
        }

        public int StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public int FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }

        public override string ToString()
        {
            return $"Event [job={job.GetId()}, station={station.GetId()}, task={task.GetId()}, startTime={startTime}, finishTime={finishTime}]";
        }
    }
}


