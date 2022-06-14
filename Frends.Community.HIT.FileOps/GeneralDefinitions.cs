using System;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading;

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
        /// Error message
        /// </summary>
        [DefaultValue(false)]
        public string Message { get; set; }

        public ReadResult(bool success, string result)
        {
            Success = success;
            ResultData = result;
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
        /// Error message
        /// </summary>
        [DefaultValue(false)]
        public string Message { get; set; }

        public WriteResult(bool success)
        {
            Success = success;
        }
    }

    public class CopyResult
    {
        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

       
        public CopyResult(bool success)
        {
            Success = success;
        }
    }

    public class JsonMoveResult
    {
        /// <summary>
        /// Bool whether the get was successful
        /// </summary>
        [DefaultValue(false)]

        public bool Success { get; set; }

        public JsonMoveResult(bool success)
        {
            Success = success;
        }
    }
}