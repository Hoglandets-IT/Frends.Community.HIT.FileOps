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
        SFTP,
        FTP
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
        public Dictionary<int, string> Paths { get; set; }
        
        public CopyResult(
            bool success,
            int filesCopied,
            Dictionary<int, string> paths
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
            } 
            else if (serverType == "FTP"){
                ServerType = ServerTypes.FTP;
            }    
            else {
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
        /// <summary>
        /// The unique GUID
        /// </summary>
        [DefaultValue(0)]
        public string ObjectGuid { get; set; }

        /// <summary>
        /// The source server to get files from
        /// Needs to be a key in the server configuration file
        /// </summary>
        [DefaultValue(@"")]
        public string SourceServerEnvironment { get; set; }

        /// <summary>
        /// The path on the source server
        /// For SMB: ShareName/folder/path/
        /// For SFTP/SCP: /root/folder or folder/nonroot/
        /// </summary>
        [DefaultValue(@"")]
        public string SourcePath { get; set; }
        
        /// <summary>
        /// The regex pattern to match files to move
        /// </summary>
        [DefaultValue(@"")]
        public string SourcePattern { get; set; }

        /// <summary>
        /// The destination server to move files to
        /// Needs to be a key in the server configuration file
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationServerEnvironment { get; set; }

        /// <summary>
        /// The path on the destination server
        /// For SMB: ShareName/folder/path/
        /// For SFTP/SCP: /root/folder or folder/nonroot/
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationPath { get; set; }

        /// <summary>
        /// The filename to set on the destination server
        /// Available placeholders:
        /// {source_filename}: Source filename
        /// {date}: YYYY-mm-dd date
        /// {time}: hh-mm-ss time
        /// {guid}: Random GUID
        /// {sequential}: A sequential number unique for the ObjectGUID
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationFilename { get; set; }

        /// <summary>
        /// Whether to overwrite the destination file if it exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }

        [JsonConstructor]
        public MoveObject(
            string objectGuid,
            string sourceServerEnvironment,
            string sourcePath,
            string sourcePattern,
            string destinationServerEnvironment,
            string destinationPath,
            string destinationFilename,
            bool overwrite
        )
        {
            ObjectGuid = objectGuid;
            SourceServerEnvironment = sourceServerEnvironment;
            SourcePath = sourcePath;
            SourcePattern = sourcePattern;
            DestinationServerEnvironment = destinationServerEnvironment;
            DestinationPath = destinationPath;
            DestinationFilename = destinationFilename;
            Overwrite = overwrite;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ObjectGuid) &&
                !string.IsNullOrEmpty(SourceServerEnvironment) &&
                !string.IsNullOrEmpty(SourcePath) &&
                !string.IsNullOrEmpty(SourcePattern) &&
                !string.IsNullOrEmpty(DestinationServerEnvironment) &&
                !string.IsNullOrEmpty(DestinationPath) &&
                !string.IsNullOrEmpty(DestinationFilename);
        }
    }

    public class MoveObjectInput
    {
        /// <summary>
        /// The unique GUID
        /// </summary>
        [DefaultValue(0)]
        public string ObjectGuid { get; set; }

        /// <summary>
        /// The source server to get files from
        /// Needs to be a key in the server configuration file
        /// </summary>
        [DefaultValue(@"")]
        public string SourceServerEnvironment { get; set; }

        /// <summary>
        /// The path on the source server
        /// For SMB: ShareName/folder/path/
        /// For SFTP/SCP: /root/folder or folder/nonroot/
        /// </summary>
        [DefaultValue(@"")]
        public string SourcePath { get; set; }
        
        /// <summary>
        /// The regex pattern to match files to move
        /// </summary>
        [DefaultValue(@"")]
        public string SourcePattern { get; set; }

        /// <summary>
        /// The destination server to move files to
        /// Needs to be a key in the server configuration file
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationServerEnvironment { get; set; }

        /// <summary>
        /// The path on the destination server
        /// For SMB: ShareName/folder/path/
        /// For SFTP/SCP: /root/folder or folder/nonroot/
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationPath { get; set; }

        /// <summary>
        /// The filename to set on the destination server
        /// Available placeholders:
        /// {source_filename}: Source filename
        /// {date}: YYYY-mm-dd date
        /// {time}: hh-mm-ss time
        /// {guid}: Random GUID
        /// {sequential}: A sequential number unique for the ObjectGUID
        /// </summary>
        [DefaultValue(@"")]
        public string DestinationFilename { get; set; }

        /// <summary>
        /// Whether to overwrite the destination file if it exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }
    }

    public class MoveItemOutput 
    {
        /// <summary>
        /// Whether the object is valid
        /// </summary>
        /// <value></value>
        [DefaultValue(true)]
        public bool Valid { get; set; }

        /// <summary>
        /// The move object
        /// </summary>
        public MoveObject Mover { get; set; }

        public MoveItemOutput(
            bool valid,
            MoveObject mover
        ) {
            Valid = valid;
            Mover = mover;
        }
    }

    public class JoinMoveItemsInput
    {
        /// <summary>
        /// The move objects to join
        /// </summary>
        public List<MoveObject> MoveObjects { get; set; }
    }

    public class JoinMoveItemsOutput
    {
        /// <summary>
        /// A complete JSON string of the move objects
        /// </summary>
        public string MoveObjects { get; set; }

        public JoinMoveItemsOutput(
            string moveObjects
        ) {
            MoveObjects = moveObjects;
        }
    }
}