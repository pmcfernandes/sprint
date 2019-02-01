namespace Sprint.IO
{
    public class Paths
    {  
        /// <summary>
        /// Gets the assembly path.
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyPath()
        {
            return System.Reflection.Assembly.GetCallingAssembly().Location;
        }
    }
}
