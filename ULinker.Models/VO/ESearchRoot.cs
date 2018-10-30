using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Models.VO
{
    public class ESearchRoot<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int took { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string timed_out { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public _shards _shards { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Hits<T> hits { get; set; }
    }

    public class _shards
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int successful { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int skipped { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int failed { get; set; }
    }

    public class HitsItem<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string _index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T _source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Highlight highlight { get; set; }
    }

    public class Hits<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string max_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<HitsItem<T>> hits { get; set; }
    }

    public class Highlight
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> Description { get; set; }
    }
}
