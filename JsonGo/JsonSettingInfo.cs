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
        internal string IdRefrencedTypeName { get; set; } = "\"$id\"";
        /// <summary>
        /// $Ref refrenced type name
        /// </summary>
        internal string RefRefrencedTypeName { get; set; } = "\"$ref\"";
        /// <summary>
        /// $Values refrenced type name
        /// </summary>
        internal string ValuesRefrencedTypeName { get; set; } = "\"$values\"";
        /// <summary>
        /// support for $id,$ref,$values for serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; } = true;

    }
}
