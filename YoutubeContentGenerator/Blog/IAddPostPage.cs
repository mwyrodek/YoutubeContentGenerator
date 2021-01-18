using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeContentGenerator.Blog
{
    public interface IAddPostPage
    {
        public IAddPostPage GoTo();
        public IAddPostPage AddPostBody(string body);
        public IAddPostPage AddPostTittle(string title);
        public IAddPostPage SetCategoty(string category);
        public IAddPostPage SchedulePost (DateTime date);
        public IAddPostPage OpenSettingsMenu();


    }
}
