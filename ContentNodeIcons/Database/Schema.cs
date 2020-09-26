using Newtonsoft.Json;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace U8SK.ContentNodeIcons.Database
{
    [TableName("U8SK_ContentNodeIcons")]
    [PrimaryKey("ContentId", AutoIncrement = false)]
    [ExplicitColumns]
    public class Schema
    {
        [Column("ContentId")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        [Column("Icon")]
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [Column("IconColor")]
        [JsonProperty("iconColor")]
        public string IconColor { get; set; }
    }
}
