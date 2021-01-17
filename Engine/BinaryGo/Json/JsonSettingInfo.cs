using System;
using System.Text;

namespace BinaryGo.Json
{
    /// <summary>
    /// Default settings for serializer and deserialier
    /// </summary>
    public class JsonConstantsString
    {
        //SkipValues = ' ', '\r', '\n', '\t'
        //EndsValues = '}', ']'
        //UnSupportedValue = ' ', ',', '\r', '\n', '\t' 
        //static byte[] SupportedValue { get; set; } = "0123456789.truefalsTRUEFALS-n".Select(x => (byte)x).ToArray();

        #region SkipValues
        /// <summary>
        /// New line \r Space
        /// </summary>
        public const char RSpace = '\r';
        /// <summary>
        /// New line \n Space
        /// </summary>
        public const char NSpace = '\n';
        /// <summary>
        /// \t Space
        /// </summary>
        public const char TSpace = '\t';
        /// <summary>
        /// Space
        /// </summary>
        public const char Space = ' ';
        #endregion
        /// <summary>
        /// Null value in memory
        /// </summary>
        public const string Null = "null";
        /// <summary>
        /// \n bytes
        /// </summary>
        public static readonly string BackSlashN = "\\n";
        /// <summary>
        /// \r bytes
        /// </summary>
        public static readonly string BackSlashR = "\\r";
        /// <summary>
        /// \t bytes
        /// </summary>
        public static readonly string BackSlashT = "\\t";
        /// <summary>
        /// \\" bytes
        /// </summary>
        public static readonly string BackSlashQuotes = "\\\"";

        /// <summary>
        /// Before create object with refrence
        /// </summary>
        public const string BeforeObjectReference = "{\"$id\":\"";
        /// <summary>
        /// After array with reference
        /// </summary>
        public const string AfterArrayObjectReference = "\",\"$values\":[";
        /// <summary>
        /// Open Square Brackets
        /// </summary>
        public const char OpenSquareBrackets = '[';
        /// <summary>
        /// Close Square Brackets
        /// </summary>
        public const char CloseSquareBrackets = ']';
        /// <summary>
        /// Close Square Brackets With Brackets
        /// </summary>
        public const string CloseSquareBracketsWithBrackets = "]}";
        /// <summary>
        /// Comma
        /// </summary>
        public const char Comma = ',';
        /// <summary>
        /// Comma with quotes
        /// </summary>
        public const string CommaQuotes = "\",";
        /// <summary>
        /// Colon with quotes
        /// </summary>
        public const string ColonQuotes = ":\"";
        /// <summary>
        /// Comma with quotes
        /// </summary>
        public const string QuotesColon = "\":";
        /// <summary>
        /// Quotes then colon, then quotes
        /// </summary>
        public const string QuotesColonQuotes = "\":\"";
        /// <summary>
        /// Colon
        /// </summary>
        public const char Colon = ':';
        /// <summary>
        /// Open braket
        /// </summary>
        public const char OpenBraket = '{';
        /// <summary>
        /// Open braket colon quotes with reference
        /// </summary>
        public const string OpenBraketColonQuotesReference = "{\"$ref\":\"";
        /// <summary>
        /// Close bracket
        /// </summary>
        public const char CloseBracket = '}';
        /// <summary>
        /// Quotes
        /// </summary>
        public const char Quotes = '"';
        /// <summary>
        /// Quotes close bracket
        /// </summary>
        public const string QuotesCloseBracket = "\"}";
        /// <summary>
        /// Back slash
        /// </summary>
        public const char BackSlash = '\\';
        /// <summary>
        /// $Id refrenced type name
        /// </summary>
        public const string IdRefrencedTypeName = "\"$id\"";
        /// <summary>
        /// $id for reference
        /// </summary>
        public const string IdRefrencedTypeNameNoQuotes = "$id";
        /// <summary>
        /// $Ref referenced type name
        /// </summary>
        public const string RefRefrencedTypeName = "\"$ref\"";
        /// <summary>
        /// $ref for reference
        /// </summary>
        public const string RefRefrencedTypeNameNoQuotes = "$ref";
        /// <summary>
        /// $Values referenced type name
        /// </summary>
        public const string ValuesRefrencedTypeName = "\"$values\"";
        /// <summary>
        /// $values for reference
        /// </summary>
        public const string ValuesRefrencedTypeNameNoQuotes = "$values";
        /// <summary>
        /// Support of $id,$ref,$values for serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; } = true;
        /// <summary>
        /// \r char for new line usage
        /// </summary>
        public const char RNewLine = 'r';
        /// <summary>
        /// \n char for new line usage
        /// </summary>
        public const char NNewLine = 'n';
        /// <summary>
        ///  \t char for tab usage
        /// </summary>
        public const char TTabLine = 't';
        /// <summary>
        /// true value of boolean
        /// </summary>
        public const string True = "true";
        /// <summary>
        /// false value of boolean
        /// </summary>
        public const string False = "false";
    }

    /// <summary>
    /// Default settings for serializer and deserialier
    /// </summary>
    public class JsonConstantsBytes
    {
        //SkipValues = ' ', '\r', '\n', '\t'
        //EndsValues = '}', ']'
        //UnSupportedValue = ' ', ',', '\r', '\n', '\t' 
        //static byte[] SupportedValue { get; set; } = "0123456789.truefalsTRUEFALS-n".Select(x => (byte)x).ToArray();

        #region SkipValues
        /// <summary>
        /// New line \r Space
        /// </summary>
        public const byte RSpace = (byte)'\r';
        /// <summary>
        /// New line \n Space
        /// </summary>
        public const byte NSpace = (byte)'\n';
        /// <summary>
        /// \t Space
        /// </summary>
        public const byte TSpace = (byte)'\t';
        /// <summary>
        /// Space
        /// </summary>
        public const byte Space = (byte)' ';
        #endregion
        /// <summary>
        /// Null value in memory
        /// </summary>
        public static readonly byte[] Null = Encoding.ASCII.GetBytes("null");
        /// <summary>
        /// \n bytes
        /// </summary>
        public static readonly byte[] BackSlashN = Encoding.ASCII.GetBytes("\\n");
        /// <summary>
        /// \r bytes
        /// </summary>
        public static readonly byte[] BackSlashR = Encoding.ASCII.GetBytes("\\r");
        /// <summary>
        /// \t bytes
        /// </summary>
        public static readonly byte[] BackSlashT = Encoding.ASCII.GetBytes("\\t");
        /// <summary>
        /// \\" bytes
        /// </summary>
        public static readonly byte[] BackSlashQuotes = Encoding.ASCII.GetBytes("\\\"");

        /// <summary>
        /// Before create object with refrence
        /// </summary>
        public static readonly byte[] BeforeObjectReference = Encoding.ASCII.GetBytes("{\"$id\":\"");
        /// <summary>
        /// After array with reference
        /// </summary>
        public static readonly byte[] AfterArrayObjectReference = Encoding.ASCII.GetBytes("\",\"$values\":[");
        /// <summary>
        /// Open Square Brackets
        /// </summary>
        public const byte OpenSquareBrackets = (byte)'[';
        /// <summary>
        /// Close Square Brackets
        /// </summary>
        public const byte CloseSquareBrackets = (byte)']';
        /// <summary>
        /// Close Square Brackets With Brackets
        /// </summary>
        public static readonly byte[] CloseSquareBracketsWithBrackets = Encoding.ASCII.GetBytes("]}");
        /// <summary>
        /// Comma
        /// </summary>
        public const byte Comma = (byte)',';
        /// <summary>
        /// Comma with quotes
        /// </summary>
        public static readonly byte[] CommaQuotes = Encoding.ASCII.GetBytes("\",");
        /// <summary>
        /// Colon with quotes
        /// </summary>
        public static readonly byte[] ColonQuotes = Encoding.ASCII.GetBytes(":\"");
        /// <summary>
        /// Comma with quotes
        /// </summary>
        public static readonly byte[] QuotesColon = Encoding.ASCII.GetBytes("\":");
        /// <summary>
        /// Quotes then colon, then quotes
        /// </summary>
        public static readonly byte[] QuotesColonQuotes = Encoding.ASCII.GetBytes("\":\"");
        /// <summary>
        /// Colon
        /// </summary>
        public const byte Colon = (byte)':';
        /// <summary>
        /// Open braket
        /// </summary>
        public const byte OpenBraket = (byte)'{';
        /// <summary>
        /// Open braket colon quotes with reference
        /// </summary>
        public static readonly byte[] OpenBraketColonQuotesReference = Encoding.ASCII.GetBytes("{\"$ref\":\"");
        /// <summary>
        /// Close bracket
        /// </summary>
        public const byte CloseBracket = (byte)'}';
        /// <summary>
        /// Quotes
        /// </summary>
        public const byte Quotes = (byte)'"';
        /// <summary>
        /// Quotes close bracket
        /// </summary>
        public static readonly byte[] QuotesCloseBracket = Encoding.ASCII.GetBytes("\"}");
        /// <summary>
        /// Back slash
        /// </summary>
        public const byte BackSlash = (byte)'\\';
        /// <summary>
        /// $Id refrenced type name
        /// </summary>
        public static readonly byte[] IdRefrencedTypeName = Encoding.ASCII.GetBytes("\"$id\"");
        /// <summary>
        /// $id for reference
        /// </summary>
        public static readonly byte[] IdRefrencedTypeNameNoQuotes = Encoding.ASCII.GetBytes("$id");
        /// <summary>
        /// $Ref referenced type name
        /// </summary>
        public static readonly byte[] RefRefrencedTypeName = Encoding.ASCII.GetBytes("\"$ref\"");
        /// <summary>
        /// $ref for reference
        /// </summary>
        public static readonly byte[] RefRefrencedTypeNameNoQuotes = Encoding.ASCII.GetBytes("$ref");
        /// <summary>
        /// $Values referenced type name
        /// </summary>
        public static readonly byte[] ValuesRefrencedTypeName = Encoding.ASCII.GetBytes("\"$values\"");
        /// <summary>
        /// $values for reference
        /// </summary>
        public static readonly byte[] ValuesRefrencedTypeNameNoQuotes = Encoding.ASCII.GetBytes("$values");
        /// <summary>
        /// Support of $id,$ref,$values for serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; } = true;
        /// <summary>
        /// \r char for new line usage
        /// </summary>
        public const byte RNewLine = (byte)'r';
        /// <summary>
        /// \n char for new line usage
        /// </summary>
        public const byte NNewLine = (byte)'n';
        /// <summary>
        ///  \t char for tab usage
        /// </summary>
        public const byte TTabLine = (byte)'t';
        /// <summary>
        /// true value of boolean
        /// </summary>
        public static readonly byte[] True = Encoding.ASCII.GetBytes("true");
        /// <summary>
        /// false value of boolean
        /// </summary>
        public static readonly byte[] False = Encoding.ASCII.GetBytes("false");
    }
}
