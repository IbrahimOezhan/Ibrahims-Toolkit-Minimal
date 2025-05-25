using System.IO;

namespace TemplateTools
{
    public class File_Utilities
    {
        public static void WriteToFile(string filePath, string fileContent)
        {
            using StreamWriter writer = new(filePath);
            writer.Write(fileContent);

            Debug.Log("Successfully written \n" + fileContent + " to " + filePath);
        }

        public static string ReadFromFile(string filePath)
        {
            string fileContent = string.Empty;

            bool fileExists = File.Exists(filePath);

            if (fileExists)
            {
                using StreamReader reader = new(filePath);
                fileContent = reader.ReadToEnd();

                Debug.Log("Successfully read \n" + fileContent + " from " + filePath);
            }
            else
            {
                Debug.Log("File at "+ filePath + " does not exist");
            }

            return fileContent;
        }
    }
}


