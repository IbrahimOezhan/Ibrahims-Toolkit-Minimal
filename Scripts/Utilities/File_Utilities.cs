using System.IO;

namespace TemplateTools
{
    public class File_Utilities
    {
        public static void WriteToFile(string filePath, string fileContent, bool ifDoesntExist = false)
        {
            if (File.Exists(filePath) && ifDoesntExist) return;

            using StreamWriter writer = new(filePath);
            writer.Write(fileContent);
        }

        public static string ReadFromFile(string filePath)
        {
            string fileContent = string.Empty;

            bool fileExists = File.Exists(filePath);

            if (fileExists)
            {
                using StreamReader reader = new(filePath);
                fileContent = reader.ReadToEnd();
            }
            else
            {
                Debug.Log("File at "+ filePath + " does not exist");
            }

            return fileContent;
        }
    }
}


