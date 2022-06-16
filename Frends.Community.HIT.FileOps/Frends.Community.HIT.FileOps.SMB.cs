using System.Text;
using SharpCifs.Smb;

#pragma warning disable 1591
#pragma warning disable 8602

namespace Frends.Community.HIT.FileOps
{
    public class SMB
    {

        public static string GetSMBConnectionString(
            string server,
            string user,
            string password,
            string domain = "",
            string path = "",
            string file = ""
        )
        {
            var connectionString = new StringBuilder();
            connectionString.Append("smb://");
            if (!string.IsNullOrEmpty(domain))
            {
                connectionString.Append($"{domain};");
            }
            connectionString.Append($"{user}:{password}@");
            connectionString.Append(server);
            // Check if string starts with /, if not, add
            if (!path.StartsWith("/"))
            {
                connectionString.Append("/");
            }
            connectionString.Append($"{path}");
            
            // Check if string ends with /, if not, add
            if (path.Length > 0 && path.EndsWith("/") == false)
            {
                connectionString.Append("/");
            }
            connectionString.Append(file);

            return connectionString.ToString();
        }
        /// <summary>
        /// List files in directory on SMB share
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        public static ListResult ListFiles(SMBListInput input)
        {
            var folder = new SmbFile(input.Path);
            var list = folder.ListFiles();

            return new ListResult(true, list.Select(x => x.GetName()).ToList());
        }

        /// <summary>
        /// Read file from SMB path
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        public static ReadResult ReadFile(SMBReadInput input)
        {
            var file = new SmbFile(input.Path);
            String resultContent = "";
            Boolean resultSuccess = false;
            Encoding encType = HelperFunctions.TranslateEncoding.GetEncoding(input.Encoding);

            if (file.Exists())
            {
                var readStream = file.GetInputStream();
                var memStream = new MemoryStream();

                ((Stream)readStream).CopyTo(memStream);
                readStream.Dispose();

                resultContent = encType.GetString(memStream.ToArray());
                resultSuccess = true;

            }
            
            return new ReadResult(resultSuccess, resultContent, encType);
        }

        /// <summary>
        /// Write file to SMB Path
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        public static WriteResult WriteFile(SMBWriteInput input)
        {
            var file = new SmbFile(input.Path);
            Encoding encType = HelperFunctions.TranslateEncoding.GetEncoding(input.Encoding);

            if (!file.Exists())
            {
                file.CreateNewFile();
            }
            else 
            {
                if (input.Overwrite)
                {
                    file.Delete();
                    file.CreateNewFile();
                }
                else {
                    throw new Exception("File already exists and overwrite is not enabled");
                }
            }

            var writeStream = file.GetOutputStream();
            writeStream.Write(encType.GetBytes(input.Content));
            writeStream.Dispose();

            return new WriteResult(true, encType);
        }
    }
}