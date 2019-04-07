using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Data;

namespace _4_04LogInsNew.Models
{
    public class HomePageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }

    }
    public class ViewModel
    {
        public bool ViewImage { get; set; }
        public string Message { get; set; }
        public Images ImageCurrent { get; set; }

    }
}