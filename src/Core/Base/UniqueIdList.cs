namespace WorkFlowApp
{
    public class UniqueIdList
    {
        private List<UniqueIdNode> list;

        public UniqueIdList()
        {
            this.list = new List<UniqueIdNode>();
        }

        public UniqueIdList(List<UniqueIdNode> list)
        {
            this.list = list;
        }

        public void Add(UniqueIdNode node)
        {
            if (ContainsId(node.GetId()))
            {
                throw new Exception("The id " + node.GetId() + " was already used!");
            }
            list.Add(node);
        }

        public UniqueIdNode FindId(string id)
        {
            if (list == null)
            {
                return null;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetId().Equals(id))
                {
                    return list[i];
                }
            }
            return null;
        }

        public bool ContainsId(string id)
        {
            return FindId(id) != null;
        }

        public string GetIdAt(int i)
        {
            return list[i].GetId();
        }

        public UniqueIdNode GetElement(int i)
        {
            return list[i];
        }

        public int GetSize()
        {
            return list.Count;
        }

        public List<UniqueIdNode> GetList()
        {
            return list;
        }

        public void PrintList()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Print();
            }
        }
    }
}