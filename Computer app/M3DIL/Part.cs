using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace M3DIL
{   
    public class Part : INotifyPropertyChanged
    {        
        public List<Group> Groups = new List<Group>();
        public int Index { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        bool _selected;
        bool _visible = true;
        bool _transparent;
        private DispMode dispMode = DispMode.normal;

        private static int _index;
        private string _name;       

        public Part()
        {            
            Index = _index++;           
            _name = String.Format("#part{0}", Index);
        }             
        
        public string Name
        {
            get => _name;
            set => _name = value;            
        }

        public bool IsTextured { get; set; } = false;
     
        public bool Selected
        {
            get => _selected;
            set
            {
                foreach (var group in Groups)
                {
                    group.Selected = value;
                }
                _selected = value;
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                foreach (var group in Groups)
                {
                    group.Visible = value;
                }
                _visible = value;
            }
        }

        public bool Transparent
        {
            get => _transparent;
            set
            {
                foreach (var group in Groups)
                {
                    group.Transparent = value;
                }
                _transparent = value;
            }
        }        
        
        public DispMode DisplayMode
        {
            get => dispMode;
            set
            {
                switch (value)
                {
                    case DispMode.normal:
                        Visible = true;
                        Transparent = false;
                        break;
                    case DispMode.hide:
                        Visible = false;
                        break;                    
                    case DispMode.transparent:
                        Transparent = true;
                        break;
                    default:
                        break;
                }
                dispMode = value;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = default(string))
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }    
}
