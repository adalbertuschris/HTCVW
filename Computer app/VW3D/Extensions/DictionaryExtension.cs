using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DIL;

namespace VW3D.Extensions
{
    static class DictionaryExtension
    {
        public static CustomEnumerator GetCustomEnumerator(this Dictionary<string, Part> parts)
        {
            return new CustomEnumerator(parts.ToList());
        }        
    }

    class CustomEnumerator
    {
        private int _counter = -1;
        List<KeyValuePair<string, Part>> _parts;

        public Part Current;

        public CustomEnumerator (List<KeyValuePair<string, Part>> parts)
        {
            _parts = parts;
        }

        public bool MoveNext()
        {
            if (_parts.Count != 0)
            { 
                if((_parts.Count - 1) > _counter)                    
                {
                    Current = _parts[++_counter].Value;
                    return true;
                }
                else if((_parts.Count - 1) == _counter)
                {
                    _counter = -1;
                }
            }
            return false;
        }

        public bool MovePrev()
        {
            if (_parts.Count != 0)
            {
                if (_counter <= (_parts.Count) && _counter > 0)
                {
                    Current = _parts[--_counter].Value;
                    return true;
                }
                else if (_counter <= 0)
                {
                    if (_counter == 0)
                    {
                        _counter = -1;
                        return false;
                    }
                    _counter = _parts.Count;
                    Current = _parts[--_counter].Value;                    
                    return true;
                }
            }            
            return false;
        }
    }
}
