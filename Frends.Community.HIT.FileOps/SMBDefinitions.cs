using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Frends.Community.HIT.FileOps
{
    public class SMBListInput
    {
        /// <summary>
        /// The SMB Path
        /// </summary>
        [DefaultValue(@"smb://domain;username:password@server01.internal.com/sharename/path/to/folder/")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        public SMBListInput(string path)
        {
            Path = path;
        }
    }

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
        public FileEncodings Encoding { get; set; }

        public SMBReadInput(string path, FileEncodings encoding = FileEncodings.UTF_8)
        {
            Path = path;
            Encoding = encoding;
        }

    }

    public class SMBWriteInput
    {
        /// <summary>
        /// The SMB Path
        /// </summary>
        [DefaultValue(@"smb://domain;username:password@server01.internal.com/sharename/path/to/file.txt")]
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

        public SMBWriteInput(string path, string content, bool overwrite = false, FileEncodings encoding = FileEncodings.UTF_8)
        {
            Path = path;
            Content = content;
            Overwrite = Overwrite;
            Encoding = encoding;
        }
    }
}