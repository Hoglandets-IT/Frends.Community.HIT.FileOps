using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading;

#pragma warning disable 1591

namespace Frends.Community.HIT.FileOps.SMB
{
    public class SMBReadInput
    {
        /// <summary>
        /// The SMB Path
        /// </summary>
        [DefaultValue(@"smb://domain;username:password@server01.internal.com/sharename/path/to/file.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        [DefaultValue(FileEncodings.UTF_8)]
        public FileEncodings OutputEncoding { get; set; }

    }

    public class WriteInput
    {
        /// <summary>
        /// The file contents to write
        /// </summary>
        [DefaultValue(@"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Content { get; set; }

        /// <summary>
        /// The path to the SMB file
        /// </summary>
        [DefaultValue(@"someserver.internal.com/share/path/to/file.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// The username to access the SMB file
        /// </summary>
        [DefaultValue("username")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Username { get; set; }

        /// <summary>
        /// The password to access the SMB file
        /// </summary>
        [PasswordPropertyText(true)]
        [DefaultValue("\"\"")]
        public string Password { get; set; }

        /// <summary>
        /// The domain to access the SMB file
        /// </summary>
        [DefaultValue("WORKGROUP")]
        public string Domain { get; set; }

        /// <summary>
        /// Overwrite the file if exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }
    }

    public class CopyInput
    {
        /// <summary>
        /// The source path to the SMB file
        /// </summary>
        [DefaultValue(@"someserver.internal.com/share/path/to/file.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string SourcePath { get; set; }

        /// <summary>
        /// The destination path to the SMB file
        /// </summary>
        [DefaultValue(@"someserver.internal.com/share/path/to/file.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string DestPath { get; set; }

        /// <summary>
        /// The source username to access the SMB file
        /// </summary>
        [DefaultValue("username")]
        [DisplayFormat(DataFormatString = "Text")]
        public string SourceUsername { get; set; }

        /// <summary>
        /// The source password to access the SMB file
        /// </summary>
        [PasswordPropertyText(true)]
        [DefaultValue("\"\"")]
        public string SourcePassword { get; set; }

        /// <summary>
        /// The source domain to access the SMB file
        /// </summary>
        [DefaultValue("WORKGROUP")]
        public string SourceDomain { get; set; }

        /// <summary>
        /// The Destination username to access the SMB file
        /// </summary>
        [DefaultValue("username")]
        [DisplayFormat(DataFormatString = "Text")]
        public string DestinationUsername { get; set; }

        /// <summary>
        /// The Destination password to access the SMB file
        /// </summary>
        [PasswordPropertyText(true)]
        [DefaultValue("\"\"")]
        public string DestinationPassword { get; set; }

        /// <summary>
        /// The Destination domain to access the SMB file
        /// </summary>
        [DefaultValue("WORKGROUP")]
        public string DestinationDomain { get; set; }

        /// <summary>
        /// Overwrite the file if exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }

    }

    public class JsonMoveInput
    {
        /// <summary>
        /// The JSON file contents with settings
        /// </summary>
        [DefaultValue(@"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string SettingJson { get; set; }
    }

}