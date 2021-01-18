using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.LoadData
{
    public interface ILoadData
    {
        List<Episode> Execute();
    }
}
