using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransStarter_Task.WPFApplication.API.Xlsx;
internal class XlsxFolderManager
{
    internal XlsxFolderManager (string directoryRelativePath)
    {
        _directoryRelativePath = directoryRelativePath;
    }

    private readonly string _directoryRelativePath;

    internal string GetFullPathToFolder =>
        Path.Combine(Environment.CurrentDirectory, _directoryRelativePath);

    private string GetFileFullPath (string fileNameWithExtension) =>
        Path.Combine(GetFullPathToFolder, fileNameWithExtension);

    private const string EXCEPTION_FILE_ALREADY_EXISTS = "File with such name already exists";
    private const string EXCEPTION_FILE_NOT_FOUND = "No file with such name was found";

    internal bool CreateFileWithName (string nameWithExtension, out Stream stream, out Exception ex)
    {
        var path = GetFileFullPath(nameWithExtension);
        if ( !File.Exists(path) )
        {
            ex = new Exception();

            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory))
            {
                stream = new MemoryStream();
                ex = new Exception("Дериктория не может быть определена.");
                return false;
            }

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            stream = File.Create(path);
            return true;
        }

        stream = new MemoryStream([]);
        ex = new Exception(EXCEPTION_FILE_ALREADY_EXISTS);
        return false;
    }

    internal bool DeleteFileWithName (string nameWithExtension, out Exception ex)
    {
        ex = new Exception();
        var path = GetFileFullPath(nameWithExtension);

        if ( File.Exists(path) )
        {
            File.Delete(path);
            return true;
        }

        ex = new Exception(EXCEPTION_FILE_NOT_FOUND);
        return false;
    }

    internal bool OpenFile (string nameWithExtension, out Stream stream, out Exception ex)
    {
        var path = GetFileFullPath(nameWithExtension);

        if ( File.Exists(path) )
        {
            ex = new Exception();
            stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return true;
        }

        ex = new Exception(EXCEPTION_FILE_NOT_FOUND);
        stream = new MemoryStream([]);
        return false;
    }
}