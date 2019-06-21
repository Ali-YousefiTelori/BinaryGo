namespace JsonGo
{
    /// <summary>
    /// default setting of json serialize and deserialier
    /// </summary>
    public class JsonSettingInfo
    {
        public const string Null = "null";
        public const string BeforeObject = "{\"$id\":\"";
        public const string AfterArrayObject = "\",\"$values\":[";
        public const char OpenSquareBrackets = '[';
        public const char CloseSquareBrackets = ']';
        public const string CloseSquareBracketsWithBrackets = "]}";
        public const char Comma = ',';
        public const string CommaQuotes = "\",";
        public const string ColonQuotes = ":\"";
        public const string QuotesColon = "\":";
        public const string QuotesColonQuotes= "\":\"";
        public const char Colon = ':';
        public const char OpenBraket = '{';
        public const string OpenBraketRefColonQuotes = "{\"$ref\":\"";
        public const char CloseBracket = '}';
        public const char Quotes = '"';
        public const string QuotesCloseBracket = "\"}";
        /// <summary>
        /// $Id refrenced type name
        /// </summary>
        public const string IdRefrencedTypeName = "\"$id\"";
        public const string IdRefrencedTypeNameNoQuotes = "$id";
        /// <summary>
        /// $Ref refrenced type name
        /// </summary>
        public const string RefRefrencedTypeName = "\"$ref\"";
        public const string RefRefrencedTypeNameNoQuotes = "$ref";
        /// <summary>
        /// $Values refrenced type name
        /// </summary>
        public const string ValuesRefrencedTypeName = "\"$values\"";
        public const string ValuesRefrencedTypeNameNoQuotes = "$values";
        /// <summary>
        /// support for $id,$ref,$values for serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; } = true;

    }
}
