using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace DataAccess.Models
{
    public class DirectedGraph<TValueType> where TValueType: IList<TValueType>,IEnumerable,ICollection, new()
    {
        public Dictionary<int, List<TValueType>> Nodes { get; set; } = new Dictionary<int, List<TValueType>>();
        public List<TValueType> this[int i]
        {
            get { return Nodes[i]; }
            set { if (Nodes.TryGetValue(i, out List<TValueType> val))
                    {
                        val.AddRange(value);
                    }
                    else
                    {
                        Nodes[i] =value;
                    }
                }
        }
        public TValueType this[int i, int i2]
        {
            get
            {
                if (Nodes.TryGetValue(i, out List<TValueType> values))
                {
                    return values.Count()<i2 ? values[i2]:default;
                }
                return default;
            }
            set
            {
                List<TValueType> val;
                if (Nodes.TryGetValue(i, out  val))
                {
                    if(val.Count<i2)
                    {
                        val[i2] = value;
                    }
                    else
                    {
                        List<TValueType> values = new List<TValueType>(i2 + 1);
                        for (int idx = 0; idx < values.Count; idx++)
                        {
                            values[idx] = new TValueType();
                            try
                            {
                                TValueType previousVal = values[idx];
                                values[idx] = previousVal;
                            }
                            catch (Exception)
                            {

                            }
                        }
                       val = values;
                    }
                    Nodes[i] = val;
                }
                else
                {
                    Nodes[i] = new List<TValueType>(i2 + 1);
                    Nodes[i][i2] = value;
                }
            }
        }
        //public List<TValueType> FindPath(TValueType initialItem, TValueType findValue,List<TValueType>  path)
        //{
        //    path.Add(initialItem);
        //    if (initialItem.Equals(findValue))
        //        return path;


        //}
        public void AddEdge(int index, TValueType value)
        {
            if(Nodes.TryGetValue(index, out List<TValueType> val))
            {
                val.Add(value);
                Nodes[index] =val ;
            }
            else
            {
                Nodes[index] = new List<TValueType>();
                Nodes[index].Add(value);
            }
        }
    }
}
