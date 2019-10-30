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
        public string id;
        public string auto_id;//项目序号
        public string num; //编号
        public string title; //名称
        public string type; //类别
        public string reg_type;
        public string rx_time; //公布时间
        public string project_num;
        public string cate; //类型
        public string province; //申报地区或单位
        public string city;
        public string content;
        public string playtype;
        public string unit;
        public string link;
    }
}
