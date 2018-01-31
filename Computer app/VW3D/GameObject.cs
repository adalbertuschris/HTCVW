using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DIL;
using OpenTK;
using VW3D.Extensions;

namespace VW3D
{
    class GameObject
    {
        public Model Model { get; private set; }
        public Physic.Transform Transform { get; private set; }
        public Part SelectedPart { get; set; }

        private CustomEnumerator _partEnumerator;

        public GameObject(M3DIL.Model model)
        {
            Model = model;
            Transform = new Physic.Transform();
        }

        public void SelectNextPart()
        {
            if (_partEnumerator == null)
            {
                _partEnumerator = Model.Parts.GetCustomEnumerator();
            }
            else
            {
                SelectedPart.Selected = false;
            }

            if (_partEnumerator.MoveNext())
            {
                SelectedPart = _partEnumerator.Current;
                SelectedPart.Selected = true;
            }
            else
            {
                _partEnumerator = null;
                SelectedPart = null;
            }
        }

        public void SelectPrevPart()
        {
            if (_partEnumerator == null)
            {
                _partEnumerator = Model.Parts.GetCustomEnumerator();
            }
            else
            {
                SelectedPart.Selected = false;
            }

            if (_partEnumerator.MovePrev())
            {
                SelectedPart = _partEnumerator.Current;
                SelectedPart.Selected = true;
            }
            else
            {
                _partEnumerator = null;
                SelectedPart = null;
            }
        }
    }
}
