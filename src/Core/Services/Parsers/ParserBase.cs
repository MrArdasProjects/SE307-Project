namespace WorkFlowApp
{
    public abstract class ParserBase
    {
        protected string GetContentInParentheses(string str)
        {
            if (str.Contains("(") && str.Contains(")"))
            {
                return str.Substring(str.IndexOf('(') + 1, str.IndexOf(')') - str.IndexOf('(') - 1);
            }
            return null;
        }

        protected int IsTaskWithSize(string[] arr, int i, bool checkDouble)
        {
            if (i == arr.Length - 1)
            {
                return 0;
            }
            else
            {
                try
                {
                    double nextElem = double.Parse(arr[i + 1]);
                    if (checkDouble && i < arr.Length - 2)
                    {
                        double plusMinus = double.Parse(arr[i + 2]);
                        return 2;
                    }
                    return 1;
                }
                catch (FormatException)
                {
                    if (checkDouble)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}