using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using notes.Models;

namespace notes.Models
{
    public static class TextHolder {
        //public static string content { get; set; }
        private static Dictionary<string,string> _content;

        public static Dictionary<string,string> content {
            get {
                if(_content == null)
                {
                    _content = new Dictionary<string, string>();
                    return _content;
                }
                else{
                    return _content;
                }
            }
            
            set
            {
                _content = value;
            }

            }
    }
    
}