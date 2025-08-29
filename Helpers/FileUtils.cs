namespace BaseDotnet.Helpers;

public class FileUtils
{
    public static bool createFile(string path){
        string projectDirectory = AppContext.BaseDirectory;
        string TempPath = projectDirectory + path;
        if (!File.Exists(TempPath))
            Directory.CreateDirectory(TempPath);
        
        return File.Exists(TempPath);
    }

    public static bool createMediaDir(){
        return createFile(@"Uploads/" + DateTime.Now.ToString("yyyy/MM"));
    }

    public static string GetHtmlFromFile(string fileName)
    {
        try
        {
            string projectDirectory = AppContext.BaseDirectory;
            string filePath = projectDirectory + @"files/template/" + fileName;
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return string.Empty;
        }
    }

   public static long GetFileSize(string filePath)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting file size: {ex.Message}");
            return -1;
        }
    }

}