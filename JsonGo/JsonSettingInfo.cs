namespace JsonGo
{
    /// <summary>
    /// default setting of json serialize and deserialier
    /// </summary>
    public class JsonSettingInfo
    {
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
