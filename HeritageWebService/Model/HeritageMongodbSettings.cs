﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageMongodbSettings : IHeritageMongodbSettings
    {
        public string ConnectionString { get; set; }
        public Collections Collections { get; set; }
        public string DatabaseName { get; set; }
    }

    public class Collections
    {
        public string BannerCollectionName { get; set; }
        public string NewsDetailCollectionName { get; set; }
        public string NewsListCollectionName { get; set; }
        public string HeritageProjectName { get; set; }
        public string HeritageProjectMainPageName { get; set; }
        public string HeritageProjectDetilName { get; set; }
        public string HeritageProjectInheritatePeople { get; set; }
        public string ForumsList { get; set; }
        public string ForumsDetail { get; set; }
        public string SpecialTopic { get; set; }
        public string SpecialTopicDetail { get; set; }
        public string PeopleMainPage { get; set; }
        public string PeopleList { get; set; }
        public string PeopleDetail { get; set; }
    }
    public interface IHeritageMongodbSettings
    {
        string ConnectionString { get; set; }
        Collections Collections { get; set; }
        string DatabaseName { get; set; }
    }
}
