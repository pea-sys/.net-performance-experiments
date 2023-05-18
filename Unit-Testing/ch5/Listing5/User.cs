namespace ch5.Listing5
{
    public class User
    {
        public string Name { get; set; }

        public string NormalizeName(string name)
        {
            string result = (name ?? "").Trim();

            if (result.Length > 50)
                return result.Substring(0, 50);

            return result;
        }
    }
}
