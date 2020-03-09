using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageProject
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ObjId { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("auto_id")]
        public string Auto_id { get; set; }//项目序号
        [BsonElement("num")]
        public string Num { get; set; } //编号
        [BsonElement("title")]
        public string Title { get; set; } //名称
        [BsonElement("type")]
        public string Type { get; set; } //类别
        [BsonElement("reg_type")]
        public string Reg_type { get; set; }
        [BsonElement("rx_time")]
        public string Rx_time { get; set; } //公布时间
        [BsonElement("project_num")]
        public string Project_num { get; set; }
        [BsonElement("cate")]
        public string Cate { get; set; } //类型
        [BsonElement("province")]
        public string Province { get; set; } //申报地区或单位
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("playtype")]
        public string Playtype { get; set; }
        [BsonElement("unit")]
        public string Unit { get; set; }
        [BsonElement("link")]
        public string Link { get; set; }
    }
}
