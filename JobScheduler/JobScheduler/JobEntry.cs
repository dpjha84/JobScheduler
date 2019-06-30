namespace JobScheduler
{
    /// <summary>
    /// JobEntry class
    /// </summary>
    public class JobEntry
    {
        public string Name { get; set; }

        public string DependsOnJobName { get; set; }
    }
}
