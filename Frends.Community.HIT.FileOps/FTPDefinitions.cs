using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Frends.Community.HIT.FileOps
{
    public class FTPServerSettings
    {
        /// <summary>
        /// Server address : port
        /// </summary>
        [DefaultValue(@"ftp.example.com:22")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Server { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [DefaultValue(@"username")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Username { get; set; }

        /// <summary>
        /// Password for the user or keyfile
        /// </summary>
        [PasswordPropertyText]
        public string Password { get; set; }

        public string GetURL(string path)
        {
            var uri = new StringBuilder();
            uri.Append(Server);
            
            if (Server.EndsWith("/") == false && path.StartsWith("/") == false)
            {
                uri.Append("/");
            }
            uri.Append(path);
            if (path.EndsWith("/") == false)
            {
                uri.Append("/");
            }

            return uri.ToString();
        }

        public FTPServerSettings(
            string server,
            string username,
            string password = ""
        )
        {
            Server = server;
            Username = username;
            Password = password;
        }
    }

    public class FTPListInput
    {
        /// <summary>
        /// The path to the folder
        /// </summary>
        [DefaultValue(@"/")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        public FTPListInput(
            string path
        )
        {
            Path = path;
        }
    }

    public class FTPReadInput
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        [DefaultValue(@"/folderA/folderB/test.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        [DefaultValue(FileEncodings.UTF_8)]
        public FileEncodings Encoding { get; set; }

        public FTPReadInput(
            string path,
            FileEncodings encoding = FileEncodings.UTF_8
        )
        {
            Path = path;
            Encoding = encoding;
        }

    }

    public class FTPWriteInput
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        [DefaultValue(@"/folderA/folderB/test.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// Overwrite the file if exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }     
        
        /// <summary>
        /// The file contents to write
        /// </summary>
        [DefaultValue(@"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Content { get; set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        [DefaultValue(FileEncodings.UTF_8)]
        public FileEncodings Encoding { get; set; }

        public FTPWriteInput(
            string path,
            string content,
            bool overwrite = false,
            FileEncodings encoding = FileEncodings.UTF_8
        )
        {
            Path = path;
            Content = content;
            Encoding = encoding;
            Overwrite = overwrite;
        }
            
    }
}