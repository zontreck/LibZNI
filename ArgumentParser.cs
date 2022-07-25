using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI
{
    public class ArgumentParser
    {
        public static Arguments Parse(string[] lInputs)
        {
            Arguments a = new Arguments();
            
            for(int i = 0; i < lInputs.Length; i ++ )
            {
                if (lInputs[i].StartsWith("-"))
                {
                    try
                    {

                        if (lInputs[i + 1].StartsWith("-"))
                            a[lInputs[i]] = "1";
                        else
                        {
                            a[lInputs[i]] = lInputs[i + 1];
                        }
                    }catch(IndexOutOfRangeException e)
                    {
                        a[lInputs[i]] = "1";
                    }
                }
            }

            return a;
        }
    }
    public class Arguments : Dictionary<string, string>
    {
        
    }
}
