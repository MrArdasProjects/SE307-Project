namespace SE307_Project
{
    public class UniqueIdNode
    {
        private string id = string.Empty;

        public UniqueIdNode(string id)
        {
            SetId(id);
        }

        public bool ValidateId(string id)
        {
            if (id.Length < 1 || !char.IsLetter(id[0]))
            {
                throw new Exception(id + " is an invalid id!");
            }
            for (int i = 1; i < id.Length; i++)
            {
                if (!char.IsLetter(id[i]) && !char.IsDigit(id[i]) && id[i] != '_')
                {
                    throw new Exception(id + " is an invalid id!");
                }
            }
            return true;
        }

        public string GetId()
        {
            return id;
        }

        public void SetId(string id)
        {
            if (ValidateId(id))
            {
                this.id = id;
            }
        }

        public virtual void Print()
        {
            Console.WriteLine("UNIQUE ID NODE :" + id);
        }
    }
}
