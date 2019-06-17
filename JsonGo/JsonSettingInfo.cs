namespace JsonGo
{
    /// <summary>
    /// default setting of json serialize and deserialier
    /// </summary>
    public class JsonSettingInfo
    {
        internal const string BeforeObject = "{\"$id\":\"";
        internal const string AfterArrayObject = "\",\"$values\":[";
        internal const char OpenSquareBrackets = '[';
        internal const char CloseSquareBrackets = ']';
        internal const char Comma = ',';
        internal const string CommaQuotes = "\",";
        internal const string ColonQuotes = ":\"";
        internal const string QuotesColon = "\":";
        internal const char Colon = ':';
        internal const char OpenBraket = '{';
        internal const char CloseBracket = '}';
        internal const char Quotes = '"';
        /// <summary>
        /// $Id refrenced type name
        /// </summary>
        internal const string IdRefrencedTypeName = "\"$id\"";
        internal const string IdRefrencedTypeNameNoQuotes = "$id";
        /// <summary>
        /// $Ref refrenced type name
        /// </summary>
        internal const string RefRefrencedTypeName = "\"$ref\"";
        internal const string RefRefrencedTypeNameNoQuotes = "$ref";
        /// <summary>
        /// $Values refrenced type name
        /// </summary>
        internal const string ValuesRefrencedTypeName = "\"$values\"";
        internal const string ValuesRefrencedTypeNameNoQuotes = "$values";
        /// <summary>
        /// support for $id,$ref,$values for serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; } = true;

    }
}
