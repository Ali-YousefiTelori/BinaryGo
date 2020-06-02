using System;

namespace JsonGo.Json
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
    }

    //public class JsonConstantsBytes
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public const string Null = "null";
    //    public const char Space = ' ';
    //    public const string BeforeObject = "{\"$id\":\"";
    //    public const string AfterArrayObject = "\",\"$values\":[";
    //    public const char OpenSquareBrackets = '[';
    //    public const char CloseSquareBrackets = ']';
    //    public const string CloseSquareBracketsWithBrackets = "]}";
    //    public const char Comma = ',';
    //    public const string CommaQuotes = "\",";
    //    public const string ColonQuotes = ":\"";
    //    public const string QuotesColon = "\":";
    //    public const string QuotesColonQuotes = "\":\"";
    //    public const char Colon = ':';
    //    public const char OpenBraket = '{';
    //    public const string OpenBraketRefColonQuotes = "{\"$ref\":\"";
    //    public const char CloseBracket = '}';
    //    public const char Quotes = '"';
    //    public const string QuotesCloseBracket = "\"}";
    //    public const char BackSlash = '\\';
    //    public const char RNewLine = 'r';
    //    public const char NNewLine = 'n';
    //    /// <summary>
    //    /// $Id refrenced type name
    //    /// </summary>
    //    public const string IdRefrencedTypeName = "\"$id\"";
    //    public const string IdRefrencedTypeNameNoQuotes = "$id";
    //    /// <summary>
    //    /// $Ref refrenced type name
    //    /// </summary>
    //    public const string RefRefrencedTypeName = "\"$ref\"";
    //    public const string RefRefrencedTypeNameNoQuotes = "$ref";
    //    /// <summary>
    //    /// $Values refrenced type name
    //    /// </summary>
    //    public const string ValuesRefrencedTypeName = "\"$values\"";
    //    public const string ValuesRefrencedTypeNameNoQuotes = "$values";
    //    /// <summary>
    //    /// support for $id,$ref,$values for serialization
    //    /// </summary>
    //    public bool HasGenerateRefrencedTypes { get; set; } = true;

    //}
}
