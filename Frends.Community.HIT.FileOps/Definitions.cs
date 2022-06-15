using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace Frends.Community.HIT.FileOps
{
    public enum FileEncodings
    {
        UTF_8,
        UTF_32,
        ISO_8859_1,
        ASCII,
        LATIN_1
    }

    public enum ServerTypes
    {
        SMB,
        SFTP
    }

    public static class TranslateEncoding
    {
       public static Encoding GetEncoding(FileEncodings encoding) {
            Encoding enc = Encoding.GetEncoding(
                    Enum.GetName(
                        typeof(FileEncodings), encoding
                    ).Replace(
                        "_", "-"
                    )
                );
            return enc;
        }
    }

    public class ListResult
    {
        /// <summary>
        /// The content of the file
        /// </summary>
        [DefaultValue(null)]
        public List<string> ResultData { get; set; }

        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

        public ListResult(bool success, List<string> result)
        {
            Success = success;
            ResultData = result;
        }
    }

    public class ReadResult
    {

        /// <summary>
        /// The content of the file
        /// </summary>
        [DefaultValue(null)]
        public string ResultData { get; set; }

        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

        /// <summary>
        /// The encoding used for the file
        /// </summary>
        public Encoding FileEncoding { get; set; }

        public ReadResult(bool success, string result, Encoding encoding)
        {
            Success = success;
            ResultData = result;
            FileEncoding = encoding;
        }
    }

    public class WriteResult
    {
        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

        /// <summary>
        /// The encoding used for the file
        /// </summary>
        public Encoding FileEncoding { get; set; }

        public WriteResult(bool success, Encoding encoding)
        {
            Success = success;
            FileEncoding = encoding;
        }
    }

    public class CopyResult
    {
        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

        /// <summary>
        /// Indicates if a file was actually copied
        /// </summary>
        public int FilesCopied { get; set; }

        /// <summary>
        /// The paths that were copied (src: dst)
        /// </summary>
        public Dictionary<string, string> Paths { get; set; }
        
        public CopyResult(
            bool success,
            int filesCopied,
            Dictionary<string, string> paths
        )
        {
            Success = success;
            FilesCopied = filesCopied;
            Paths = paths;
        }
    }

    public class ServerConfiguration
    {
        /// <summary>
        /// The server type
        /// </summary>
        [DefaultValue(ServerTypes.SMB)]
        public ServerTypes ServerType { get; set; }

        /// <summary>
        /// The server name
        /// </summary>
        [DefaultValue(@"")]
        public string Server { get; set; }

        /// <summary>
        /// The server username
        /// </summary>
        [DefaultValue(@"")]
        public string Username { get; set; }

        /// <summary>
        /// The server password
        /// </summary>
        /// 
        [DefaultValue(@"")]
        public string Password { get; set; }

        /// <summary>
        /// The server port
        /// </summary>
        [DefaultValue(@"")]
        public string PrivateKey { get; set; } = "";

        /// <summary>
        /// The server fingerprint
        /// </summary>
        [DefaultValue(@"")]
        public string Fingerprint { get; set; } = "";

        /// <summary>
        /// The account domain
        /// </summary>
        [DefaultValue(@"")]
        public string Domain { get; set; } = "";

        public ServerConfiguration(
            ServerTypes serverType,
            string server,
            string username,
            string password = "",
            string privateKey = "",
            string fingerprint = "",
            string domain = ""
        )
        {
            Server = server;
            Username = username;
            Password = password;
            PrivateKey = privateKey;
            Fingerprint = fingerprint;
            Domain = domain;
        }

        [JsonConstructor]
        public ServerConfiguration(
            string serverType,
            string server,
            string username,
            string password = "",
            string privateKey = "",
            string fingerprint = "",
            string domain = ""
        )
        {
            if (serverType == "SFTP") {
                ServerType = ServerTypes.SFTP;
            } else {
                ServerType = ServerTypes.SMB;
            }
            Server = server;
            Username = username;
            Password = password;
            PrivateKey = privateKey;
            Fingerprint = fingerprint;
            Domain = domain;
        }
    }

    public class MoveObject
    {
        [DefaultValue(0)]
        public int ObjectID { get; set; }

        [DefaultValue(@"")]
        public string SourceServer { get; set; }

        [DefaultValue(@"")]
        public string SourcePath { get; set; }

        [DefaultValue(@"")]
        public string SourcePattern { get; set; }

        [DefaultValue(@"")]
        public string DestinationServer { get; set; }

        [DefaultValue(@"")]
        public string DestinationPath { get; set; }

        [DefaultValue(@"")]
        public string DestinationFilename { get; set; }

        [DefaultValue(false)]
        public bool Overwrite { get; set; }

        [JsonConstructor]
        public MoveObject(
            int objectID,
            string sourceServer,
            string sourcePath,
            string sourcePattern,
            string destinationServer,
            string destinationPath,
            string destinationFilename,
            bool overwrite
        )
        {
            SourceServer = sourceServer;
            SourcePath = sourcePath;
            SourcePattern = sourcePattern;
            DestinationServer = destinationServer;
            DestinationPath = destinationPath;
            DestinationFilename = destinationFilename;
            Overwrite = overwrite;
        }
    }


}