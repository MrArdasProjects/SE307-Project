namespace SE307_Project
{
    public class Task : UniqueIdNode
    {
        private double size;

        public Task(string id) : base(id)
        {
            SetSize(0.0);
        }

        public Task(string name, double size) : base(name)
        {
            SetSize(size);
        }

        public double GetSize()
        {
            return size;
        }

        public void SetSize(double size)
        {
            if (size < 0.0)
            {
                throw new Exception("The task " + GetId() + " has a negative task size.");
            }
            this.size = size;
        }

        public override void Print()
        {
            Console.WriteLine("Task - " + GetId() + " size: " + size + " units");
        }
    }
}