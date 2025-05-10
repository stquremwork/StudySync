using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class IniFileHelper
{
    private string Path;

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public IniFileHelper(string iniFilePath)
    {
        Path = iniFilePath;
    }

    public void WriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    public string ReadValue(string section, string key, string defaultValue = "")
    {
        StringBuilder temp = new StringBuilder(255);
        int i = GetPrivateProfileString(section, key, defaultValue, temp, 255, Path);
        return temp.ToString();
    }

    public bool SectionExists(string section)
    {
        // Простой способ проверить существование секции - попытаться прочитать ключ
        return ReadValue(section, null) != null;
    }
}